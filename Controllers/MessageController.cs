using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealTimeChatApi.BusinessLogicLayer.DTOs;
using RealTimeChatApi.BusinessLogicLayer.Interfaces;

namespace RealTimeChatApi.Controllers
{
    [Authorize]
    [Route("api/messages")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        public readonly IMessageService _messageService;
        public MessageController(IMessageService messageService)
        {
            _messageService = messageService;
        }
        [HttpPost]
        public async Task<IActionResult> SendMessage(string receiverId, [FromBody] SendMessageRequestDto request)
        {
            return await _messageService.SendMessage(receiverId, request);
        }

        [HttpPut("{messageId}")]
        public async Task<IActionResult> EditMessage(int messageId, [FromBody] EditMessageRequestDto request)
        {
            return await _messageService.EditMessage(messageId, request);
        }

        [HttpDelete("{messageId}")]
        public async Task<IActionResult> DeleteMessage(int messageId)
        {
            return await _messageService.DeleteMessage(messageId);
        }

        [HttpGet]
        public async Task<IActionResult> GetConversationHistory(string userId, DateTime before, int count = 20, string sort = "desc") 
        { 
            return await _messageService.GetConversationHistory(userId, before, count = 20, sort = "desc");
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchConversations(string query)
        {
            return await _messageService.SearchConversations(query);
        }
    }
}
