using DataAccess;
using Domain;
using IDataAccess;
using IServices;
using Microsoft.AspNetCore.Mvc;
using Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServerLogWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogsController : Controller
    {
        private readonly ILogService logService;
        public LogsController(ILogService _logService) 
        {
            logService = _logService;
        }
   
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            List<Log> logs  = logService.GetLogs();
            return Ok(logs);
        }
    }
}