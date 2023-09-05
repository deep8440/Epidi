using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Models.Model
{
    public class AppUser : CommonField
    {
        public long Id { get; set; }

        public long CountryId { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }
        public string? OldPassword { get; set; }
        public string? NewPassword { get; set; }
        public string? ConfirmPassword { get; set; }

        public bool IsEmailConfirm { get; set; }
    }
}
