using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class Product
    {
        [Key]
        public int ProductID { get; set; }

        [ForeignKey("Category")]
        public int CategoryID { get; set; }
        public Category Category { get; set; }

        [Required]
        [StringLength(255)]
        public string ProductName { get; set; }

        public string ProductDescription { get; set; }

        [Range(0, 1)]
        public float Discount { get; set; }

        public string ImageUrl { get; set; }

        public bool IsAvailable { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public bool IsActive { get; set; } = true;
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public DateTime? ModifyDate { get; set; }

        public ICollection<ProductSize> ProductSizes { get; set; }
    }

}
