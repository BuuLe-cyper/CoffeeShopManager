using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObjects.DTOs
{
    public class OrderDTO
    {
        public Guid OrderId { get; set; }
        public string? UserName { get; set; }
        public DateTime OrderDate { get; set; }
        public string? PaymentMethod { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
