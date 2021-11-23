using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.UserVM
{
  public  class ResetPasswordVM
    {
    //    [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

//[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Email { get; set; }
        public string Token { get; set; }

    }
}
