using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class Order
    {
        [Key]
        public Guid OrderID { get; set; }

        [ForeignKey("User")]
        public Guid UserID { get; set; }
        public User User { get; set; }

        [ForeignKey("Table")]
        public int TableID { get; set; }
        public Table Table { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.Now;

        [Required]
        [StringLength(50)]
        public string PaymentMethod { get; set; }

        public decimal TotalAmount { get; set; }
        public bool IsDeleted { get; set; } = false;
        public bool IsActive { get; set; } = true;
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public DateTime? ModifyDate { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
