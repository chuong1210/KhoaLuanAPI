using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Confluent.Kafka;
using Application.IntegrationEvents.MessageBroker.Kafka.Interfaces;
namespace Application.IntegrationEventss.MessageBroker.Kafka
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
                BootstrapServers = configuration["MessageBroker:Kafka:BootstrapServers"]
            };

            _producer = new ProducerBuilder<string, string>(config).Build();
        }

        public async Task PublishAsync<T>(T @event, string topic = null) where T : class
        {
            topic ??= GetTopicName<T>();
            var message = JsonSerializer.Serialize(@event);
            var key = Guid.NewGuid().ToString();

            try
            {
                var result = await _producer.ProduceAsync(topic, new Message<string, string>
                {
                    Key = key,
                    Value = message
                });

                _logger.LogInformation("Published event {EventType} to topic {Topic}",
                    typeof(T).Name, topic);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to publish event {EventType}", typeof(T).Name);
                throw;
            }
        }

        private string GetTopicName<T>()
        {
            var eventName = typeof(T).Name;
            return _configuration[$"MessageBroker:Kafka:Topics:{eventName}"]
                   ?? eventName.ToLower();
        }
    }

}
