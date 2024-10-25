namespace CoffeeShop.ViewModels
{
    public class CreatePaymentLinkRequest
    {
        public string OrderName { get; set; }
        //show about "userName" payment
        public string Description { get; set; }
        public long TotalPrice { get; set; }
        public string returnUrl { get; set; } = "https://localhost:7197/DemoPaymentReturn/Success";
        public string cancelUrl { get; set; } = "https://localhost:7197/DemoPaymentReturn/Fail";
    }
}
