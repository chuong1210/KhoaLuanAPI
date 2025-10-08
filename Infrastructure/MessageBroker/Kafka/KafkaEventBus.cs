using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Confluent.Kafka;
using Infrastructure.IntegrationEvents.MessageBroker.Kafka.Interfaces;
using Application.Interfaces.MessageBroker;

namespace Infrastructure.IntegrationEvents.MessageBroker.Kafka
{
    public class KafkaEventBus : IEventBus
    {
        private readonly IProducer<string, string> _producer;
        private readonly IConfiguration _configuration;
        private readonly ILogger<KafkaEventBus> _logger;

        public KafkaEventBus(IConfiguration configuration, ILogger<KafkaEventBus> logger)
        {
            _configuration = configuration;
            _logger = logger;

            var config = new ProducerConfig
            {
                BootstrapServers = configuration["MessageBroker:Kafka:BootstrapServers"],
                Acks = Acks.All,
                MessageTimeoutMs = 5000,
                RequestTimeoutMs = 5000
            };

            _producer = new ProducerBuilder<string, string>(config).Build();
        }

        public async Task PublishAsync<TEvent>(TEvent @event, string topic = null) where TEvent : class
        {
            topic ??= GetTopicName<TEvent>();
            var message = JsonSerializer.Serialize(@event);
            var key = Guid.NewGuid().ToString();

            try
            {
                var result = await _producer.ProduceAsync(topic, new Message<string, string>
                {
                    Key = key,
                    Value = message,
                    Timestamp = Timestamp.Default
                });

                _logger.LogInformation("Published event {EventType} to topic {Topic}, partition {Partition}, offset {Offset}",
                    typeof(TEvent).Name, topic, result.Partition.Value, result.Offset.Value);
            }
            catch (ProduceException<string, string> ex)
            {
                _logger.LogError(ex, "Failed to publish event {EventType} to topic {Topic}. Error: {ErrorReason}",
                    typeof(TEvent).Name, topic, ex.Error.Reason);
                throw;
            }
        }

        private string GetTopicName<TEvent>()
        {
            var eventName = typeof(TEvent).Name;
            return _configuration[$"MessageBroker:Kafka:Topics:{eventName}"] ?? eventName.ToLower();
        }

        public void Dispose()
        {
            _producer?.Flush(TimeSpan.FromSeconds(10));
            _producer?.Dispose();
        }
    }

}
