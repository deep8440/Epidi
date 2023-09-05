using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Models.ViewModel.LeverageRules
{
    public class LeverageRulesDtlViewModel
    {
        public int Id { get; set; }
        public string TableName { get; set; }
        public string FieldName { get; set; }
        public int LeverageRules { get; set; }
        public int? RuleConditionsId { get; set; }        
        /// <summary>
        /// Gets or sets the Opration value.
        /// </summary>
        public string Opration { get; set; }

        /// <summary>
        /// Gets or sets the OprationValue value.
        /// </summary>
        public string OprationValue { get; set; }

        /// <summary>
        /// Gets or sets the Datatype value.
        /// </summary>
        public string? Datatype { get; set; }

        /// <summary>
        /// Gets or sets the ForeignKey value.
        /// </summary>
        public int? ForeignKey { get; set; }
    }
}
