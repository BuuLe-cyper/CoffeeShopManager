using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeeShop.Areas.Shared.Pages.DemoPaymentReturn
{
    [AllowAnonymous]
    public class FailModel : PageModel
    {

        public void OnGet()
        {
        }
    }
}
