using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace SplitterExampleConsumer
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            ConnectionFactory factory = new ConnectionFactory() {HostName = "localhost"};

            using (IConnection connection = factory.CreateConnection())
            using (IModel channel = connection.CreateModel())
            {
                Splitter splitter = new Splitter();
                splitter.SetupQueues(channel);

                splitter.Consume();

                Console.ReadLine();
            }
        }
    }
}