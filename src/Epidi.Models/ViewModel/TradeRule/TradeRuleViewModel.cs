using Epidi.Models.ViewModel.LeverageRules;
using Epidi.Models.ViewModel.RuleCondition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Models.ViewModel.TradeRule
{
    public class TradeRuleViewModel
    {
        /// <summary>
        /// Gets or sets the Id value.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the Name value.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Priority value.
        /// </summary>
        public int Priority { get; set; }

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
        public bool IsActive { get; set; }

        public RuleConditionViewModel objRuleConditionViewModel { get; set; }
    }
    public class TradeRuleViewModelImport
    {
        public int RuleId { get; set; }
        public string Name { get; set; }
        public int Priority { get; set; }
        public int RuleInstrumentIdCounter { get; set; }
        public string MasterInstrumentName { get; set; }
        public string SymbolGroups { get; set; }
        public string IBName { get; set; }
        public string Uid { get; set; }
        public string CountryName { get; set; }
        public string LegalEntity { get; set; }
        public string Day { get; set; }
        public string TimeTo { get; set; }
        public string TimeFrom { get; set; }
        public string LPName { get; set; }
        public int PriorityNo { get; set; }
        public string HedgeType { get; set; }
        public int VolumeFrom { get; set; }
        public int VolumeTo { get; set; }
        public decimal Coefficient { get; set; }
        public string PositionType { get; set; }
        public decimal MaxCoefficient { get; set; }
        public decimal Value1 { get; set; }
        public decimal Value2 { get; set; }
        public decimal Value3 { get; set; }
        public decimal Value4 { get; set; }
        public decimal Value5 { get; set; }
        public decimal Value6 { get; set; }
        public decimal Value7 { get; set; }
        public decimal Value8 { get; set; }
        public decimal Value9 { get; set; }
        public decimal Value10 { get; set; }
        public decimal Value11 { get; set; }
        public decimal Value12 { get; set; }
        public decimal Value13 { get; set; }
        public decimal Value14 { get; set; }
        public decimal Value15 { get; set; }
        public decimal Value16 { get; set; }
        public decimal Value17 { get; set; }
        public decimal Value18 { get; set; }
        public decimal Value19 { get; set; }
        public decimal Value20 { get; set; }
        public decimal Value21 { get; set; }
        public decimal Value22 { get; set; }
        public decimal Value23 { get; set; }
        public decimal Value24 { get; set; }
        public decimal Value25 { get; set; }
        public decimal Value26 { get; set; }
        public decimal Value27 { get; set; }
        public decimal Value28 { get; set; }
    }
    public class TradeRuleViewModelEdit
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Priority { get; set; }
        public string objRuleInstrumentViewModelstr { get; set; }
        public string objRuleConditionViewModelstr { get; set; }
        public List<LeverageRulesDtlViewModel> objRuleConditionViewModel { get; set; }
        public List<TradeRuleInstrumentViewModelGet> objRuleInstrumentViewModel { get; set; }
    }

    public class TradeValuesViewModel
    {
        public int Id { get; set; }
        public int TradeRuleId { get; set; }
        public int TradeRuleInstrumentId { get; set; }
        public string TradeValueName { get; set; }
        public decimal TradeValues { get; set; }
    }
}
