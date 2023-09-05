using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Models.ViewModel.SwapRule
{
    public class SwapRuleInstrumentViewModel
    {
        public int? Id { get; set; }
        public int? SwapRuleId { get; set; }
        public int? InstrumentId { get; set; }
        public int? SymbolGroupId { get; set; }
        public string InstrumentName { get; set; }
        public string SymbolGroup { get; set; }
        public decimal BuySwapPer1000Notional { get; set; }
        public decimal SellSwapPer1000Notional { get; set; }
        public decimal BuySwapNotional { get; set; }
        public decimal SellSwapNotional { get; set; }
        public int? TrippleSwapsDay { get; set; }
        public string DaystoApply { get; set; }
        public string RoundingBuy { get; set; }
        public string RoundingSell { get; set; }
        public decimal DecimalCut { get; set; }
        public long Priority { get; set; }
    }

    public class ImportSwapRuleInstrumentViewModel
    {
        public int Id { get; set; }
        public int InstrumentId { get; set; }
        public int SymbolGroupId { get; set; }

        public string InstrumentName { get; set; }
        public string SymbolGroup { get; set; }
        public decimal BuySwapPer1000Notional { get; set; }
        public decimal SellSwapPer1000Notional { get; set; }
        public decimal BuySwapNotional { get; set; }
        public decimal SellSwapNotional { get; set; }
        public int TrippleSwapsDay { get; set; }
        public string DaystoApply { get; set; }
        public string RoundingBuy { get; set; }
        public string RoundingSell { get; set; }
        public decimal DecimalCut { get; set; }
        public long Priority { get; set; }
    }
}
