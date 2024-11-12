namespace CoffeeShop.ViewModels
{
    public class CreatePaymentLinkRequest
    {
        public string UserInfor { get; set; }
        //show about "userName" payment
        public string Description { get; set; } = "BY PAYOS";
        public decimal TotalPrice { get; set; }
        public string returnUrl { get; set; } = "https://localhost:7197/Shared/DemoPaymentReturn/Success";
        public string cancelUrl { get; set; } = "https://localhost:7197/Shared/DemoPaymentReturn/Fail";
    }
}
