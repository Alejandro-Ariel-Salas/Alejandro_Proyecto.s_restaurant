using domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraesructure.Perssistence
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options)
        : base(options)
        {
        }

        public DbSet<Status> statuses { get; set; }
        public DbSet<Category> categories { get; set; }
        public DbSet<DeliveryType> deliveryTypes { get; set; }
        public DbSet<Order> orders { get; set; }
        public DbSet<Dish> dishes { get; set; }
        public DbSet<OrderItem> orderItems { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Status>(entity => 
            {
                entity.ToTable("Status");
                entity.HasKey(e => e.StatusId);
                entity.Property(e => e.StatusId).ValueGeneratedOnAdd();
                entity.Property(e => e.Name).HasColumnType("varchar(25)").IsRequired();
                entity.HasMany(e => e.Orders).WithOne(e => e.Status).HasForeignKey(e => e.OverallStatus).OnDelete(DeleteBehavior.NoAction);
                entity.HasMany(e => e.OrderItems).WithOne(e => e.Status).HasForeignKey(e => e.StatusId).OnDelete(DeleteBehavior.NoAction);

                entity.HasData(
                    new Status { StatusId = 1, Name = "Pending" },
                    new Status { StatusId = 2, Name = "In progress" },
                    new Status { StatusId = 3, Name = "Ready" },
                    new Status { StatusId = 4, Name = "Delivery" },
                    new Status { StatusId = 5, Name = "Closed" }
                );
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Category");
                entity.HasKey(e => e.CategoryId);
                entity.Property(e => e.CategoryId).ValueGeneratedOnAdd();
                entity.Property(e => e.Name).HasColumnType("varchar(25)").IsRequired();
                entity.Property(e => e.Description).HasColumnType("varchar(255)");
                entity.HasMany(e => e.Dishes).WithOne(e => e.Category).HasForeignKey(e => e.CategoryId);

                entity.HasData(
                    new Category { CategoryId = 1, Name = "Entradas", Description = "Pequeñas porciones para abrir el apetito antes del plato principal.", Order = 1 },
                    new Category { CategoryId = 2, Name = "Ensaladas", Description = "Opciones frescas y livianas, ideales como acompañamiento o plato principal.", Order = 2 },
                    new Category { CategoryId = 3, Name = "Minutas", Description = "Platos rápidos y clásicos de bodegón: milanesas, tortillas, revueltos.", Order = 3 },
                    new Category { CategoryId = 4, Name = "Pastas", Description = "Variedad de pastas caseras y salsas tradicionales.", Order = 4 },
                    new Category { CategoryId = 5, Name = "Parrilla", Description = "Cortes de carne asados a la parrilla, servidos con guarniciones.", Order = 5 },
                    new Category { CategoryId = 6, Name = "Pizzas", Description = "Pizzas artesanales con masa casera y variedad de ingredientes.", Order = 6 },
                    new Category { CategoryId = 7, Name = "Sandwiches", Description = "Sandwiches y lomitos completos preparados al momento.", Order = 7 },
                    new Category { CategoryId = 8, Name = "Bebidas", Description = "Gaseosas, jugos, aguas y opciones sin alcohol.", Order = 8 },
                    new Category { CategoryId = 9, Name = "Cerveza Artesanal", Description = "Cervezas de producción artesanal, rubias, rojas y negras.", Order = 9 },
                    new Category { CategoryId = 10, Name = "Postres", Description = "Clásicos dulces caseros para cerrar la comida.", Order = 10 }

                    );
            });

            modelBuilder.Entity<DeliveryType>(entity =>
            {
                entity.ToTable("DeliveryType");
                entity.HasKey(e => e.DeliveryTypeId);
                entity.Property(e => e.DeliveryTypeId).ValueGeneratedOnAdd();
                entity.Property(e => e.Name).HasColumnType("nvarchar(25)").IsRequired();
                entity.HasMany(e => e.Orders).WithOne(e => e.DeliveryType).HasForeignKey(e => e.DeliveryTypeId);

                entity.HasData(
                    new DeliveryType { DeliveryTypeId = 1, Name = "Delivery" },
                    new DeliveryType { DeliveryTypeId = 2, Name = "Take away" },
                    new DeliveryType { DeliveryTypeId = 3, Name = "Dine in" }
                );
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Order");
                entity.HasKey(e => e.OrderId);
                entity.Property(e => e.DeliveryTo).HasColumnType("varchar(255)").IsRequired();
                entity.Property(e => e.Notes).HasColumnType("varchar(MAX)");
                entity.Property(e => e.OverallStatus).IsRequired();
                entity.Property(e => e.Price).IsRequired().HasColumnType("decimal(18,2)");
                entity.Property(e => e.CreateDate).HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.UpdateDate).HasDefaultValueSql("GETDATE()");
                entity.HasMany(e => e.OrderItems).WithOne(e => e.Order).HasForeignKey(e => e.OrderId);
            });

            modelBuilder.Entity<Dish>(entity =>
            {
                entity.ToTable("Dish");
                entity.HasKey(e => e.DishId);
                entity.Property(e => e.DishId).HasDefaultValueSql("NEWID()");
                entity.Property(e => e.Name).HasColumnType("varchar(255)").IsRequired();
                entity.Property(e => e.Description).HasColumnType("varchar(MAX)");
                entity.Property(e => e.Price).IsRequired().HasColumnType("decimal(18,2)");
                entity.Property(e => e.ImageUrl).HasColumnType("varchar(MAX)");
                entity.Property(e => e.Available).IsRequired();
                entity.Property(e => e.CreateDate).HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.UpdateDate).HasDefaultValueSql("GETDATE()");
                entity.HasMany(e => e.OrderItems).WithOne(e => e.Dish).HasForeignKey(e => e.DishId);
            });

            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.ToTable("OrderItem");
                entity.HasKey(e => e.OrderItemId);
                entity.Property(e => e.Notes).HasColumnType("varchar(MAX)");
                entity.Property(e => e.OrderItemId).ValueGeneratedOnAdd();
                entity.Property(e => e.Quantity).IsRequired();
                entity.Property(e => e.CreateDate).HasDefaultValueSql("GETDATE()");
            });
        }
    }
}
