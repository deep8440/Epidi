using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Models.ViewModel.Instrument
{
    public class GateIOInstrumentMappingViewModel
    {
        public int TradeStatus { get; set; }
        public string Name { get; set; }
        public string Base  { get; set; }
        public string Quote { get; set; }
        public decimal Fee { get; set; }
        public decimal MinBaseAmount { get; set; }
        public decimal MinQuoteAmount { get; set; }
        public decimal AmountPrecision { get; set; }
        public decimal Precision { get; set; }
        public decimal SellStart { get; set; }
        public decimal BuyStart { get; set; }
    }

    public class ImportGateIOInstrumentMappingViewModel
    {
        public int Id { get; set; }
        public int TradeStatus { get; set; }
        public string Name { get; set; }
        public string Base { get; set; }
        public string Quote { get; set; }
        public decimal Fee { get; set; }
        public decimal MinBaseAmount { get; set; }
        public decimal MinQuoteAmount { get; set; }
        public decimal AmountPrecision { get; set; }
        public decimal Precision { get; set; }
        public decimal SellStart { get; set; }
        public decimal BuyStart { get; set; }
    }
}
