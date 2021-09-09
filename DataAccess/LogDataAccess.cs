using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using Domain;
using Exception;
using ICommon;
using IDataAccess;
using Persistance;
using RabbitMQ.Client;

namespace DataAccess
{
    public class LogDataAccess : ILogDataAccess
    {
        private readonly List<Log> logs = MemoryDataBase.GetInstance().Logs;

        public void AddLog(Log log)
        {
            logs.Add(log);
        }
        public List<Log> GetLogs()
        {
            return logs;
        }
    }
}