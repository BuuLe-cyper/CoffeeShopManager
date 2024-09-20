using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class Size
    {
        [Key]
        public int SizeID { get; set; }

        [Required]
        [StringLength(100)]
        public string SizeName { get; set; }
        public bool IsDeleted { get; set; } = false;
        public bool IsActive { get; set; } = true;
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public DateTime? ModifyDate { get; set; }

        public ICollection<ProductSize> ProductSizes { get; set; }
    }
}
