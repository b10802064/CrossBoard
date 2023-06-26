using System.ComponentModel.DataAnnotations;

namespace cross_border.ViewModels
{
    public class RegisterViewModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int Verificationcode { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]



        public string ConfirmPassword { get; set; }
    }
}
