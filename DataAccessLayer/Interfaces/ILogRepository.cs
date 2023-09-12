using Microsoft.AspNetCore.Mvc;
using RealTimeChatApi.DataAccessLayer.Models;

namespace RealTimeChatApi.DataAccessLayer.Interfaces
{
    public interface ILogRepository
    {
        Task<IQueryable<Log>> GetLogs(DateTime? parsedStartTime, DateTime? parsedEndTime);
    }
}
