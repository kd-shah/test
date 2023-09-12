using Microsoft.AspNetCore.Mvc;
using RealTimeChatApi.BusinessLogicLayer.DTOs;

namespace RealTimeChatApi.BusinessLogicLayer.Interfaces
{
    public interface IMessageService
    {
        Task<IActionResult> SendMessage(string receiverId, [FromBody] SendMessageRequestDto request);

        Task<IActionResult> EditMessage(int messageId, [FromBody] EditMessageRequestDto request);

        Task<IActionResult> DeleteMessage(int messageId);

        Task<IActionResult> GetConversationHistory(string userId, DateTime before, int count = 20, string sort = "desc");

        Task<IActionResult> SearchConversations(string query);
    }

}
