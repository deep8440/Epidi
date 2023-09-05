using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Models.ViewModel.Users
{
    public class UsersVsRulesViewModel
    {
        public int Id { get; set; }
        public int RuleId { get; set; }
        public int UserId { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? DeleteDate { get; set; }
    }
}
