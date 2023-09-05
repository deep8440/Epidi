using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Models.ViewModel.RuleLpPriority
{
    public class RuleLpPriorityViewModel
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
        /// Gets or sets the LPId value.
        /// </summary>
        public int LPId { get; set; }

        /// <summary>
        /// Gets or sets the Priority value.
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// Gets or sets the MarkUpValue value.
        /// </summary>
        public double MarkUpValue { get; set; }
        public int Mbid { get; set; }
        public int Mask { get; set; }
        public int Timeout { get; set; }
        public int RuleInstrumentId { get; set; }

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
        public string PRuleName { get; set; }
    }
    public class RuleLpPriorityViewModel_Dt
    {
        public int Id { get; set; }
        public int RuleId { get; set; }
        public int RuleInstrumentId { get; set; }
        public int LPId { get; set; }
        public int Priority { get; set; }
        public double MarkUpValue { get; set; }
        public int Mbid { get; set; }
        public int Mask { get; set; }
        public int Timeout { get; set; }
        public int CreatedBy { get; set; }
    }


}
