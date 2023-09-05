using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Models.ViewModel.User
{
    public class UpdatePasswordViewModel
    {
        public long Id { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }
    }
}
