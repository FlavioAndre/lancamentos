using Microsoft.EntityFrameworkCore;
using Consolidado.Infrastructure.Data;
using Consolidado.Infrastructure.Repositories;
using Consolidado.Application.Interfaces;
using Consolidado.Application.Repositories;
using Consolidado.Application.Handlers;

using RabbitMQ.Client;
using ControleLancamentos.Infrastructure.Messaging;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

DotNetEnv.Env.Load();

builder.Services.AddDbContext<ConsolidationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

 builder.Services.AddSingleton<IConnectionFactory>(sp =>
{
    var rabbitMqUri = builder.Configuration["RabbitMQ:Uri"] ?? "amqp://guest:guest@rabbitmq:5672";
    var uri = new Uri(rabbitMqUri);
    return new ConnectionFactory() { Uri = uri };
});

builder.Services.AddScoped<IConsolidationRepository, ConsolidationRepository>();
builder.Services.AddScoped<ITransactionEventHandler, TransactionEventHandler>();
builder.Services.AddHostedService<RabbitMqTransactionConsumer>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "API para recuperar o saldo consolidado",
        Description = "Documentação da API para o sistema de controle de lançamentos",
        Contact = new OpenApiContact
        {
            Name = "dev",
        }
    });
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API de Controle de Lançamentos V1");
        c.RoutePrefix = string.Empty; // Swagger disponível em http://localhost:8081/
    });
}

//app.UseAuthorization();
app.MapControllers();

app.Run();
