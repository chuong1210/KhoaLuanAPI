using Application.IntegrationEvents.Incoming;
using Application.IntegrationEvents.Outgoing;
using Confluent.Kafka;
using Confluent.Kafka.Admin;
using Infrastructure.IntegrationEvents.MessageBroker.Kafka.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.IntegrationEvents.MessageBroker
{
    public class KafkaConsumerHostedService : BackgroundService
    {
        private readonly IConsumer<string, string> _consumer;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<KafkaConsumerHostedService> _logger;
        private readonly string[] _topics;
        private readonly string _bootstrapServers;

        public KafkaConsumerHostedService(
            IConfiguration configuration,
            IServiceProvider serviceProvider,
            ILogger<KafkaConsumerHostedService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;

            _bootstrapServers = configuration["MessageBroker:Kafka:BootstrapServers"];

            var config = new ConsumerConfig
            {
                BootstrapServers = _bootstrapServers,
                GroupId = configuration["MessageBroker:Kafka:GroupId"],
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false,
                SessionTimeoutMs = int.Parse(configuration["MessageBroker:Kafka:SessionTimeoutMs"] ?? "10000")
            };

            _topics = new[]
            {
                "product.updated",
                "product.deleted",
                "order-shop.created",
                "order-shop.completed",
                "order-shop.cancelled",
                "user-profile.created",
                "address.updated",
                "shop.created",
                "shop.updated",
                "voucher-shop.used",
                "cart.updated",
                "cart-item.removed",
                "cart.cleared"
            };

            _consumer = new ConsumerBuilder<string, string>(config).Build();

            // Đảm bảo các topic tồn tại (tự tạo nếu chưa có)
            EnsureTopicsExist(_bootstrapServers, _topics).GetAwaiter().GetResult();

            _consumer.Subscribe(_topics);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("KafkaConsumerHostedService started. Listening to topics: {Topics}", string.Join(", ", _topics));

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var consumeResult = _consumer.Consume(stoppingToken);

                    if (consumeResult?.Message?.Value != null)
                    {
                        await ProcessMessageAsync(consumeResult.Topic, consumeResult.Message.Value);
                        _consumer.Commit(consumeResult);
                    }
                }
                catch (ConsumeException ex)
                {
                    _logger.LogWarning("Kafka consume warning: {Reason}", ex.Error.Reason);
                    await Task.Delay(2000, stoppingToken); // tránh vòng lặp nhanh gây spam log
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Unexpected error while consuming Kafka messages.");
                    await Task.Delay(5000, stoppingToken);
                }
            }

            _logger.LogInformation("KafkaConsumerHostedService is stopping...");
        }

        private async Task ProcessMessageAsync(string topic, string message)
        {
            using var scope = _serviceProvider.CreateScope();
            var sp = scope.ServiceProvider;
            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            try
            {
                switch (topic)
                {
                    case "product.updated":
                        await HandleEventAsync<ProductUpdatedEvent>(sp, message, jsonOptions);
                        break;

                    case "product.deleted":
                        await HandleEventAsync<ProductDeletedEvent>(sp, message, jsonOptions);
                        break;

                    case "order-shop.created":
                        await HandleEventAsync<OrderShopCreatedEvent>(sp, message, jsonOptions);
                        break;

                    case "order-shop.completed":
                        await HandleEventAsync<OrderShopCompletedEvent>(sp, message, jsonOptions);
                        break;

                    case "order-shop.cancelled":
                        await HandleEventAsync<OrderShopCancelledEvent>(sp, message, jsonOptions);
                        break;

                    case "user-profile.created":
                        await HandleEventAsync<UserProfileCreatedEvent>(sp, message, jsonOptions);
                        break;

                    case "address.updated":
                        await HandleEventAsync<AddressUpdatedEvent>(sp, message, jsonOptions);
                        break;

                    case "shop.created":
                        await HandleEventAsync<ShopCreatedEvent>(sp, message, jsonOptions);
                        break;

                    case "shop.updated":
                        await HandleEventAsync<ShopUpdatedEvent>(sp, message, jsonOptions);
                        break;

                    case "voucher-shop.used":
                        await HandleEventAsync<VoucherShopUsedEvent>(sp, message, jsonOptions);
                        break;

                    case "cart.updated":
                        await HandleEventAsync<CartUpdatedEvent>(sp, message, jsonOptions);
                        break;

                    case "cart-item.removed":
                        await HandleEventAsync<CartItemRemovedEvent>(sp, message, jsonOptions);
                        break;

                    case "cart.cleared":
                        await HandleEventAsync<CartClearedEvent>(sp, message, jsonOptions);
                        break;

                    default:
                        _logger.LogInformation("No handler registered for topic {Topic}", topic);
                        break;
                }

                _logger.LogInformation("Processed message from topic {Topic}", topic);
            }
            catch (Exception ex)
            {
                // Không throw tiếp để tránh service crash - log kỹ để debug
                _logger.LogError(ex, "Failed to process message from topic {Topic}", topic);
            }
        }

        private async Task HandleEventAsync<TEvent>(IServiceProvider sp, string message, JsonSerializerOptions jsonOptions)
            where TEvent : class
        {
            try
            {
                TEvent? ev = null;
                try
                {
                    ev = JsonSerializer.Deserialize<TEvent>(message, jsonOptions);
                }
                catch (JsonException jex)
                {
                    _logger.LogError(jex, "JSON deserialization error for event type {EventType} - message: {Message}", typeof(TEvent).Name, message);
                    return;
                }

                if (ev == null)
                {
                    _logger.LogWarning("Deserialized event is null for type {EventType}. Original message: {Message}", typeof(TEvent).Name, message);
                    return;
                }

                var handler = sp.GetService<IEventHandler<TEvent>>();
                if (handler == null)
                {
                    _logger.LogWarning("No IEventHandler<{EventType}> registered in DI container. Topic message will be ignored.", typeof(TEvent).Name);
                    return;
                }

                await handler.HandleAsync(ev);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling event of type {EventType}", typeof(TEvent).Name);
            }
        }

        private async Task EnsureTopicsExist(string bootstrapServers, string[] topics)
        {
            try
            {
                var adminConfig = new AdminClientConfig { BootstrapServers = bootstrapServers };

                using var adminClient = new AdminClientBuilder(adminConfig).Build();

                var metadata = adminClient.GetMetadata(TimeSpan.FromSeconds(5));
                var existingTopics = new HashSet<string>();
                foreach (var t in metadata.Topics)
                    existingTopics.Add(t.Topic);

                var missingTopics = new List<TopicSpecification>();
                foreach (var t in topics)
                {
                    if (!existingTopics.Contains(t))
                        missingTopics.Add(new TopicSpecification { Name = t, NumPartitions = 1, ReplicationFactor = 1 });
                }

                if (missingTopics.Count > 0)
                {
                    _logger.LogInformation("Creating missing Kafka topics: {Topics}", string.Join(", ", missingTopics.Select(t => t.Name)));
                    await adminClient.CreateTopicsAsync(missingTopics);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Could not verify or create Kafka topics. Ensure broker is reachable.");
            }
        }

        public override void Dispose()
        {
            try
            {
                _consumer?.Close();
                _consumer?.Dispose();
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error during Kafka consumer cleanup.");
            }

            base.Dispose();
        }
    }
}
