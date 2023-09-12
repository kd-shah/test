using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealTimeChatApi.DataAccessLayer.Data;
using RealTimeChatApi.DataAccessLayer.Interfaces;
using RealTimeChatApi.DataAccessLayer.Models;
using System.Security.Claims;

namespace RealTimeChatApi.DataAccessLayer.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly RealTimeChatDbContext _context;
        public MessageRepository(IHttpContextAccessor httpContextAccessor,
            RealTimeChatDbContext context) 
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        
        public async Task<AppUser> GetSender()
        {
            var currentUserId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (currentUserId != null)
            {

                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == currentUserId);

                return user;
            }

            return null;
        }

        public async Task<AppUser> GetReceiver(string receiverId)
        {
            var receiver = await _context.Users.FindAsync(receiverId);      

            if (receiver != null)
            {
                return receiver;
            }

            return null;
        }

        public async Task<Message> GetMessage(int messageId)
        {
            var message = await _context.Messages.FirstOrDefaultAsync(m => m.messageId == messageId);
            
            if (message != null)
            {
                return message;
            }

            return null;
        }

        public async Task<IActionResult> SendMessage(Message message)
        {
            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            return null;
        }

        public async Task<IActionResult> EditMessage(Message message)
        {
            
                await _context.SaveChangesAsync();
                return new OkObjectResult("Message edited successfully");
            
        }

        public async Task<IActionResult> DeleteMessage(Message message)
        {
            _context.Messages.Remove(message);
            await _context.SaveChangesAsync();
            return new OkObjectResult("Message deleted successfully");

        }

        public async Task<IQueryable<Message>> GetConversationHistory(string id, AppUser authenticatedUser) 
        {
            var conversation = _context.Messages.Include(m => m.sender)
                .Include(m => m.receiver)
                .Where(m => (m.senderId == authenticatedUser.Id && m.receiverId == id) ||
                            (m.senderId == id && m.receiverId == authenticatedUser.Id));

            return conversation;
        }
        
        public async Task<IQueryable<Message>> SearchMessages(string userId , string query)
        {
            var messages = _context.Messages
        .Where(message =>
            (message.senderId == userId || message.receiverId == userId) && 
            message.content.Contains(query));

            return messages;
        }
    }
}
