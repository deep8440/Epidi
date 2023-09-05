using Epidi.Models.ViewModel.LeverageRules;
using Epidi.Models.ViewModel.RuleCondition;
using Epidi.Models.ViewModel.RuleInstrument;
using Epidi.Models.ViewModel.RuleLpPriority;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Models.ViewModel.SwapRule
{
    public class SwapRuleViewModel
    {
        public int Id { get; set; }
        public string RuleName { get; set; }
        public string Comment { get; set; }
        public long Priority { get; set; }
        
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy hh:mm:ss tt}")]
        public TimeSpan TimeToApply { get; set; }
        public RuleConditionViewModel objRuleConditionViewModel { get; set; }

    }

    public class EditSwapRuleViewModel
    {
        public int Id { get; set; }
        public string RuleName { get; set; }
        public string Comment { get; set; }
        public long Priority { get; set; }
        public TimeSpan TimeToApply { get; set; }
        public string objRuleInstrumentViewModelstr { get; set; }
        public string objRuleConditionViewModelstr { get; set; }


        public List<SwapRuleInstrumentViewModel> objRuleInstrumentViewModel { get; set; }
        public List<LeverageRulesDtlViewModel> objRuleConditionViewModel { get; set; }
    }
}
