using Epidi.Models.ViewModel.LeverageRules;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Models.ViewModel.RuleCondition
{
    public class RuleConditionViewModel
    {
		/// <summary>
		/// Gets or sets the Id value.
		/// </summary>
		public int Id{ get; set; }

		/// <summary>
		/// Gets or sets the RuleId value.
		/// </summary>
		public int RuleId{ get; set; }

        /// <summary>
        /// Gets or sets the RuleId value.
        /// </summary>
        public int SwapRuleId { get; set; }

        /// <summary>
        /// Gets or sets the RuleId value.
        /// </summary>
        public int TradeRuleId { get; set; }

        /// <summary>
        /// Gets or sets the RuleId value.
        /// </summary>
        public int CommissionRuleId { get; set; }

        /// <summary>
        /// Gets or sets the RuleId value.
        /// </summary>
        public int SpecificationRuleId { get; set; }

        /// <summary>
        /// Gets or sets the MarginRuleId value.
        /// </summary>
        public int MarginRuleId{ get; set; }

		/// <summary>
		/// Gets or sets the FieldName value.
		/// </summary>
		public string FieldName{ get; set; }

		/// <summary>
		/// Gets or sets the MatchingValues value.
		/// </summary>
		public string MatchingValues{ get; set; }

		/// <summary>
		/// Gets or sets the SqlQuery value.
		/// </summary>
		public string SqlQuery{ get; set; }

		/// <summary>
		/// Gets or sets the Opration value.
		/// </summary>
		public string Opration
		{ get; set; }

		/// <summary>
		/// Gets or sets the CreatedDate value.
		/// </summary>
		public DateTime CreatedDate{ get; set; }

		/// <summary>
		/// Gets or sets the CreatedBy value.
		/// </summary>
		public int CreatedBy{ get; set; }

		/// <summary>
		/// Gets or sets the UpdatedBy value.
		/// </summary>
		public int UpdatedBy{ get; set; }

		/// <summary>
		/// Gets or sets the UpdatedDate value.
		/// </summary>
		public DateTime UpdatedDate{ get; set; }

		/// <summary>
		/// Gets or sets the DeletedBy value.
		/// </summary>
		public int DeletedBy{ get; set; }

        /// <summary>
        /// Gets or sets the LeverageRuleId value.
        /// </summary>
        public int LeverageRuleId { get; set; }
        public int PromotionRuleId { get; set; }
        /// <summary>
        /// Gets or sets the DeletedDate value.
        /// </summary>
        public DateTime DeletedDate{ get; set; }
		[NotMapped]
		public string QuoteMarkUpRuleName { get; set; }
		public string conditionRulesDtlObj { get; set; }
		public List<LeverageRulesDtlViewModel> rulesDtlViewModel { get; set; }
	}
	public class RuleConditionViewModel_Dt
	{
		public int Id { get; set; }
		public int RuleId { get; set; }
		public int RuleConditionsId { get; set; }
		public string SqlQuery { get; set; }
		public string FieldName { get; set; }
		public string Opration { get; set; }
		public string OprationValue { get; set; }
		public string Datatype { get; set; }
		public int ForeignKey { get; set; }
		public string Value { get; set; }

	}
}
