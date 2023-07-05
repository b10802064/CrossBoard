using CrossBorder.Models;
using System.ComponentModel.DataAnnotations;

namespace cross_border.ViewModels
{
    public class ProductinfoViewModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public Product Products { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public int Amount { get; set; }

    }
}
