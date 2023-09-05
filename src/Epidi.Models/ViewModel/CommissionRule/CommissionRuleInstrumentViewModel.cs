using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Models.ViewModel.CommissionRule
{
    public class CommissionRuleInstrumentViewModel
    {
        public int Id { get; set; }
        public int CommissionRuleId { get; set; }
        public int SymbolGroupId { get; set; }
        public int InstrumentId { get; set; }
        public string SymbolGroup { get; set; }
        public string InstrumentName { get; set; }
        public decimal BuyFeePer1000Notional { get; set; }
        public decimal SellFeePerNotional1000 { get; set; }
        public decimal BuyFeeNotional { get; set; }
        public decimal SellFeeNotional { get; set; }
        public decimal FeeBuyPerUnits { get; set; }
        public decimal FeeSellUnits { get; set; }
        public string Feecharge { get; set; }
        public string RoundingBuy { get; set; }
        public string RoundingSell { get; set; }
        public decimal DecimalCut { get; set; }
        public int DaysToApply { get; set; }
    }

    public class ImportCommissionRuleInstrumentViewModel
    {
        public int Id { get; set; }
        public string InstrumentName { get; set; }
        public string SymbolGroup { get; set; }
        public decimal BuyFeePer1000Notional { get; set; }
        public decimal SellFeePerNotional1000 { get; set; }
        public decimal BuyFeeNotional { get; set; }
        public decimal SellFeeNotional { get; set; }
        public decimal FeeBuyPerUnits { get; set; }
        public decimal FeeSellUnits { get; set; }
        public string Feecharge { get; set; }
        public string RoundingBuy { get; set; }
        public string RoundingSell { get; set; }
        public decimal DecimalCut { get; set; }
        public int DaysToApply { get; set; }
    }
}
