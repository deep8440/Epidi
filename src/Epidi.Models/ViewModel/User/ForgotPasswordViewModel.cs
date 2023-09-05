using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Models.ViewModel.User
{
    public class ForgotPasswordViewModel
    {
        public int UserId { get; set; }
        public string OldPassword { get; set; }
        public string Password { get; set; }
        public string ReEnterPassword { get; set; }
    }
    public class ChangePasswordByEmailIdViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ReEnterPassword { get; set; }
    }

    public class ChangePasswordViewModel
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
