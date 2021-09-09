using DataAccess;
using Domain;
using IDataAccess;
using IServices;
using Persistance;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Services;
using System;
using System.Text;
using System.Text.Json;

namespace QueueManager
{
    public class Program
    {
        static void Main(string[] args)
        {
            ILogDataAccess logDataAccess = new LogDataAccess();
            ILogService logService = new LogService(logDataAccess);
            IModel _queue = SingletonInstance.Instance;
            _queue.QueueDeclare("log_queue",
                false,
                false,
                false,
                null);


            var consumer = new EventingBasicConsumer(_queue);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var log = JsonSerializer.Deserialize<Log>(message);
                logService.AddLog(log);
                Console.WriteLine(" [x] Received log level [{0}], message [{1}]", log.EventType, log.Message);
            };

            _queue.BasicConsume("log_queue",
                true,
                consumer);

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}
