using ChatHubSignalR.Models;
using ChatHubSignalR.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace ChatHubSignalR
{
    public class ChatHub : Hub
    {
        private readonly ChatContext _context;

        public ChatHub(ChatContext dbContext) {
            _context = dbContext;
        }
        public async Task JoinGroup(string userId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, userId);
        }
        private readonly Dictionary<string, string> _userConnections = new();

        public override Task OnConnectedAsync()
        {

            //_userConnections[Context.UserIdentifier] = Context.ConnectionId;
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)

        {
            _userConnections.Remove(Context.UserIdentifier);

            return base.OnDisconnectedAsync(exception);
        }
        public async Task SendPrivateMessage(string userId, string message)
        {
            if (_userConnections.TryGetValue(userId, out var connectionId))
            {

                var chatMessage = new ChatMessage { User = userId, Message = message };

                _context.Messages.Add(chatMessage);
                await _context.SaveChangesAsync();
                await Clients.Client(connectionId).SendAsync("ReceiveMessage", message);
            }
        }
       
        public async Task SendMessage(string user, string message)
        {
            var chatMessage = new ChatMessage { User = user, Message = message };
            _context.Messages.Add(chatMessage);
            await _context.SaveChangesAsync();
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
