using AutoMapper;
using BussinessObjects.DTOs.Message;
using BussinessObjects.Services;
using CoffeeShop.ViewModels.Message;
using DataAccess.DataContext;
using DataAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CoffeeShop.CoffeeShopHub
{
    [AllowAnonymous]
    public class ChatHub : Hub
    {
        private readonly IMessService _messService;
        private readonly IMapper _mapper;

        public ChatHub(IMessService messService, IMapper mapper)
        {
            _messService = messService;
            _mapper = mapper;
        }

        public async Task JoinRoom(string tableId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, tableId);

            var messages = _mapper.Map<IEnumerable<MessageVM>>(await _messService.GetMessageByTableId(int.Parse(tableId)))
                                    .OrderBy(m => m.SentAt)
                                    .ToList();

            var userRole = Context.User?.FindFirst(ClaimTypes.Role)?.Value ?? "User";

            foreach (var message in messages)
            {
                var displayName = string.IsNullOrEmpty(message.User?.Username) ? "User" : message.User.Username;
                var isAdminMessage = message.User?.AccountType == 1;
                var role = isAdminMessage ? "Admin" : "User";
                if(isAdminMessage)
                {
                    displayName = "Admin";
                }


                await Clients.Caller.SendAsync("ReceiveMessage", message.UserID.ToString(), role, message.Content, message.SentAt, displayName);
            }
        }

        public async Task SendMessage(string tableId, string userId, string messageContent)
        {
            var sentAt = DateTime.Now;
            var newMessage = new MessageVM
            {
                TableID = int.Parse(tableId),
                UserID = string.IsNullOrWhiteSpace(userId) || !Guid.TryParse(userId.Trim(), out Guid parsedUserId)
                ? (Guid?)null
                : parsedUserId,
                Content = messageContent,
                SentAt = sentAt,
            };
            await _messService.CreateMessage(_mapper.Map<MessageDTO>(newMessage));

            var displayName = Context.User?.FindFirst(ClaimTypes.Name)?.Value ?? "User";
            var isAdminMessage = Context.User?.FindFirst(ClaimTypes.Role)?.Value.Equals("Admin") ?? false;

            var role = isAdminMessage ? "Admin" : "User";
            if (isAdminMessage)
            {
                displayName = "Admin";
            }


            await Clients.Group(tableId).SendAsync("ReceiveMessage", userId, role, messageContent, sentAt, displayName);
        }
    }
}
