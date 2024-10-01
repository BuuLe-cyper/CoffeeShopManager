using System.ComponentModel.DataAnnotations;

namespace CoffeeShop.ViewModels.Tables
{
    public class TableVM
    {
        public int TableID { get; set; }
        [StringLength(255)]
        public string Description { get; set; }
        [Display(Name = "QR Code")]
        public string? QRCodeTable { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? ModifyDate { get; set; }
    }
}
