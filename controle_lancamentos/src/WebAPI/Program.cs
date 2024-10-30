using System.Text.Json;
using System.Text.Json.Serialization;
using ControleLancamentos.Application.Handlers;
using ControleLancamentos.Application.Interfaces;
using ControleLancamentos.Application.Services;
using ControleLancamentos.Infrastructure.Data;
using ControleLancamentos.Infrastructure.Messaging;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using RabbitMQ.Client;
using WebAPI.Settings;

var builder = WebApplication.CreateBuilder(args);

// Carregar variáveis de ambiente do .env
DotNetEnv.Env.Load();

// Configurar DB Context com variáveis de ambiente
builder.Services.AddDbContext<TransactionDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configurar RabbitMQ com variáveis de ambiente
builder.Services.AddSingleton<IConnectionFactory>(sp =>
{
    var rabbitMqUri = builder.Configuration["RabbitMQ:Uri"] ?? "amqp://guest:guest@rabbitmq:5672";
    var uri = new Uri(rabbitMqUri);
    return new ConnectionFactory() { Uri = uri };
});
builder.Services.AddScoped<IMessagePublisher, RabbitMqMessagePublisher>();

// Configurar Serviços e JWT usando variáveis de ambiente
var jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET");
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JWT"));
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();

// Configuração de autenticação, JWT, Swagger e serviços
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
});


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "API de Controle de Lançamentos",
        Description = "Documentação da API para o sistema de controle de lançamentos",
        Contact = new OpenApiContact
        {
            Name = "dev",
        }
    });
});

builder.Services.AddMediatR(typeof(CreateTransactionCommandHandler).Assembly);

var app = builder.Build();
// Configuração do pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API de Controle de Lançamentos V1");
        c.RoutePrefix = string.Empty; // Swagger disponível em http://localhost:8080/
    });
}

//app.UseHttpsRedirection();
//app.UseAuthentication();
//app.UseAuthorization();
app.MapControllers();
app.Run();
