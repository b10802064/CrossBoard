using CrossBorder.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace cross_border.ViewModels
{
    public class IndexViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public IEnumerable<Product> Products { get; set; }
        [Required]
        public IEnumerable<Product> Productsman { get; set; }
        [Required]
        public IEnumerable<Product> Productswoman { get; set; }
        [Required]
        public IEnumerable<Product> Productskid { get; set; }
        [Required]
        public IEnumerable<Product> Productsbaby { get; set; }

    }
}