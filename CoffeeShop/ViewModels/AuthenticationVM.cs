namespace CoffeeShop.ViewModels
{
    public class RegisterVM
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }

        public RegisterVM(string username, string password, string email)
        {
            Username = username;
            Password = password;
            Email = email;
        }
    }
    public class LoginVM
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public LoginVM(string username, string password)
        {
            Username=username;
            Password = password;
        }
    }
    public class LoginRes
    {
        public string Token { get; set; }
        public string Role { get; set; }
        public Guid UserId { get; set; }
        public string Username { get; set; }
    }
}
