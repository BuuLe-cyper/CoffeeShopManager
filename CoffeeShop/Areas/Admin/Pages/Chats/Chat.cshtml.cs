using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeeShop.Areas.Admin.Pages.Chats
{
    [Authorize(Roles="Admin")]
    public class ChatModel : PageModel
    {
        public string TableId { get; set; }

        public void OnGet(int tableId)
        {
            TableId = tableId.ToString();
            var userId = User.FindFirst("userId")?.Value;
            ViewData["UserId"] = userId;

        }
    }
}
