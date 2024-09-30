using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CoffeeShop.ViewModels
{
    public class UserVM
    {
        [Key]
        public Guid UserID { get; set; }

        [Required]
        [StringLength(15)]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Username { get; set; } = string.Empty;
        
        [Required]
        [PasswordPropertyText]
        public string Password { get; set; } = string.Empty;

        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [Range(0, 1)]
        public int AccountType { get; set; } = 0; // 0: Customer, 1: Staff

        [StringLength(255)]
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
}
