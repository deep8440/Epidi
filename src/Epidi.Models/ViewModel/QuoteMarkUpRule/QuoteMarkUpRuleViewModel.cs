using Epidi.Models.ViewModel.LeverageRules;
using Epidi.Models.ViewModel.RuleCondition;
using Epidi.Models.ViewModel.RuleInstrument;
using Epidi.Models.ViewModel.RuleLpPriority;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Models.ViewModel.QuoteMarkUpRule
{
    public class QuoteMarkUpRuleViewModel
    {
        /// <summary>
        /// Gets or sets the Id value.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the Name value.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the StartTime value.
        /// </summary>
        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm:ss}", ApplyFormatInEditMode = true)]
        public string StartTime { get; set; }

        /// <summary>
        /// Gets or sets the EndTime value.
        /// </summary>
        public string EndTime { get; set; }

        /// <summary>
        /// Gets or sets the Day value.
        /// </summary>
        public int Day { get; set; }

        /// <summary>
        /// Gets or sets the MaxSpread value.
        /// </summary>
        public double MaxSpread { get; set; }

        /// <summary>
        /// Gets or sets the AutoupdateLPPriorityUp value.
        /// </summary>
        public bool AutoupdateLPPriorityUp { get; set; }
        public int Priority { get; set; }

        /// <summary>
        /// Gets or sets the AutoupdateLPPriorityDown value.
        /// </summary>
        public bool AutoupdateLPPriorityDown { get; set; }

        /// <summary>
        /// Gets or sets the CreatedBy value.
        /// </summary>
        public int CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the CreatedDate value.
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the UpdatedBy value.
        /// </summary>
        public int UpdatedBy { get; set; }

        /// <summary>
        /// Gets or sets the UpdatedDate value.
        /// </summary>
        public DateTime UpdatedDate { get; set; }

        /// <summary>
        /// Gets or sets the DeletedBy value.
        /// </summary>
        public int DeletedBy { get; set; }

        /// <summary>
        /// Gets or sets the DeletedDate value.
        /// </summary>
        public DateTime DeletedDate { get; set; }

        public RuleInstrumentViewModel objRuleInstrumentViewModel { get; set; }
        public RuleLpPriorityViewModel objRuleLpPriorityViewModel { get; set; }
        public RuleConditionViewModel objRuleConditionViewModel { get; set; }        
        [NotMapped]
        public string StartTimeStr => StartTime != null ? StartTime : "-";
        [NotMapped]
        public string EndTimeStr => EndTime != null ? EndTime : "-";
        public string mode { get; set; } = "Add";
    }
    public class QuoteMarkUpRuleViewModelImport
    {
        //public int RuleId { get; set; }
        public string RuleName { get; set; }
        public bool AutoupdateLPPriorityUp { get; set; }
        public int RuleInstrumentIdCounter { get; set; }
        public int Priority { get; set; }
        
        public string MasterInstrumentName { get; set; }
        public string SymbolGroups { get; set; }
        public string Day { get; set; }
        public string FromTime { get; set; }
        public string ToTime { get; set; }
        public double MaxSpread { get; set; }
        public int PriorityNo { get; set; }
        public string LPName { get; set; }
        public double Mbid { get; set; }
        public double Mask { get; set; }
        public long Timeout { get; set; }
        //public List<RuleLpPriority> ruleLpPriorities { get; set; }
    }
    public class RuleLpPriority
    {
        public int PriorityNo { get; set; }
        public string LPName { get; set; }
        public int Mbid { get; set; }
        public int Mask { get; set; }
        public long Timeout { get; set; }
    }
    public class QuoteMarkUpRuleViewModelEdit
    {
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the Name value.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the AutoupdateLPPriorityUp value.
        /// </summary>
        public bool AutoupdateLPPriorityUp { get; set; }
        public int Priority { get; set; }
        public string objRuleInstrumentViewModelstr { get; set; }
        public string objRuleLpPriorityViewModelstr { get; set; }
        public string objRuleConditionViewModelstr { get; set; }

        public List<RuleInstrumentLPViewModel> objRuleInstrumentViewModel { get; set; }
        public List<RuleLpPriorityViewModel_Dt> objRuleLpPriorityViewModel { get; set; }
        public List<LeverageRulesDtlViewModel> objRuleConditionViewModel { get; set; }
    }
}
