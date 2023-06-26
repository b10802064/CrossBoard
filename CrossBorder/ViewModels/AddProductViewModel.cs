using System.ComponentModel.DataAnnotations;

namespace cross_border.ViewModels
{
    public class AddProductViewModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string ProductId { get; set; }

    }
}
