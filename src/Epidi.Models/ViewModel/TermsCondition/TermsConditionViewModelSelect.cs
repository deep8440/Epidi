using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Models.ViewModel.TermsCondition
{
    public class TermsConditionViewModelSelect
    {
        public int Id { get; set; }
        public string TermsConditionText { get; set; }


        public string FileUrl { get; set; }

        public static implicit operator TermsConditionViewModelSelect(TermsConditionViewModel v)
        {
            throw new NotImplementedException();
        }
    }
}
