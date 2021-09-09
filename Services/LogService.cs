using System;
using System.Collections.Generic;
using System.Text;
using Domain;
using IDataAccess;
using IServices;
namespace Services
{
    public class LogService : ILogService
    {
        public ILogDataAccess logsDataAccess;

        public LogService(ILogDataAccess iLogsDataAccess)
        {
            logsDataAccess = iLogsDataAccess;
        }

        public void AddLog(Log log)
        {
            logsDataAccess.AddLog(log);
        }

        public List<Log> GetLogs()
        {
            return logsDataAccess.GetLogs();
        }
    }
}