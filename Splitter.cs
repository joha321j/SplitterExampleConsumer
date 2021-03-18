using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace SplitterExampleConsumer
{
    public class Splitter
    {
        private IModel _channel;
        private const string ExchangeName = "splitter_example";
        private const string RoutingKey = "receiverChannel";
        private string _queueName;
        
        public void SetupQueues(IModel channel)
        {
            _channel = channel;
            _channel.ExchangeDeclare(exchange: ExchangeName, type: ExchangeType.Direct); 
            _queueName = _channel.QueueDeclare(durable: true).QueueName;
            
            channel.QueueBind(
                queue: _queueName,
                exchange: ExchangeName,
                routingKey: RoutingKey);
        }

        public void Consume()
        {
            Console.WriteLine("Waiting for messages. To exit press enter!");
            
            EventingBasicConsumer consumer = new EventingBasicConsumer(_channel);
            
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var routingKey = ea.RoutingKey;
                Console.WriteLine(
                    "Consumer received '{0}':'{1}'",
                    routingKey,
                    message);
            };
            
            _channel.BasicConsume(
                queue: _queueName,
                autoAck: true,
                consumer: consumer);
            
        }
    }
}