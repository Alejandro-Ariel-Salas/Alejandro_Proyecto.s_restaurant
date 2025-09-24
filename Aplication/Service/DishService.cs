using Aplication.Enums;
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
using System.Windows.Markup;

namespace Aplication.Service
{
    public class DishService : IDishService
    {
        private readonly IDishCommand _dishCommand;
        private readonly IDishQuery _dishQuery;
        private readonly IOrderItemQuery _orderItemQuery;
        private readonly IStatusQuery _statusQuery;
        private readonly ICategoryQuery _categoryQuery;
        private readonly IDeliveryTypeQuery _deliveryTypeQuery;


        public DishService(IDishCommand dishCommand, IDishQuery dishQuery, IStatusQuery statusQuery, ICategoryQuery categoryQuery, IDeliveryTypeQuery deliveryTypeQuery, IOrderItemQuery orderItemQuery)
        {
            _dishCommand = dishCommand;
            _dishQuery = dishQuery;
            _orderItemQuery = orderItemQuery;
            _statusQuery = statusQuery;
            _categoryQuery = categoryQuery;
            _deliveryTypeQuery = deliveryTypeQuery;
        }

        public async Task<DishResponse> CreateDish(DishRequest dishModel)
        {
            await DishValidation(dishModel);
            {
                var dish = new Dish
                {
                    Name = dishModel.Name,
                    Description = dishModel.Description,
                    Price = dishModel.Price,
                    Category = dishModel.Category,
                    Available = true,
                    ImageUrl = dishModel.Image,
                };
                await _dishCommand.InsertDish(dish);
                var createdDish = await _dishQuery.GetDishById(dish.DishId);

                var response = new DishResponse
                {
                    Id = dish.DishId,
                    Name = dish.Name,
                    Description = dish.Description,
                    Price = dish.Price,
                    Category = new GenericResponse
                    {
                        Id = createdDish.Categorys.Id,
                        Name = createdDish.Categorys.Name
                    },
                    IsActive = dish.Available,
                    Image= dish.ImageUrl,
                    CreatedAt = dish.CreateDate,
                    UpdatedAt = dish.UpdateDate,
                };
                return response;
            }
        }

        public async Task DishValidation(DishRequest dish)
        {
            var existingDish = await _dishQuery.GetDishByName(dish.Name);
            if (existingDish != null)
            {
                throw new ExceptionConflict("Ya Existe un plato con el mismo nombre.");
            }

            if (dish.Price <= 0)
            {
                throw new ExceptionBadRequest("El precio del plato debe ser mayor a cero.");
            }

            var categoryExists = await _dishQuery.ExistCategory(dish.Category);
            if (!categoryExists)
            {
                throw new ExceptionBadRequest("La categoria no existe.");
            }
        }

        public async Task<DishResponse> DeleteDish(Guid id)
        {
            var dish = await _dishQuery.GetDishById(id);
            var orderItems = await _orderItemQuery.GetOrderItemsByDishId(id);

            if (dish == null)
            {
                throw new ExeptionNotFound("Plato no encontrado");
            }

            var responseDish = new DishResponse
            {
                Id = dish.DishId,
                Name = dish.Name,
                Description = dish.Description,
                Price = dish.Price,
                Category = new GenericResponse
                {
                    Id = dish.Categorys.Id,
                    Name = dish.Categorys.Name
                },
                IsActive = false,
                Image = dish.ImageUrl,
                CreatedAt = dish.CreateDate,
                UpdatedAt = dish.UpdateDate,
            };

            if (orderItems != null)
            {
                dish.Available = false;
                await _dishCommand.UpdateDish(dish);
                throw new ExceptionConflict("No se puede eliminar el plato porque está incluido en órdenes activas");
            }
            if (orderItems == null )
            {
                await _dishCommand.DeleteDish(dish);
            }
            return responseDish;
        }

        public Task<List<DishResponse>> GetAllDishes()
        {
            throw new NotImplementedException();
        }

        public async Task<DishResponse> GetDishById(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ExceptionBadRequest("Formato de ID inválido");
            }

            var dish = await _dishQuery.GetDishById(id);

            if (dish == null)
            {
                throw new ExeptionNotFound("Plato no encontrado");
            }

            return new DishResponse
            {
                Id = dish.DishId,
                Name = dish.Name,
                Description = dish.Description,
                Price = dish.Price,
                Category = new GenericResponse
                {
                    Id = dish.Categorys.Id,
                    Name = dish.Categorys.Name
                },
                IsActive = dish.Available,
                Image = dish.ImageUrl,
                CreatedAt = dish.CreateDate,
                UpdatedAt = dish.UpdateDate,
            };
        }

        public async Task<List<DishResponse>> GetDishes(string? name, int? category, EnumSort sort, bool dishAvailable)
        {
            if (category != null)
                {
                var categoryExists = await _dishQuery.ExistCategory(category.Value);
                if (!categoryExists)
                {
                    throw new ExceptionBadRequest("Parámetros de ordenamiento inválidos");
                }
            }

            var query = await _dishQuery.GetDishesByFilter(name, dishAvailable, sort, category);
            return query.Select( d => new DishResponse
            {
                Id = d.DishId,
                Name = d.Name,
                Description = d.Description,
                Price = d.Price,
                Category = new GenericResponse
                {
                    Id = d.Categorys.Id,
                    Name = d.Categorys.Name
                },
                IsActive = d.Available,
                Image = d.ImageUrl,
                CreatedAt = d.CreateDate,
                UpdatedAt = d.UpdateDate,
            }).ToList();
        }

        public async Task<DishResponse> UpdateDish(Guid dishId, DishUpdateRequest dish)
        { 
            await DishValidation( new DishRequest
            {
                Name = dish.Name,
                Description = dish.Description,
                Price = dish.Price,
                Category = dish.Category,
                Image = dish.Image,
            });

            var existingDish = await _dishQuery.GetDishById(dishId);
            if (existingDish == null)
            {
                throw new ExceptionBadRequest("El plato no existe.");
            }

            existingDish.Name = dish.Name;
            existingDish.Description = dish.Description;
            existingDish.Price = dish.Price;
            existingDish.Category = dish.Category;
            existingDish.Available = dish.IsActive;
            existingDish.ImageUrl = dish.Image;
            existingDish.UpdateDate = DateTime.UtcNow;

            await _dishCommand.UpdateDish(existingDish);
            existingDish = await _dishQuery.GetDishById(dishId);

            return new DishResponse
            {
                Id = existingDish.DishId,
                Name = dish.Name,
                Description = dish.Description,
                Price = dish.Price,
                Category = new GenericResponse
                {
                    Id = existingDish.Categorys.Id,
                    Name = existingDish.Categorys.Name
                },
                IsActive = dish.IsActive,
                Image = dish.Image,
                CreatedAt = existingDish.CreateDate,
                UpdatedAt = DateTime.UtcNow,
            };

        }

        public async Task<List<CategoryResponse>> GetAllCategory()
        {
            var categories = await _categoryQuery.GetAllCategories();
            return categories.Select(c => new CategoryResponse
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                Order = c.Order
            }).ToList();
        }

        public async Task<List<DeliveryTypeResponse>> GetAllDeliveryType()
        {
            var deliveryTypes = await _deliveryTypeQuery.GetAllDeliveryTypes();
            return deliveryTypes.Select(dt => new DeliveryTypeResponse
            {
                Id = dt.Id,
                Name = dt.Name
            }).ToList();
        }

        public async Task<List<StatusResponse>> GetAllStatus()
        {
            var statuses = await _statusQuery.GetAllStatus();
            return statuses.Select(s => new StatusResponse
            {
                Id = s.Id,
                Name = s.Name
            }).ToList();
        }
    }
}
