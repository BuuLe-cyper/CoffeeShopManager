using DataAccess.Models;
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
        public Guid UserID { get; set; }
        public DateTime OrderDate { get; set; }
        public string? PaymentMethod { get; set; }
        public decimal TotalAmount { get; set; }
        public int TableID { get; set; }
        //public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
