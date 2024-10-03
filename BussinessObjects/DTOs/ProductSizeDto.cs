using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObjects.DTOs
{
    public class ProductSizeDto
    {
        public int ProductSizeID { get; set; }
        public int ProductID { get; set; }
        public int SizeID { get; set; }
        public decimal Price { get; set; }
        public decimal OriginalPrice { get; set; }
        public bool IsActive { get; set; }
    }
}
