using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using SportsLeague.Data;
using SportsLeague.Models;
using System;
using System.Threading.Tasks;

namespace SportsLeague.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly ApplicationDbContext _context;

        public ChatHub(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task JoinTeamChat(string teamId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, teamId);
        }

        public async Task SendMessageToTeam(string teamId, string message, string filePath)
        {
            if (!int.TryParse(teamId, out var parsedTeamId))
            {
                return;
            }

            var senderName = Context.User?.Identity?.Name ?? "Anonymous";
            var chatMessage = new ChatMessage
            {
                TeamId = parsedTeamId,
                SenderName = senderName,
                MessageText = message ?? string.Empty,
                FilePath = string.IsNullOrWhiteSpace(filePath) ? null : filePath,
                Timestamp = DateTime.Now
            };

            _context.ChatMessages.Add(chatMessage);
            await _context.SaveChangesAsync();

            await Clients.Group(teamId).SendAsync(
                "ReceiveMessage",
                senderName,
                message,
                filePath,
                chatMessage.Timestamp.ToString("g"));
        }
    }
}
