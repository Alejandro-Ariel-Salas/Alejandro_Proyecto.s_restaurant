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
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Name).HasColumnType("varchar(25)").IsRequired();
                entity.HasMany(e => e.Orders).WithOne(e => e.Status).HasForeignKey(e => e.OverallStatus).OnDelete(DeleteBehavior.NoAction);
                entity.HasMany(e => e.OrderItems).WithOne(e => e.Statuses).HasForeignKey(e => e.Status).OnDelete(DeleteBehavior.NoAction);

                entity.HasData(
                    new Status { Id = 1, Name = "Pending" },
                    new Status { Id = 2, Name = "In progress" },
                    new Status { Id = 3, Name = "Ready" },
                    new Status { Id = 4, Name = "Delivery" },
                    new Status { Id = 5, Name = "Closed" }
                );
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Category");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Name).HasColumnType("varchar(25)").IsRequired();
                entity.Property(e => e.Description).HasColumnType("varchar(255)");
                entity.HasMany(e => e.Dishes).WithOne(e => e.Categorys).HasForeignKey(e => e.Category);

                entity.HasData(
                    new Category { Id = 1, Name = "Entradas", Description = "Pequeñas porciones para abrir el apetito antes del plato principal.", Order = 1 },
                    new Category { Id = 2, Name = "Ensaladas", Description = "Opciones frescas y livianas, ideales como acompañamiento o plato principal.", Order = 2 },
                    new Category { Id = 3, Name = "Minutas", Description = "Platos rápidos y clásicos de bodegón: milanesas, tortillas, revueltos.", Order = 3 },
                    new Category { Id = 4, Name = "Pastas", Description = "Variedad de pastas caseras y salsas tradicionales.", Order = 4 },
                    new Category { Id = 5, Name = "Parrilla", Description = "Cortes de carne asados a la parrilla, servidos con guarniciones.", Order = 5 },
                    new Category { Id = 6, Name = "Pizzas", Description = "Pizzas artesanales con masa casera y variedad de ingredientes.", Order = 6 },
                    new Category { Id = 7, Name = "Sandwiches", Description = "Sandwiches y lomitos completos preparados al momento.", Order = 7 },
                    new Category { Id = 8, Name = "Bebidas", Description = "Gaseosas, jugos, aguas y opciones sin alcohol.", Order = 8 },
                    new Category { Id = 9, Name = "Cerveza Artesanal", Description = "Cervezas de producción artesanal, rubias, rojas y negras.", Order = 9 },
                    new Category { Id = 10, Name = "Postres", Description = "Clásicos dulces caseros para cerrar la comida.", Order = 10 }

                    );
            });

            modelBuilder.Entity<DeliveryType>(entity =>
            {
                entity.ToTable("DeliveryType");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Name).HasColumnType("nvarchar(25)").IsRequired();
                entity.HasMany(e => e.Orders).WithOne(e => e.DeliveryTypes).HasForeignKey(e => e.DeliveryType);

                entity.HasData(
                    new DeliveryType { Id = 1, Name = "Delivery" },
                    new DeliveryType { Id = 2, Name = "Take away" },
                    new DeliveryType { Id = 3, Name = "Dine in" }
                );
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Order");
                entity.HasKey(e => e.OrderId);
                entity.Property(e => e.OrderId).HasColumnType("bigint").ValueGeneratedOnAdd();
                entity.Property(e => e.DeliveryType).IsRequired();
                entity.Property(e => e.DeliveryTo).HasColumnType("varchar(255)").IsRequired();
                entity.Property(e => e.Notes).HasColumnType("varchar(MAX)");
                entity.Property(e => e.OverallStatus).IsRequired();
                entity.Property(e => e.Price).IsRequired().HasColumnType("decimal(18,2)");
                entity.Property(e => e.CreateDate).HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.UpdateDate).HasDefaultValueSql("GETDATE()");
                entity.HasMany(e => e.OrderItems).WithOne(e => e.Orders).HasForeignKey(e => e.Order);
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
                entity.HasMany(e => e.OrderItems).WithOne(e => e.Dishes).HasForeignKey(e => e.Dish);
            });

            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.ToTable("OrderItem");
                entity.HasKey(e => e.OrderItemId);
                entity.Property(e => e.OrderItemId).HasColumnType("bigint").ValueGeneratedOnAdd();
                entity.Property(e => e.Order).HasColumnType("bigint").IsRequired();
                entity.Property(e => e.Dish).HasColumnType("uniqueidentifier").IsRequired();
                entity.Property(e => e.Notes).HasColumnType("varchar(MAX)");
                entity.Property(e => e.OrderItemId).ValueGeneratedOnAdd();
                entity.Property(e => e.Quantity).IsRequired();
                entity.Property(e => e.CreateDate).HasDefaultValueSql("GETDATE()");
            });
        }
    }
}
