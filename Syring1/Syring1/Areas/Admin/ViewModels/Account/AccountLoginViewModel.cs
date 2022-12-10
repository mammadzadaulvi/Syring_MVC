using System.ComponentModel.DataAnnotations;

namespace Syring1.Areas.Admin.ViewModels.Account
{
    public class AccountLoginViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }
        public string? ReturnUrl { get; set; }
    }
}
