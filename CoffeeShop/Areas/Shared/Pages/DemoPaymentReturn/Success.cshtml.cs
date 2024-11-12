using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeeShop.Areas.Shared.Pages.DemoPaymentReturn
{
    [AllowAnonymous]
    public class SuccessModel : PageModel
    {
        public void OnGet()
        {
        }

    }
}
