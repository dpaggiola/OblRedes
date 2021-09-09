using System.Collections.Generic;
using Domain;

namespace IServices
{
    public interface ILogService
    {
        void AddLog(Log log);
      
        List<Log> GetLogs();
    
    }
}