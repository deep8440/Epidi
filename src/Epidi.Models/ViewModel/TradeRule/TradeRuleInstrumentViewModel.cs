using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Models.ViewModel.TradeRule
{
    public class TradeRuleInstrumentViewModel
    {
        /// <summary>
        /// Gets or sets the Id value.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the RuleId value.
        /// </summary>
        public int RuleId { get; set; }

        /// <summary>
        /// Gets or sets the InstrumentId value.
        /// </summary>
        public int InstrumentId { get; set; }

        /// <summary>
        /// Gets or sets the LegalEntityId value.
        /// </summary>
        public int LegalEntityId { get; set; }

        /// <summary>
        /// Gets or sets the StartTime value.
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Gets or sets the EndTime value.
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// Gets or sets the Day value.
        /// </summary>
        public int Day { get; set; }

        /// <summary>
        /// Gets or sets the IBID value.
        /// </summary>
        public int IBID { get; set; }

        /// <summary>
        /// Gets or sets the CountryId value.
        /// </summary>
        public int CountryId { get; set; }

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
    }
    public class TradeRuleInstrumentViewModelGet
    {
        public int Id { get; set; }
        public int RuleId { get; set; }
        public int? InstrumentId { get; set; }
        public int? LegalEntityId { get; set; }
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy hh:mm:ss tt}")]
        public TimeSpan StartTime { get; set; }
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy hh:mm:ss tt}")]
        public TimeSpan EndTime { get; set; }
        public string Day { get; set; }
        public int? IBID { get; set; }
        public int? CountryId { get; set; }
        public string UID { get; set; }
        public int tblRulePriority_ID { get; set; }
        public int RuleInstrumentId { get; set; }
        public int LPId { get; set; }
        public int Priority { get; set; }
        public string HedgeType { get; set; }
        public int VolumeFrom { get; set; }
        public int VolumeTo { get; set; }
        public decimal Coefficient { get; set; }
        public int? PositionTypeId { get; set; }
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
        public string InstrumentName { get; set; }
        public string SymbolGroups { get; set; }
        public string IB { get; set; }
        public string LegalEntity { get; set; }
        public string CountryName { get; set; }
        public string LPName { get; set; }
        public string PositionType { get; set; }
    }
    public class TradeRuleInstrumentViewModelExport
    {
        public string InstrumentName { get; set; }
        //[JsonProperty("Symbol Groups")]
        public string SymbolGroups { get; set; }
        public string IB { get; set; }
        public string LegalEntity { get; set; }
        public string Country { get; set; }
        public string UID { get; set; }
        public int Day { get; set; }

        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy hh:mm:ss tt}")]
        public TimeSpan StartTime { get; set; }
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy hh:mm:ss tt}")]
        public TimeSpan EndTime { get; set; }
        public string LPName { get; set; }
        public int Priority { get; set; }
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
    }
    public class TradeRuleUniversalValues 
    {
        public int Id { get; set; }
        public string Values { get; set; }
    }

}
