using System.Collections.Generic;
using Domain;

namespace IDataAccess
{
    public interface ILogDataAccess
    {
        void AddLog(Log log);
        List<Log> GetLogs();
       
    }
}