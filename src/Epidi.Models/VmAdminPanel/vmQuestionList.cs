using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Models.VmAdminPanel
{
    public class vmQuestionList
    {
        public long QuestionId { get; set; }

        public string QuestionDesc { get; set; }

        public string QuestionInfo { get; set; }

        public string CountryName { get; set; }

        public int AnsType { get; set; }

        public bool IsActive { get; set; }
    }
}
