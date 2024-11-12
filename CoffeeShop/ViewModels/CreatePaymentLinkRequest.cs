namespace CoffeeShop.ViewModels
{
    public class CreatePaymentLinkRequest
    {
        public string UserInfor { get; set; }
        //show about "userName" payment
        public string Description { get; set; } = "BY PAYOS";
        public decimal TotalPrice { get; set; }
        public string returnUrl { get; set; } = "https://coffeeshopprn221.azurewebsites.net/Shared/DemoPaymentReturn/Success";
        public string cancelUrl { get; set; } = "https://coffeeshopprn221.azurewebsites.net/Shared/DemoPaymentReturn/Fail";
    }
}
