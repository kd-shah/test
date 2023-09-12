using Microsoft.AspNetCore.Mvc;
using RealTimeChatApi.DataAccessLayer.Models;

namespace RealTimeChatApi.DataAccessLayer.Interfaces
{
    public interface IMessageRepository
    {
        Task<AppUser> GetSender();

        Task<AppUser> GetReceiver(string receiverId);

        Task<Message> GetMessage(int messageId);

        Task<IActionResult> SendMessage(Message message);

        Task<IActionResult> EditMessage(Message message);

        Task<IActionResult> DeleteMessage(Message message);

        Task<IQueryable<Message>> GetConversationHistory(string id, AppUser authenticateduser);

        Task<IQueryable<Message>> SearchMessages(string userId, string query);

    }
}
