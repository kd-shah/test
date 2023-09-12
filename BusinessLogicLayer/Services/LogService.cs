using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealTimeChatApi.BusinessLogicLayer.Interfaces;
using RealTimeChatApi.DataAccessLayer.Data;
using RealTimeChatApi.DataAccessLayer.Interfaces;

namespace RealTimeChatApi.BusinessLogicLayer.Services
{
    public class LogService : ILogService
    {
        private readonly ILogRepository _logRepository;
        public LogService(ILogRepository logRepository)
        {
            _logRepository = logRepository;
        }

        public async Task<IActionResult> GetLogs([FromQuery] string timeframe, [FromQuery] string startTime, [FromQuery] string endTime)
        {
            DateTime? parsedStartTime = ParseDateTime(startTime);
            DateTime? parsedEndTime = ParseDateTime(endTime);
            if (parsedStartTime == null)
                parsedStartTime = DateTime.Now.AddMinutes(-5);
            if (parsedEndTime == null)
                parsedEndTime = DateTime.Now;


            if (string.IsNullOrEmpty(timeframe))
            {
                timeframe = "5";
            }

            switch (timeframe)
            {
                case "5":
                    parsedStartTime = DateTime.Now.AddMinutes(-5);
                    break;
                case "10":
                    parsedStartTime = DateTime.Now.AddMinutes(-10);
                    break;
                case "30":
                    parsedStartTime = DateTime.Now.AddMinutes(-30);
                    break;
                case "custom":
                    parsedStartTime = parsedStartTime ?? DateTime.Now.AddMinutes(-30);
                    parsedEndTime = parsedEndTime ?? DateTime.Now;
                    break;
                default:
                    parsedStartTime = DateTime.Now.AddMinutes(-5);
                    break;
            }

            var logs = await _logRepository.GetLogs(parsedStartTime, parsedEndTime);

            return new OkObjectResult(logs);
        }
        private DateTime? ParseDateTime(string dateTimeString)
        {
            if (string.IsNullOrEmpty(dateTimeString))
                return null;
            if (DateTime.TryParse(dateTimeString, out DateTime parsedDateTime))
                return parsedDateTime;
            return null;
        }
    }
}
