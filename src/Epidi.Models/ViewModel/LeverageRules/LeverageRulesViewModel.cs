using Epidi.Models.ViewModel.LeverageRules;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Models.ViewModel.LeverageRule
{
    public class LeverageRulesViewModel
    {
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the Rule value.
        /// </summary>
        public string RuleName { get; set; }

        /// <summary>
        /// Gets or sets the LeverageValue value.
        /// </summary>
        public string LevregeValue { get; set; }

        /// <summary>
        /// Gets or sets the Priority value.
        /// </summary>
        public int Priority { get; set; }
        /// <summary>
        /// Gets or sets the IsActive value.
        /// </summary>
        public bool IsActive { get; set; }
        /// <summary>
        /// Gets or sets the SqlQuery value.
        /// </summary>
        public string? SqlQuery { get; set; }
        /// <summary>
        /// Gets or sets the CreatedDate value.
        /// </summary>
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the CreatedBy value.
        /// </summary>
        public int? CreatedBy { get; set; }
        /// <summary>
        /// Gets or sets the UpdatedAt value.
        /// </summary>
        public int? UpdatedAt { get; set; }
        [NotMapped]
        public string? FieldName { get; set; }
        /// <summary>
        /// Gets or sets the UpdatedDate value.
        /// </summary>
        public DateTime? UpdatedDate { get; set; }
        public string leverageRulesDtlObj { get; set; }

        public List<LeverageRulesDtlViewModel> leverageRulesDtlViewModels { get; set; }
    }

}
