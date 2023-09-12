using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealTimeChatApi.DataAccessLayer.Data;
using RealTimeChatApi.DataAccessLayer.Interfaces;
using RealTimeChatApi.DataAccessLayer.Models;

namespace RealTimeChatApi.DataAccessLayer.Repositories
{
    public class LogRepository : ILogRepository
    {
        private readonly RealTimeChatDbContext _context;

        public LogRepository(RealTimeChatDbContext context)
        {

            _context = context;
        }
        public async Task<IQueryable<Log>> GetLogs(DateTime? parsedStartTime, DateTime? parsedEndTime)
        {
            var allLogs = _context.Logs.Where(log => log.timeStamp >= parsedStartTime && log.timeStamp <= parsedEndTime);
            return allLogs;
        }
    }
}
