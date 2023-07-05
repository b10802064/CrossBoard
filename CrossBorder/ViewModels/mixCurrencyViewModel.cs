using CrossBorder.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace cross_border.ViewModels
{
    public class mixCurrencyViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public IEnumerable<ProductinfoViewModel> Products { get; set; }
        [Required]
        public IEnumerable<CurrencyViewModel> currencyVMs { get; set; }

    }
}