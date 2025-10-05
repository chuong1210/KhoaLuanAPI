using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Application.IntegrationEvents.Incoming;
using Application.IntegrationEvents.MessageBroker.Kafka.Interfaces;
namespace Application.IntegrationEvents.MessageBroker
{
    public class KafkaConsumerHostedService : BackgroundService
    {
        private readonly IConsumer<string, string> _consumer;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<KafkaConsumerHostedService> _logger;

        public KafkaConsumerHostedService(
            IConfiguration configuration,
            IServiceProvider serviceProvider,
            ILogger<KafkaConsumerHostedService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;

            var config = new ConsumerConfig
            {
                BootstrapServers = configuration["MessageBroker:Kafka:BootstrapServers"],
                GroupId = configuration["MessageBroker:Kafka:GroupId"],
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false
            };

            _consumer = new ConsumerBuilder<string, string>(config).Build();

            // Subscribe to topics
            var topics = new[]
            {
                "product.updated",
                "product.deleted",
                "order-shop.created",
                "order-shop.completed",
                "user-profile.created",
                "address.updated"
            };

            _consumer.Subscribe(topics);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var consumeResult = _consumer.Consume(stoppingToken);

                    await ProcessMessageAsync(consumeResult.Topic, consumeResult.Message.Value);

                    _consumer.Commit(consumeResult);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error consuming message");
                }
            }
        }

        private async Task ProcessMessageAsync(string topic, string message)
        {
            using var scope = _serviceProvider.CreateScope();

            try
            {
                switch (topic)
                {
                    case "product.updated":
                        var productEvent = JsonSerializer.Deserialize<ProductUpdatedEvent>(message);
                        var productHandler = scope.ServiceProvider
                            .GetRequiredService<IEventHandler<ProductUpdatedEvent>>();
                        await productHandler.HandleAsync(productEvent);
                        break;

                    case "order-shop.completed":
                        var orderEvent = JsonSerializer.Deserialize<OrderShopCompletedEvent>(message);
                        var orderHandler = scope.ServiceProvider
                            .GetRequiredService<IEventHandler<OrderShopCompletedEvent>>();
                        await orderHandler.HandleAsync(orderEvent);
                        break;

                        // Add more cases...
                }

                _logger.LogInformation("Processed message from topic {Topic}", topic);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to process message from topic {Topic}", topic);
                throw;
            }
        }

        public override void Dispose()
        {
            _consumer?.Close();
            _consumer?.Dispose();
            base.Dispose();
        }
    }
}
