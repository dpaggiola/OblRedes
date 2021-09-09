using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace QueueManager
{
    public class SingletonInstance
    {
        public SingletonInstance()
        {
        }

        public static IModel Instance { get; } =
            new ConnectionFactory { HostName = "localhost" }.CreateConnection().CreateModel();
    }
}