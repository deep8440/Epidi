using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Epidi.Enums
{
    public enum UserRole
    {
        [Display(Name = "Super Admin")]
        SUPERADMIN,
        [Display(Name = "Admin")]
        ADMIN,
        [Display(Name = "Partner")]
        IB,
        [Display(Name = "User")]
        USER
    }
}