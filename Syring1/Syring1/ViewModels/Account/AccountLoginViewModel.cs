using System.ComponentModel.DataAnnotations;

namespace Syring1.ViewModels.Account
{
    public class AccountLoginViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required, MaxLength(100), DataType(DataType.Password)]
        public string Password { get; set; }
        public string? ReturnUrl { get; set; }
    }
}
