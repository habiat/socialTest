using System.ComponentModel.DataAnnotations;

namespace Social.Core.ViewModel
{
    public class LoginModel
    {
        [Required]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }
}
