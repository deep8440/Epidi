using Epidi.Models.ViewModel.RuleLpPriority;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Models.ViewModel.RuleInstrument
{
    public class RuleInstrumentViewModel
    {
        /// <summary>
        /// Gets or sets the Id value.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the RuleId value.
        /// </summary>
        public int RuleId { get; set; }

        ///// <summary>
        ///// Gets or sets the InstrumentIds value.
        ///// </summary>
        //public string InstrumentIds { get; set; }

        /// <summary>
        /// Gets or sets the InstrumentId value.
        /// </summary>
        public int InstrumentId { get; set; }

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
        /// Gets or sets the MaxSpread value.
        /// </summary>
        public double MaxSpread { get; set; }

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
        [NotMapped]
        public string QuoteMarkUpRuleName { get; set; }
        [NotMapped]
        public string MasterInstrumentName { get; set; }
        [NotMapped]
        public List<RuleLpPriorityViewModel> ruleLpPriorityViewModelList { get; set; }
    }
    public class RuleInstrumentViewModel_Dt
    {
        public int Id { get; set; }
        public int RuleId { get; set; }
        public int InstrumentId { get; set; }

        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy hh:mm tt}")]
        public TimeSpan StartTime { get; set; }

        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy hh:mm tt}")]
        public TimeSpan EndTime { get; set; }
        public int Day { get; set; }
        public double MaxSpread { get; set; }
        public int CreatedBy { get; set; }
    }
    public class RuleInstrumentLPViewModel
    {
        public int Id { get; set; }
        public int RuleId { get; set; }
        public int InstrumentId { get; set; }
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy hh:mm tt}")]
        public TimeSpan StartTime { get; set; }
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy hh:mm tt}")]
        public TimeSpan EndTime { get; set; }
        public int? Day { get; set; }
        public double MaxSpread { get; set; }
        public int tblLP_ID { get; set; }
        public int? LPId { get; set; }
        public int Priority { get; set; }
        public double MarkUpValue { get; set; }
        public double Mbid { get; set; }
        public double Mask { get; set; }
        public int Timeout { get; set; }
        public int RuleInstrumentId { get; set; }
        public int CreatedBy { get; set; }
        public string InstrumentName { get; set; }
    }
    public class RuleInstrumentLPViewModelExport
    {
        //[JsonProperty("Instrument Name")]
        public int RuleId { get; set; }
        public string InstrumentName { get; set; }
        //[JsonProperty("Symbol Groups")]
        public string SymbolGroups { get; set; }
        public int? Day { get; set; }
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy hh:mm tt}")]
        //[JsonProperty("Time to")]
        public TimeSpan StartTime { get; set; }
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy hh:mm tt}")]
        //[JsonProperty("Time From")]
        public TimeSpan EndTime { get; set; }
        //[JsonProperty("Max Spread")]
        public double MaxSpread { get; set; }
        //[JsonProperty("LP")]
        public string LPName { get; set; }
        //[JsonProperty("Priority")]
        public int Priority { get; set; }
        //[JsonProperty("Mbid")]
        public double Mbid { get; set; }
        //[JsonProperty("Mask")]
        public double Mask { get; set; }
        //[JsonProperty("Timeout")]
        public int Timeout { get; set; }
    }
}
