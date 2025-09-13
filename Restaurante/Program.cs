using Aplication.Interfaces;
using Aplication.Service;
using Infraesructure.Command;
using Infraesructure.Perssistence;
using Infraesructure.Querys;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Dependences Injection
var ConetionString = builder.Configuration["ConectionString"];
builder.Services.AddDbContext<AppDBContext>(option => option.UseSqlServer(ConetionString));

//Services
builder.Services.AddScoped<IDishService, DishService>();

//Commands
builder.Services.AddScoped<IDishCommand, DishCommand>();

//Querys
builder.Services.AddScoped<IDishQuery, DishQuery>();
builder.Services.AddScoped<IOrderItemQuery, OrderItemQuery>();
builder.Services.AddScoped<ICategoryQuery, CategoryQuery>();
builder.Services.AddScoped<IDeliveryTypeQuery, DeliveryTypeQuery>();
builder.Services.AddScoped<IStatusQuery, StatusQuery>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
