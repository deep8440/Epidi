using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Epidi.Models.ViewModel.LeverageRules;
using Epidi.Models.ViewModel.RuleCondition;
using Epidi.Models.ViewModel.RuleInstrument;
using Epidi.Models.ViewModel.RuleLpPriority;
using Epidi.Models.ViewModel.SwapRule;

namespace Epidi.Models.Model
{
    public class Promotion
    {
        public int Id { get; set; }
        public int RuleConditionId { get; set; }
        public string Name { get; set; }
        public int Priority { get; set; }
        public CreditPromotion creditPromotion { get; set; }
        public string ObjRuleConditions { get; set; }
        public string objRuleInstrumentViewModelstr { get; set; }
        public string objRuleConditionViewModelstr { get; set; }


        public List<SwapRuleInstrumentViewModel> objRuleInstrumentViewModel { get; set; }
        public List<LeverageRulesDtlViewModel> objRuleConditionViewModel { get; set; }
    }
    public class CreditPromotion
    {
        public int CreditPromoId { get; set; }
        public int Id { get; set; }
        public DateTime AcceptedDateTime { get; set; }
        public DateTime PostedDateTime { get; set; }
        public DateTime PromoExpDate { get; set; }
        public string PromoName { get; set; }
        public decimal PromoAmount { get; set; }
        public string Countries { get; set; }
        public string EnableTrigger { get; set; }

        public decimal PerOfDeposit { get; set; }
    }
    public class ReferPromotion
    {
        public int ReferFriendPromoId { get; set; }
        public int Id { get; set; }
        public string PromoRefCode { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal SenderRefFee { get; set; }
        public decimal RecipientFee { get; set; }
        public string EnableTrigger { get; set; }
        public string Countries { get; set; }
        public bool Active { get; set; }


    }
    public class FreeAssetPromotion
    {
        public int FreeAssetsPromoId { get; set; }
        public int Id { get; set; }
        public string PromoRefCode { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal MaxAmount { get; set; }
        public decimal AllowedShare { get; set; }
        public decimal MinDeposit { get; set; }
        public string EnableTrigger { get; set; }
        public string Countries { get; set; }
        public bool Active { get; set; }
    }
    public class VoucherPromotion
    {
        public string VoucherPromoId { get; set; }
        public string Id { get; set; }
        public string PromoRefCode { get; set; }
        public string Name { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string VoucherAmt { get; set; }
        public string MaxAllowedShare { get; set; }
        public string MinDeposit { get; set; }
        public string EnableTrigger { get; set; }
        public string Countries { get; set; }
        public string Active { get; set; }
    }
    public class RebatePromotion
    {
        public int RebatePromoId { get; set; }
        public int Id { get; set; }
        public string PromoRefCode { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal RebateAmt { get; set; }
        public string EnableTrigger { get; set; }
        public decimal RebateVol { get; set; }
        public string Countries { get; set; }
        public string Active { get; set; }

    }
}
