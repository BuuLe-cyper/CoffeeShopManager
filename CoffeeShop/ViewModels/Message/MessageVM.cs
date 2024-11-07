using DataAccess.Models;

namespace CoffeeShop.ViewModels.Message
{
    public class MessageVM
    {
        public int TableID { get; set; }
        public Guid? UserID { get; set; }
        public string Content { get; set; }
        public DateTime SentAt { get; set; } = DateTime.Now;
        public UserVM User { get; set; }
    }
}
