using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class Table
    {
        [Key]
        public int TableID { get; set; }

        [StringLength(255)]
        public string Description { get; set; }
        public string? QRCodeTable { get; set; }
        public bool IsDeleted { get; set; } = false;
        public bool IsActive { get; set; } = true;
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public DateTime? ModifyDate { get; set; }

        public ICollection<Message> Messages { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
