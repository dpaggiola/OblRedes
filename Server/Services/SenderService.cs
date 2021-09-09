using Common;
using Domain;
using ICommon;
using QueueManager;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Server.Services
{
    public static class SenderService
    {
        static IModel channel = SingletonInstance.Instance;
        public static async void CreateLog(string commandType, byte[] frame)
        {

            IParser parser = new Parser();
            var data = parser.GetDataObject(frame);
            string message = "";
            for (int i = 0; i < data.Length; i++)
            {
                message += data[i] + "@";
            }
            var log = new Log();
            log.EventType = commandType;
            log.Message = message;
            var stringLog = JsonSerializer.Serialize(log);
            var result = await SendMessage(channel, stringLog);
        }

        public static Task<bool> SendMessage(IModel channel, string message)
        {
            bool returnVal;
            try
            {
                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(exchange: "",
                    routingKey: "log_queue",
                    basicProperties: null,
                    body: body);
                returnVal = true;
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e);
                returnVal = false;
            }

            return Task.FromResult(returnVal);
        }
    }
}