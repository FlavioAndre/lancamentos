using System.Text;
using System.Text.Json;
using Consolidado.Application.Interfaces;
using Consolidado.Application.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Consolidado.Infrastructure.Messaging
{
    public class RabbitMqTransactionConsumer : BackgroundService
    {
        private readonly ILogger<RabbitMqTransactionConsumer> _logger;

        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IConnectionFactory _connectionFactory;

        public RabbitMqTransactionConsumer(IConnectionFactory connectionFactory,
            ILogger<RabbitMqTransactionConsumer> logger,
            IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
            _connectionFactory = connectionFactory;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                var connection = _connectionFactory.CreateConnection();
                var channel = connection.CreateModel();
                channel.QueueDeclare(queue: "transactionQueue", durable: true, exclusive: false, autoDelete: false, arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += async (sender, eventArgs) =>
                {
                    using var scope = _serviceScopeFactory.CreateScope();
                    var handler = scope.ServiceProvider.GetRequiredService<ITransactionEventHandler>();

                    var message = Encoding.UTF8.GetString(eventArgs.Body.ToArray());
                    _logger.LogInformation("Mensagem recebida na fila: {Message}", message);

                    try
                    {
                        var transactionEvent = JsonSerializer.Deserialize<TransactionEvent>(message);
                        if (transactionEvent != null)
                        {
                            await handler.HandleTransactionEvent(transactionEvent);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Erro ao processar mensagem da fila");
                    }

                    channel.BasicAck(eventArgs.DeliveryTag, multiple: false);
                };

                channel.BasicConsume(queue: "transactionQueue", autoAck: false, consumer: consumer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao conectar ao RabbitMQ");

            }
            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            base.Dispose();
        }


    }
}
