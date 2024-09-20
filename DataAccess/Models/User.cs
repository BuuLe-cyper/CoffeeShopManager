using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class User
    {
        [Key]
        public Guid UserID { get; set; }

        [Required]
        [StringLength(15)]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [StringLength(100)]
        public string FullName { get; set; }

        [Required]
        public int AccountType { get; set; } // 0: Customer, 1: Staff

        [StringLength(255)]
        public string Email { get; set; }
        public bool IsDeleted { get; set; } = false;
        public bool IsActive { get; set; } = true;
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public DateTime? ModifyDate { get; set; }

        public ICollection<Message> Messages { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
