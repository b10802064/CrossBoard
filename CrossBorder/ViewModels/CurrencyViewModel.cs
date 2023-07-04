using CrossBorder.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace cross_border.ViewModels
{
    public class CurrencyViewModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public string Photo { get; set; }

    }
}
