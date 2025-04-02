using System.ComponentModel.DataAnnotations;

namespace CoffeeShopAPI.RequestModel
{
    public class LoginRequest
    {
        public string Username { get; set; }

        public string Password { get; set; }
    }

    public class RegisterRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}
