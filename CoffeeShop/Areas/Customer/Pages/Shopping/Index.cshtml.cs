using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeeShop.Areas.Customer.Pages.Shopping
{
    [AllowAnonymous]
    public class IndexModel : PageModel
    {
        public int TableId { get; set; }

        public void OnGet(int tableId)
        {
            TableId = tableId;
        }
    }
}
