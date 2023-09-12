using Microsoft.AspNetCore.SignalR;

namespace RealTimeChatApi.Hubs
{
    public sealed class ChatHub : Hub
    {
        //public async Task SendMessage(string receiverId, string content)
        //{
        //    await Clients.User(receiverId).SendAsync("ReceiveMessage", content);
        //}
        public override async Task OnConnectedAsync()
        {
            await Clients.All.SendAsync("ReceiveMessage" , $"{Context.ConnectionId}has joined");
        }

    }
}
