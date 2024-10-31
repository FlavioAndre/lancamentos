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

// Configurar serviços
ConfigureServices(builder.Services, builder.Configuration);

var app = builder.Build();

// Configurar pipeline HTTP
ConfigurePipeline(app);

app.Run();

void ConfigureServices(IServiceCollection services, IConfiguration configuration)
{
    // Configurar DB Context com variáveis de ambiente
    services.AddDbContext<TransactionDbContext>(options =>
        options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

    // Configurar RabbitMQ com variáveis de ambiente
    services.AddSingleton<IConnectionFactory>(sp =>
    {
        var rabbitMqUri = configuration["RabbitMQ:Uri"] ?? "amqp://guest:guest@rabbitmq:5672";
        var uri = new Uri(rabbitMqUri);
        return new ConnectionFactory() { Uri = uri };
    });
    services.AddScoped<IMessagePublisher, RabbitMqMessagePublisher>();

    // Configurar Serviços e JWT usando variáveis de ambiente
    services.Configure<JwtSettings>(configuration.GetSection("JWT"));
    services.AddScoped<ITransactionService, TransactionService>();
    services.AddScoped<ITransactionRepository, TransactionRepository>();

    // Configuração de autenticação, JWT, Swagger e serviços
    services.AddControllers().AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
    });

    services.AddEndpointsApiExplorer();
    ConfigureSwagger(services);

    services.AddMediatR(typeof(CreateTransactionCommandHandler).Assembly);
}

void ConfigureSwagger(IServiceCollection services)
{
    services.AddSwaggerGen(c =>
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
}

void ConfigurePipeline(WebApplication app)
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "API de Controle de Lançamentos V1");
            c.RoutePrefix = string.Empty; // Swagger disponível em http://localhost:8080/
        });
    }

    app.MapControllers();
}