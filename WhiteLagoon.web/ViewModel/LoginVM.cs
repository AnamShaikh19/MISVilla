using System.ComponentModel.DataAnnotations;

namespace WhiteLagoon.web.ViewModel
{
    public class LoginVM
    {
        [Required]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool Rememberme { get; set; }
        public string? RedirectUrl { get; set; }
       
    }
}
