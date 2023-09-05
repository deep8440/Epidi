using Epidi.Models.ViewModel.LeverageRules;
using Epidi.Models.ViewModel.RuleCondition;
using Epidi.Models.ViewModel.RuleInstrument;
using Epidi.Models.ViewModel.RuleLpPriority;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Models.ViewModel.MarginRuleViewModel
{
    public class MarginRuleViewModel
    {
        /// <summary>
        /// Gets or sets the MarginRuleId value.
        /// </summary>
        public long MarginRuleId { get; set; }

        /// <summary>
        /// Gets or sets the Name value.
        /// </summary>
        public string RuleName { get; set; }

        /// <summary>
        /// Gets or sets the StopOutPercent value.
        /// </summary> 
        public int Priority { get; set; }

        /// <summary>
        /// Gets or sets the StopOutPercent value.
        /// </summary> 
        public int StopOutPercent { get; set; }

        /// <summary>
        /// Gets or sets the MarginCallPercent value.
        /// </summary>
        public int MarginCallPercent { get; set; }

        /// <summary>
        /// Gets or sets the IsPartialCloseoutonStopOut value.
        /// </summary>
        public bool IsPartialCloseoutonStopOut { get; set; }

        /// <summary>
        /// Gets or sets the LeveltotheStopOut value.
        /// </summary>
        public int LeveltotheStopOut { get; set; }

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
        public RuleConditionViewModel objRuleConditionViewModel { get; set; }
        public string mode { get; set; } = "Add";
    }

    public class MarginRuleViewModelEdit
    {
        /// <summary>
        /// Gets or sets the MarginRuleId value.
        /// </summary>
        public long MarginRuleId { get; set; }

        /// <summary>
        /// Gets or sets the Name value.
        /// </summary>
        public string RuleName { get; set; }

        /// <summary>
        /// Gets or sets the StopOutPercent value.
        /// </summary> 
        public int Priority { get; set; }

        /// <summary>
        /// Gets or sets the StopOutPercent value.
        /// </summary> 
        public int StopOutPercent { get; set; }

        /// <summary>
        /// Gets or sets the MarginCallPercent value.
        /// </summary>
        public int MarginCallPercent { get; set; }

        /// <summary>
        /// Gets or sets the IsPartialCloseoutonStopOut value.
        /// </summary>
        public bool IsPartialCloseoutonStopOut { get; set; }

        /// <summary>
        /// Gets or sets the LeveltotheStopOut value.
        /// </summary>
        public int LeveltotheStopOut { get; set; }

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
        public string objRuleConditionViewModelstr { get; set; }
        public List<LeverageRulesDtlViewModel> objRuleConditionViewModel { get; set; }
    }
}
