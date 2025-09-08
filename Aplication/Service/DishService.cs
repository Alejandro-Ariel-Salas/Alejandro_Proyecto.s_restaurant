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

        public DishService(IDishCommand dishCommand, IDishQuery dishQuery)
        {
            _dishCommand = dishCommand;
            _dishQuery = dishQuery;
        }

        public async Task<DishResponse> CreateDish(DishModel dishModel)
        {
            await DishValidation(dishModel);
            {
                var dish = new Dish
                {
                    Name = dishModel.Name,
                    Description = dishModel.Description,
                    Price = dishModel.Price,
                    CategoryId = dishModel.CategoryId,
                    Available = true,
                    ImageUrl = dishModel.ImageUrl,
                };
                await _dishCommand.InsertDish(dish);
                var createdDish = await _dishQuery.GetDishById(dish.DishId);

                var response = new DishResponse
                {
                    DishId = dish.DishId,
                    Name = dish.Name,
                    Description = dish.Description,
                    Price = dish.Price,
                    Category = new CategoryResponse
                    {
                        CategoryId = createdDish.Category.CategoryId,
                        Name = createdDish.Category.Name
                    },
                    Available = dish.Available,
                    ImageUrl = dish.ImageUrl,
                    CreateDate = dish.CreateDate,
                    UpdateDate = dish.UpdateDate,
                };
                return response;
            }
        }

        public async Task DishValidation(DishModel dish)
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

            var categoryExists = await _dishQuery.ExistCategory(dish.CategoryId);
            if (!categoryExists)
            {
                throw new ExceptionBadRequest("La categoria no existe.");
            }
        }

        public Task<Dish> DeleteDish(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Dish>> GetAllDishes()
        {
            throw new NotImplementedException();
        }

        public async Task<Dish> GetDishById(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<DishResponse>> GetDishes(string? name, int? category, EnumSort sort, bool dishAvailable)
        {
            if (!string.IsNullOrEmpty(name))
            {
                var query = await _dishQuery.GetDishesByName(name, dishAvailable, sort);
                return query.Select( d => new DishResponse
                {
                    DishId = d.DishId,
                    Name = d.Name,
                    Description = d.Description,
                    Price = d.Price,
                    Category = new CategoryResponse
                    {
                        CategoryId = d.Category.CategoryId,
                        Name = d.Category.Name
                    },
                    Available = d.Available,
                    ImageUrl = d.ImageUrl,
                    CreateDate = d.CreateDate,
                    UpdateDate = d.UpdateDate,
                }).ToList();
            }

            if (category.HasValue)
            {
                var query = await _dishQuery.GetByCategoryId((int)category, dishAvailable, sort);
                return query.Select(d => new DishResponse
                {
                    DishId = d.DishId,
                    Name = d.Name,
                    Description = d.Description,
                    Price = d.Price,
                    Category = new CategoryResponse 
                    { 
                        CategoryId = d.Category.CategoryId, 
                        Name = d.Category.Name 
                    },
                    Available = d.Available,
                    ImageUrl = d.ImageUrl,
                    CreateDate = d.CreateDate,
                    UpdateDate = d.UpdateDate,
                }).ToList();
            }

            throw new ExceptionBadRequest("Parámetros de ordenamiento inválidos");
        }

        public async Task<DishResponse> UpdateDish(Guid dishId, DishUpdateModel dish)
        { 
            await DishValidation( new DishModel
            {
                Name = dish.Name,
                Description = dish.Description,
                Price = dish.Price,
                CategoryId = dish.Category,
                ImageUrl = dish.Image,
            });

            var existingDish = await _dishQuery.GetDishById(dishId);
            if (existingDish == null)
            {
                throw new ExceptionBadRequest("El plato no existe.");
            }

            existingDish.Name = dish.Name;
            existingDish.Description = dish.Description;
            existingDish.Price = dish.Price;
            existingDish.CategoryId = dish.Category;
            existingDish.Available = dish.IsActive;
            existingDish.ImageUrl = dish.Image;
            existingDish.UpdateDate = DateTime.UtcNow;

            await _dishCommand.UpdateDish(existingDish);
            existingDish = await _dishQuery.GetDishById(dishId);

            return new DishResponse
            {
                DishId = existingDish.DishId,
                Name = dish.Name,
                Description = dish.Description,
                Price = dish.Price,
                Category = new CategoryResponse
                {
                    CategoryId = existingDish.Category.CategoryId,
                    Name = existingDish.Category.Name
                },
                Available = dish.IsActive,
                ImageUrl = dish.Image,
                CreateDate = existingDish.CreateDate,
                UpdateDate = DateTime.UtcNow,
            };

        }
    }
}
