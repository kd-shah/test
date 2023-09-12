using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RealTimeChatApi.BusinessLogicLayer.DTOs;
using RealTimeChatApi.DataAccessLayer.Models;

namespace RealTimeChatApi.BusinessLogicLayer.Interfaces
{
    public interface ILogService
    {
        Task<IActionResult> GetLogs([FromQuery] string timeframe, [FromQuery] string startTime = null, [FromQuery] string endTime = null);
        
    }
}
