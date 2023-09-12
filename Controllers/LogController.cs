using Microsoft.AspNetCore.Mvc;
using RealTimeChatApi.BusinessLogicLayer.Interfaces;
using RealTimeChatApi.DataAccessLayer.Data;

namespace RealTimeChatApi.Controllers
{
    [Route("api/log")]
    [ApiController]
    public class LogController : ControllerBase
    {
        private readonly RealTimeChatDbContext _context;
        private readonly ILogService _logService;
        public LogController(RealTimeChatDbContext context,
            ILogService logService)
        {
            _logService = logService;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetLogs([FromQuery] string timeframe = null, [FromQuery] string startTime = null, [FromQuery] string endTime = null)
        {
            return await _logService.GetLogs(timeframe, startTime, endTime);
        }
    }
}
