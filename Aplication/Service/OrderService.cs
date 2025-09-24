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

        public async Task<OrderCreateReponse> CreateOrder(OrderRequest order)
        {
            decimal totalAmount = 0;
            foreach (var item in order.Items)
            {
                var dish = await _dishQuery.GetDishById(item.Id);
                if (dish == null || !dish.Available)
                {
                    throw new Exception("El plato especificado no existe o no está disponible");
                }

                totalAmount += dish.Price * item.Quantity;
            }

            var newOrder = new Order
            {
                DeliveryType = order.Delivery.Id,
                DeliveryTo = order.Delivery.To,
                OverallStatus = 1,
                Price = totalAmount,
                Notes = order.Notes
            };
            await _orderCommand.InsertOrder(newOrder);

            foreach (var item in order.Items)
            {
                var orderItem = new OrderItem
                {
                    Order = newOrder.OrderId,
                    Dish = item.Id,
                    Quantity = item.Quantity,
                    Notes = item.Notes,
                    Status = 1,
                };
                await _orderItemCommand.InsertOrderItem(orderItem);
            }
            var createdOrder = await _orderQuery.GetOrderById(newOrder.OrderId);
            return new OrderCreateReponse
            {
                CreatedAt = createdOrder.CreateDate,
                OrderNumber = createdOrder.OrderId,
                TotalAmount = createdOrder.Price,
            };
        }

        public async Task<OrderDetailResponse> GetOrderById(long id)
        {
            var order = await _orderQuery.GetOrderById(id);

            if (order == null)
            {
                throw new ExeptionNotFound("Orden no encontrada");
            }

            return new OrderDetailResponse
            {
                OrderNumber = order.OrderId,
                TotalAmount = order.Price,
                DeliveryTo = order.DeliveryTo,
                Notes = order.Notes,
                Status = new StatusResponse
                {
                    Id = order.Status.Id,
                    Name = order.Status.Name
                },
                DeliveryType = new DeliveryTypeResponse
                {
                    Id = order.DeliveryTypes.Id,
                    Name = order.DeliveryTypes.Name
                },
                Items = order.OrderItems.Select(oi => new OrderItemShowResponse
                {
                    id = oi.OrderItemId,
                    quantity = oi.Quantity,
                    notes = oi.Notes,
                    status = new StatusResponse
                    {
                        Id = oi.Statuses.Id,
                        Name = oi.Statuses.Name
                    },
                    dish = new DishShortResponse
                    {
                        Id = oi.Dishes.DishId,
                        Name = oi.Dishes.Name,
                        Image = oi.Dishes.ImageUrl
                    }
                }).ToList(),
                CreatedAt = order.CreateDate,
                UpdatedAt = order.UpdateDate
            };
        }

        public async Task<List<OrderDetailResponse>> GetOrderWithFilter(string? dateFrom, string? dateTo, int? status)
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

            return orders.Select(o => new OrderDetailResponse
            {
                OrderNumber = o.OrderId,
                TotalAmount = o.Price,
                DeliveryTo = o.DeliveryTo,
                Notes = o.Notes,
                Status = new StatusResponse
                {
                    Id = o.Status.Id,
                    Name = o.Status.Name
                },
                DeliveryType = new DeliveryTypeResponse
                {
                    Id = o.DeliveryTypes.Id,
                    Name = o.DeliveryTypes.Name
                },
                Items = o.OrderItems.Select(oi => new OrderItemShowResponse
                {
                    id = oi.OrderItemId,
                    quantity = oi.Quantity,
                    notes = oi.Notes,
                    status = new StatusResponse
                    {
                        Id = oi.Statuses.Id,
                        Name = oi.Statuses.Name
                    },
                    dish = new DishShortResponse
                    {
                        Id = oi.Dishes.DishId,
                        Name = oi.Dishes.Name,
                        Image = oi.Dishes.ImageUrl
                    }
                }).ToList(),
                CreatedAt = o.CreateDate,
                UpdatedAt = o.UpdateDate
            }).ToList();


        }

        public async Task<OrderUpdateReponse> UpdateItems(long id, OrderUpdateRequest orderModifyModel)
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

            var itemsToRemove = new List<Items>();

            foreach (var item in orderModifyModel.Items)
            {
                if (item.Quantity <= 0)
                {
                    throw new ExceptionBadRequest("La cantidad debe ser mayor a cero");
                }
                foreach (var orderitem in order.OrderItems)
                {
                    if (orderitem.Dish == item.Id)
                    {
                        orderitem.Quantity = item.Quantity;
                        orderitem.Notes = item.Notes;

                        itemsToRemove.Add(item);

                        var dish = await _dishQuery.GetDishById(item.Id);
                        totalAmount += dish.Price * item.Quantity;

                        await _orderItemCommand.UpdateOrderItem(orderitem);
                    }
                }
            }

            foreach (var itemToRemove in itemsToRemove)
            {
                orderModifyModel.Items.Remove(itemToRemove);
            }

            foreach (var newItem in orderModifyModel.Items)
            {
                var dish = await _dishQuery.GetDishById(newItem.Id);
                if (dish == null || !dish.Available)
                {
                    throw new ExceptionBadRequest("El plato especificado no existe o no está disponible");
                }
                var orderItem = new OrderItem
                {
                    Order = order.OrderId,
                    Dish = newItem.Id,
                    Quantity = newItem.Quantity,
                    Notes = newItem.Notes,
                    Status = 1,
                };
                totalAmount += dish.Price * newItem.Quantity;
                await _orderItemCommand.InsertOrderItem(orderItem);
            }

            order.Price = totalAmount;
            order.UpdateDate = DateTime.Now;
            await _orderCommand.UpdateOrder(order);

            return new OrderUpdateReponse
            {
                OrderNumber = order.OrderId,
                TotalAmount = order.Price,
                UpdatedAt = order.UpdateDate
            };
        }

        public async Task<OrderUpdateReponse> UpdateOrderItemStatus(long id, long orderItemId, OrderItemUpdateRequest status)
        {
            var order = await _orderQuery.GetOrderByItemId(id);
            if (order == null)
            {
                throw new ExeptionNotFound("Orden no encontrada");
            }
            if (! await _statusQuery.ExistEstatus(status.Status))
            {
                throw new ExceptionBadRequest("El estado especificado no es válido");
            }
            foreach (var item in order.OrderItems)
            {
                if (item.OrderItemId == orderItemId)
                {
                    item.Status = status.Status;
                    await _orderItemCommand.UpdateOrderItem(item);
                }
            }
            order.UpdateDate = DateTime.Now;
            await _orderCommand.UpdateOrder(order);
            return new OrderUpdateReponse {
                OrderNumber = order.OrderId,
                TotalAmount = order.Price,
                UpdatedAt = order.UpdateDate
            };
        }
    }
}
