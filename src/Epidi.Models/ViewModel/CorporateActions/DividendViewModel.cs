using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Models.ViewModel.CorporateActions
{
    public class DividendViewModel
    {
        public int Id { get; set; }
        public int InstrumentId { get; set; }
    }

    public class DividendDetailsViewModel
    {
        public int Id { get; set; }
        public string InstrumentName { get; set; }
        public int InstrumentId { get; set; }
        public string SymbolGroupName { get; set; }
        public TimeSpan TimeTo { get; set; }
        public string TaxApplyBuyLeverageEqulTo1 { get; set; }
        public string TaxApplyBuyLeverageGreaterThen1 { get; set; }
        public string TaxApplySell { get; set; }
        public decimal DividendPerUnit { get; set; }
        public decimal DividendPerUnitSell { get; set; }
        public decimal DividendPerUnitBuyLeverageGreaterThen1 { get; set; }
        public decimal DividendPerUnitBuyLeverageEqualTo1 { get; set; }
        public string ExDividendDate { get; set; }
      
    }

    public class DividendOrdersViewModel
    {
        public int ClientId { get; set; }
        public string TradingAccount { get; set; }
        public int OrderId { get; set; }
        public string SideName { get; set; }
        public string EquityName { get; set; }
        public string OpenDateTiming { get; set; }
        public string LegalEntity { get; set; }
        public decimal OpenPrice { get; set; }
        public decimal OpenUnits { get; set; }
        public decimal Leverage { get; set; }
        public decimal DividendPerUnitSell { get; set; }
        public decimal DividendPerUnitBuyLeverageGreaterThen1 { get; set; }
        public decimal DividendPerUnitBuyLeverageEqualTo1 { get; set; }
        public DateTime ExDividendDate { get; set; }
        public decimal DividendInteger { get; set; }
        public decimal DividendDecimal { get; set; }
        public decimal DividendTotal { get; set; }
        public int MasterInstrumentId { get; set; }
        public string Position { get; set; }
    }
}
