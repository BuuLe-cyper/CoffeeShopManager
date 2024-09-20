using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class OrderDetail
    {
        [Key]
        public int OrderDetailID { get; set; }

        [ForeignKey("Order")]
        public Guid OrderID { get; set; }
        public Order Order { get; set; }

        [ForeignKey("ProductSize")]
        public int ProductSizeID { get; set; }
        public ProductSize ProductSize { get; set; }

        public decimal UnitPrice { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Range(0, 1)]
        public float Discount { get; set; }

        // Audit Fields
        public bool IsDeleted { get; set; } = false;
        public bool IsActive { get; set; } = true;
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public DateTime? ModifyDate { get; set; }
    }
}
