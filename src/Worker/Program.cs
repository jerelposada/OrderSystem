using MassTransit;
using Microsoft.EntityFrameworkCore;
using OrderSystem.Application.Interfaces;
using OrderSystem.Application.Services;
using OrderSystem.Infrastructure.Persistence;
using OrderSystem.Worker.Consumers;

var builder = Host.CreateApplicationBuilder(args);

// Base de datos
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

// Inyección de dependencias
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();

// RabbitMQ con MassTransit
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<CreateOrderConsumer>();

    x.UsingRabbitMq((ctx, cfg) =>
    {
        var host = builder.Configuration["RabbitMq:Host"];
        var user = builder.Configuration["RabbitMq:User"];
        var pass = builder.Configuration["RabbitMq:Pass"];

        cfg.Host(host, h =>
        {
            h.Username(user!);
            h.Password(pass!);
        });

        cfg.ReceiveEndpoint("orders-queue", e =>
        {
            e.UseMessageRetry(r => r.Exponential(
                5,
                TimeSpan.FromSeconds(1),
                TimeSpan.FromSeconds(30),
                TimeSpan.FromSeconds(5)));

            e.ConfigureConsumer<CreateOrderConsumer>(ctx);
        });
    });
});

var host = builder.Build();
await host.RunAsync();