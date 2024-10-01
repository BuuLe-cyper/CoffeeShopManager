using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObjects.DTOs.Message
{
    public class MessageDTO
    {
        public int TableID { get; set; }
        public Guid? UserID { get; set; }
        public string Content { get; set; }
        public DateTime SentAt { get; set; } = DateTime.Now;
        public UsersDTO User { get; set; }

    }
}
