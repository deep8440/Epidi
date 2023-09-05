using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Models.ViewModel.CorporateActions
{
    public class SpinoffViewModel
    {
        public int Id { get; set; }
        public int InstrumentId { get; set; }
        public int NewInstrumentId { get; set; }
        public string EquityToSpinOff { get; set; }
        public string NewEquitySpinned { get; set; }
        public decimal NewRatio { get; set; }
        public decimal SpinOffRatio { get; set; }
        public int SpinOffMDU { get; set; }
        public bool SpinOffAdjustment { get; set; }
        public string SpinOffRoundUnitsBuy { get; set; }
        public string SpinOffRoundUnitsSell { get; set; }

    }
    public class SpinOffOldOrderViewModel
    {
        public int ClientId { get; set; }
        public string TradingAccount { get; set; }
        public int OrderId { get; set; }
        public string SideName { get; set; }
        public string EquityName { get; set; }
        public string OpenTiming { get; set; }
        public string LegalEntity { get; set; }
        public decimal OpenPrice { get; set; }
        public decimal OpenUnits { get; set; }
        public decimal ClosePrice { get; set; }
        public decimal Swap { get; set; }
        public decimal Commission { get; set; }
        public decimal Leverage { get; set; }
        public string Position { get; set; }
    }
    public class SpinOffNewOrderViewModel
    {
        public int ClientId { get; set; }
        public string TradingAccount { get; set; }
        public int OrderId { get; set; }
        public int NewOrderId { get; set; }
        public string SideName { get; set; }
        public string NewEquityName { get; set; }
        public string OpenDateTiming { get; set; }
        public string LegalEntity { get; set; }
        public decimal ClosePriceBefore { get; set; }
        public decimal OpenUnitsOldEquity { get; set; }
        public decimal OpenPriceAfterSpinOff { get; set; }
        public decimal OpenUnitsAfterSpinOff { get; set; }
        public decimal Leverage { get; set; }
        public decimal OpenPriceOldEquity { get; set; }
        public decimal UnitsNoRound { get; set; }
        public decimal PriceDiff { get; set; }
        public decimal UnitsDiff { get; set; }
        public decimal SpinOffAdjustment { get; set; }
        public string Position { get; set; }
        public int NewInstrumentId { get; set; }
    }

    public class SpinOffOrderUpdateViewModel
    {
        public int OrderId { get; set; }
        public string Position { get; set; }
        public decimal OpenPriceAfterSpinOff { get; set; }
        public decimal OpenUnitsAfterSpinOff { get; set; }
        public decimal SpinOffAdjustment { get; set; }
        public int NewInstrumentId { get; set; }
    }
}
