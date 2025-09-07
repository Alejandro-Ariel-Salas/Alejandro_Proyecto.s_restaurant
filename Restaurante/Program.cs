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
builder.Services.AddScoped<IDishService, DishService>();

builder.Services.AddScoped<IDishCommand, DishCommand>();
builder.Services.AddScoped<IDishQuery, DishQuery>();

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
