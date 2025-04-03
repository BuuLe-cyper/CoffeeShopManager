using AutoMapper;
using BussinessObjects.DTOs.Message;
using BussinessObjects.Services;
using CoffeeShop.ViewModels.Message;
using DataAccess.DataContext;
using DataAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace CoffeeShop.CoffeeShopHub
{
    [AllowAnonymous]
    public class ChatHub : Hub
    {
        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;
        private readonly string _baseUrlApi;

        public ChatHub(IHttpClientFactory httpClientFactory, IMapper mapper, IConfiguration configuration)
        {
            _httpClient = httpClientFactory.CreateClient();
            _mapper = mapper;
            _baseUrlApi = configuration["BaseUrlApi"];
        }

        public async Task JoinRoom(string tableId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, tableId);


            await _httpClient.DeleteAsync($"{_baseUrlApi}/api/Messages/CleanOldMessages");

            var response = await _httpClient.GetAsync($"{_baseUrlApi}/api/Messages/ByTable/{tableId}");
            if (!response.IsSuccessStatusCode) return;

            var json = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(json);

            var messages = JsonSerializer.Deserialize<List<MessageVM>>(doc.RootElement.GetRawText(), new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new();

            var userRole = Context.User?.FindFirst(ClaimTypes.Role)?.Value ?? "User";

            foreach (var message in messages.OrderBy(m => m.SentAt))
            {
                var displayName = string.IsNullOrEmpty(message.User?.Username) ? "User" : message.User.Username;
                var isAdminMessage = message.User?.AccountType == 1;
                var role = isAdminMessage ? "Admin" : "User";
                if (isAdminMessage) displayName = "Admin";

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

            var json = JsonSerializer.Serialize(newMessage);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            await _httpClient.PostAsync($"{_baseUrlApi}/api/Messages", content);

            var displayName = Context.User?.FindFirst(ClaimTypes.Name)?.Value ?? "User";
            var isAdminMessage = Context.User?.FindFirst(ClaimTypes.Role)?.Value == "Admin";
            var role = isAdminMessage ? "Admin" : "User";
            if (isAdminMessage) displayName = "Admin";

            await Clients.Group(tableId).SendAsync("ReceiveMessage", userId, role, messageContent, sentAt, displayName);
        }

    }
}
