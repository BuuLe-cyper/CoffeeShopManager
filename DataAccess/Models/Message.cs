using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class Message
    {
        [Key]
        public int MessageID { get; set; }

        [ForeignKey("Table")]
        public int TableID { get; set; }
        public Table Table { get; set; }

        [ForeignKey("User")]
        public Guid? UserID { get; set; }
        public User User { get; set; }

        public DateTime SentAt { get; set; } = DateTime.Now;

        [Required]
        public string Content { get; set; }
        public bool IsDeleted { get; set; } = false;
        public bool IsActive { get; set; } = true;
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public DateTime? ModifyDate { get; set; }
    }

}
