using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Models.ViewModel.MarkupRuleVsUserId
{
    public class MarkupRuleVsUserId
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int MarkUpRuleId { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime DeletedDate { get; set; }
        public bool DeletedBy { get; set; }
    }
    
}
