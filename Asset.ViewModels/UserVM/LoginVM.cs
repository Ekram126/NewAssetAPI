using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.UserVM
{
  public  class LoginVM
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public string PasswordHash { get; set; }



        public string Lang { get; set; }
    }
}
