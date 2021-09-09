using DataAccess;
using Domain;
using IDataAccess;
using IServices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using QueueManager;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Services;
using System.Text;
using System.Text.Json;

namespace ServerLogWebApi
{
    public class Program
    {
        public static void Main(string[] args)
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
            };

            _queue.BasicConsume("log_queue",
                true,
                consumer);

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
        }
    }
}