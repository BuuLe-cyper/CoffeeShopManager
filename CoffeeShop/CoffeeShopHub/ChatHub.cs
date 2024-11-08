using AutoMapper;
using BussinessObjects.DTOs.Message;
using BussinessObjects.Services;
using CoffeeShop.ViewModels.Message;
using DataAccess.DataContext;
using DataAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using NuGet.Packaging.Signing;
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

            var userRole = Context.User?.FindFirst(ClaimTypes.Role)?.Value;
            var isAdminPage = userRole?.Equals("Admin") == true ? true : false;


            foreach (var message in messages)
            {
                // Check if User is null and assign the correct display name
                var displayName = message.User != null && !string.IsNullOrEmpty(message.User.Username)
                    ? message.User.Username
                    : "User";

                var isAdminMessage = message.User != null && message.User.AccountType == 1;
                var role = isAdminMessage ? "Admin" : displayName;

                var alignment = isAdminPage
                    ? (isAdminMessage ? "right" : "left")
                    : (isAdminMessage ? "left" : "right");

                await Clients.Caller.SendAsync("ReceiveMessage", message.UserID.ToString(), role, message.Content, message.SentAt, alignment);
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

            var isAdminMessage = Context.User?.FindFirst(ClaimTypes.Role)?.Value.Equals("Admin") ?? false;
            var userNameFromClaim = !string.IsNullOrEmpty(Context.User?.FindFirst(ClaimTypes.Name)?.Value)
                        ? Context.User.FindFirst(ClaimTypes.Name).Value
                        : "User";

            var role = Context.User?.FindFirst(ClaimTypes.Role)?.Value?.Equals("Admin") == true ? "Admin" : userNameFromClaim;

            var userRole = Context.User?.FindFirst(ClaimTypes.Role)?.Value.Equals("Admin");
            var isAdminPage = userRole == true ? true : false;


            var alignment = isAdminPage
                ? (isAdminMessage ? "right" : "left")
                : (isAdminMessage ? "left" : "right");

            await Clients.Group(tableId).SendAsync("ReceiveMessage", userId, role, messageContent, sentAt, alignment);
        }
    }
}
