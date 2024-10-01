using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObjects.DTOs
{
    public class ProductViewDto
    {
        public int ProductID { get; set; }
        public int CategoryID { get; set; }
        public Category Category { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public float Discount { get; set; }
        public string ImageUrl { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? ModifyDate { get; set; }
        public ICollection<ProductSize> ProductSizes { get; set; }
    }
}
