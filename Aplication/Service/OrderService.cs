using Aplication.Exceptions;
using Aplication.Interfaces;
using Aplication.Models;
using Aplication.Responses;
using domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Service
{
    public class OrderService : IOrderService
    {
        private readonly IOrderCommand _orderCommand;
        private readonly IOrderItemCommand _orderItemCommand;
        private readonly IDishQuery _dishQuery;
        private readonly IOrderQuery _orderQuery;
        private readonly IStatusQuery _statusQuery;

        public OrderService(IOrderCommand orderCommand, IOrderItemCommand orderItemCommand, IDishQuery dishQuery, IOrderQuery orderQuery, IStatusQuery statusQuery)
        {
            _orderCommand = orderCommand;
            _orderItemCommand = orderItemCommand;
            _dishQuery = dishQuery;
            _orderQuery = orderQuery;
            _statusQuery = statusQuery;
        }

        public async Task<OrderResponse> CreateOrder(OrderModel order)
        {
            decimal totalAmount = 0;
            foreach (var item in order.items)
            {
                var dish = await _dishQuery.GetDishById(item.id);
                if (dish == null || !dish.Available)
                {
                    throw new Exception("El plato especificado no existe o no está disponible");
                }

                totalAmount += dish.Price * item.quantity;
            }

            var newOrder = new Order
            {
                DeliveryType = order.delivery.id,
                DeliveryTo = order.delivery.to,
                OverallStatus = 1,
                Price = totalAmount,
                Notes = order.notes
            };
            await _orderCommand.InsertOrder(newOrder);

            foreach (var item in order.items)
            {
                var orderItem = new OrderItem
                {
                    Order = newOrder.OrderId,
                    Dish = item.id,
                    Quantity = item.quantity,
                    Notes = item.notes,
                    Status = 1,
                };
                await _orderItemCommand.InsertOrderItem(orderItem);
            }
            var createdOrder = await _orderQuery.GetOrderById(newOrder.OrderId);
            return new OrderResponse
            {
                createAt = createdOrder.CreateDate,
                orderNumber = createdOrder.OrderId,
                totalAmount = createdOrder.Price,
            };
        }

        public async Task<OrderShowResponse> GetOrderById(long id)
        {
            var order = await _orderQuery.GetOrderById(id);

            if (order == null)
            {
                throw new ExeptionNotFound("Orden no encontrada");
            }

            return new OrderShowResponse
            {
                orderNumber = order.OrderId,
                totalAmount = order.Price,
                deliveryTo = order.DeliveryTo,
                notes = order.Notes,
                status = new StatusResponse
                {
                    Id = order.Status.Id,
                    Name = order.Status.Name
                },
                deliveryType = new DeliveryTypeResponse
                {
                    Id = order.DeliveryTypes.Id,
                    Name = order.DeliveryTypes.Name
                },
                items = order.OrderItems.Select(oi => new OrderItemShowResponse
                {
                    id = oi.OrderItemId,
                    quantity = oi.Quantity,
                    notes = oi.Notes,
                    status = new StatusResponse
                    {
                        Id = oi.Statuses.Id,
                        Name = oi.Statuses.Name
                    },
                    dish = new DishShowResponse
                    {
                        id = oi.Dishes.DishId,
                        name = oi.Dishes.Name,
                        image = oi.Dishes.ImageUrl
                    }
                }).ToList()
            };
        }

        public async Task<List<OrderShowResponse>> GetOrderWithFilter(string? dateFrom, string? dateTo, int? status)
        {
            DateTime? fromDate = null;
            DateTime? toDate = null;

            if (!string.IsNullOrEmpty(dateFrom) && !string.IsNullOrEmpty(dateTo))
            {
                if (!DateTime.TryParse(dateFrom, out DateTime parsedFrom))
                {
                    throw new ExceptionBadRequest("Rango de fechas inválido");
                }
                fromDate = parsedFrom;

                if (!DateTime.TryParse(dateTo, out DateTime parsedTo))
                {
                    throw new ExceptionBadRequest("Rango de fechas inválido");
                }
                toDate = parsedTo;

                if (fromDate.HasValue && toDate.HasValue && fromDate > toDate)
                {
                    throw new ExceptionBadRequest("Rango de fechas inválido");
                }
            }

            var orders = await _orderQuery.GetOrders(fromDate, toDate, status);

            return orders.Select(o => new OrderShowResponse
            {
                orderNumber = o.OrderId,
                totalAmount = o.Price,
                deliveryTo = o.DeliveryTo,
                notes = o.Notes,
                status = new StatusResponse
                {
                    Id = o.Status.Id,
                    Name = o.Status.Name
                },
                deliveryType = new DeliveryTypeResponse
                {
                    Id = o.DeliveryTypes.Id,
                    Name = o.DeliveryTypes.Name
                },
                items = o.OrderItems.Select(oi => new OrderItemShowResponse
                {
                    id = oi.OrderItemId,
                    quantity = oi.Quantity,
                    notes = oi.Notes,
                    status = new StatusResponse
                    {
                        Id = oi.Statuses.Id,
                        Name = oi.Statuses.Name
                    },
                    dish = new DishShowResponse
                    {
                        id = oi.Dishes.DishId,
                        name = oi.Dishes.Name,
                        image = oi.Dishes.ImageUrl
                    }
                }).ToList()
            }).ToList();


        }

        public async Task<OrderUpdateResponse> UpdateItems(long id, OrderModifyModel orderModifyModel)
        {
            var order = await _orderQuery.GetOrderById(id);
            decimal totalAmount = 0;

            if (order == null)
            {
                throw new ExeptionNotFound("Orden no encontrada");
            }
            if (order.OverallStatus >= 2)
            {
                throw new ExceptionBadRequest("No se puede modificar una orden que ya está en preparación");
            }

            var itemsToRemove = new List<ItemModel>();

            foreach (var item in orderModifyModel.items)
            {
                if (item.quantity <= 0)
                {
                    throw new ExceptionBadRequest("La cantidad debe ser mayor a cero");
                }
                foreach (var orderitem in order.OrderItems)
                {
                    if (orderitem.Dish == item.id)
                    {
                        orderitem.Quantity = item.quantity;
                        orderitem.Notes = item.notes;

                        itemsToRemove.Add(item);

                        var dish = await _dishQuery.GetDishById(item.id);
                        totalAmount += dish.Price * item.quantity;

                        await _orderItemCommand.UpdateOrderItem(orderitem);
                    }
                }
            }

            foreach (var itemToRemove in itemsToRemove)
            {
                orderModifyModel.items.Remove(itemToRemove);
            }

            foreach (var newItem in orderModifyModel.items)
            {
                var dish = await _dishQuery.GetDishById(newItem.id);
                if (dish == null || !dish.Available)
                {
                    throw new ExceptionBadRequest("El plato especificado no existe o no está disponible");
                }
                var orderItem = new OrderItem
                {
                    Order = order.OrderId,
                    Dish = newItem.id,
                    Quantity = newItem.quantity,
                    Notes = newItem.notes,
                    Status = 1,
                };
                totalAmount += dish.Price * newItem.quantity;
                await _orderItemCommand.InsertOrderItem(orderItem);
            }

            order.Price = totalAmount;
            order.UpdateDate = DateTime.Now;
            await _orderCommand.UpdateOrder(order);

            return new OrderUpdateResponse
            {
                orderNumber = order.OrderId,
                totalAmount = order.Price,
                updateAt = order.UpdateDate
            };
        }

        public async Task<OrderUpdateResponse> UpdateOrderItemStatus(long id, long orderItemId, StatusModifyModel status)
        {
            var order = await _orderQuery.GetOrderByItemId(id);
            if (order == null)
            {
                throw new ExeptionNotFound("Orden no encontrada");
            }
            if (! await _statusQuery.ExistEstatus(status.status))
            {
                throw new ExceptionBadRequest("El estado especificado no es válido");
            }
            foreach (var item in order.OrderItems)
            {
                if (item.OrderItemId == orderItemId)
                {
                    item.Status = status.status;
                    await _orderItemCommand.UpdateOrderItem(item);
                }
            }
            order.UpdateDate = DateTime.Now;
            await _orderCommand.UpdateOrder(order);
            return new OrderUpdateResponse {
                orderNumber = order.OrderId,
                totalAmount = order.Price,
                updateAt = order.UpdateDate
            };
        }
    }
}
