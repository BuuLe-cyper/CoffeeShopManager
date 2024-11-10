using AutoMapper;
using BussinessObjects.Services;
using CoffeeShop.ViewModels.Message;
using DataAccess.DataContext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NuGet.Protocol.Plugins;
using System.Security.Claims;

namespace CoffeeShop.Areas.Customer.Pages.Chats
{
    [AllowAnonymous]
    public class ChatModel : PageModel
    {
        public string TableId { get; set; }

        public void OnGet(int tableId)
        {
            TableId = tableId.ToString();

            var userId = User.FindFirst("userId")?.Value;
            ViewData["UserId"] = userId;

            var userRole = User.FindFirst(ClaimTypes.Role)?.Value ?? "User";
            ViewData["Role"] = userRole == "Admin" ? "Admin" : "User";
        }
    }
}
