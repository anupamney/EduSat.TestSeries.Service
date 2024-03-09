using System.ComponentModel.DataAnnotations;

namespace EduSat.TestSeries.Service.Models.DTOs
{
    public class UserResgistrationRequestDto
    {
        [Required(ErrorMessage = "Email is required")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required")]
        [Compare("Password", ErrorMessage = "Password and Confirm Password do not match")]
        public string? ConfirmPassword { get; set; }

    }
}
