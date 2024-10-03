using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObjects.DTOs
{
    public class ProductSizesViewDto
    {
        public int ProductSizeID { get; set; }
        public int ProductID { get; set; }
        public Product Product { get; set; }
        public int SizeID { get; set; }
        public Size Size { get; set; }
        public decimal Price { get; set; }
        public decimal OriginalPrice { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreateDate { get; set; } 
        public DateTime? ModifyDate { get; set; }
    }
}
