using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Models.ViewModel.CorporateActions
{
    public class SplitCalculatedOrderViewModel
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int MasterInstrumentId { get; set; }
        public int ClientId { get; set; }
        public string TradingAccount { get; set; }
        public string SideName { get; set; }
        public string EquityName { get; set; }
        public string LegalEntity { get; set; }
        public string OpenTiming { get; set; }
        public string OpenPriceBefore { get; set; }
        public string OpenUnitsBefore { get; set; }
        public string OpenPriceAfterSplit { get; set; }
        public string OpenUnitsAfterSplit { get; set; }
        public string PriceNoRound { get; set; }
        public string UnitsNoRound { get; set; }
        public string PriceDiff { get; set; }
        public string UnitsDiff { get; set; }
        public string SplitAdjustment { get; set; }
        public string Position { get; set; }
        public string Leverage { get; set; }
    }
}
