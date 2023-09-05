using Epidi.Models.ViewModel.LeverageRules;
using Epidi.Models.ViewModel.RuleCondition;
using Epidi.Models.ViewModel.SwapRule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Models.ViewModel.CommissionRule
{
    public class CommissionRuleViewModel
    {
        public int Id { get; set; }
        public string Comment { get; set; }
        public int Priority { get; set; }
        public TimeSpan TimeToApply { get; set; }
        public int WhenToApplyComboId { get; set; }
        public string WhenToApplyCombo { get; set; }
        public string OrderComment { get; set; }
        public int Leverage { get; set; }
        public int UnitsTypeId { get; set; }
        public string UnitsType { get; set; }
        public string WhereToApplyId { get; set; }
        public int PercentageValue { get; set; }

        public bool IsMobileView { get; set; }

        public bool IsBrokerCommission { get; set;  }

        public RuleConditionViewModel objRuleConditionViewModel { get; set; }
        


    }

    public class EditCommissionRuleViewModel
    {
        public int Id { get; set; }
        public string Comment { get; set; }
        public int Priority { get; set; }
        public TimeSpan TimeToApply { get; set; }
        public int WhenToApplyComboId { get; set; }
        public string WhenToApplyCombo { get; set; }
        public string OrderComment { get; set; }
        public int Leverage { get; set; }
        public int UnitsTypeId { get; set; }
        public string   WhereToApplyId { get; set; }
        public int PercentageValue { get; set; }

        public bool IsMobileView { get; set; }

        public bool IsBrokerCommission { get; set; }

        public string objRuleInstrumentViewModelstr { get; set; }
        public string objRuleConditionViewModelstr { get; set; }
        public List<ImportCommissionRuleInstrumentViewModel> objRuleInstrumentViewModel { get; set; }
        public List<LeverageRulesDtlViewModel> objRuleConditionViewModel { get; set; }

       



    }
}
