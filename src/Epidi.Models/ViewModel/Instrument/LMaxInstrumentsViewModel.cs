using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Models.ViewModel.Instrument
{
    public class LMaxInstrumentsViewModel
    {
        public string Symbol { get; set; }
        public long Securityid { get; set; }
        public string Currency { get; set; }
        public string UOM { get; set; }
        public string AssetClass { get; set; }
        public decimal QtyIncrement { get; set; }
        public decimal PriceIncrement { get; set; }
        public bool TickerEnabled { get; set; }
    }

    public class ImportLMaxInstrumentsViewModel
    {
        public int Id { get; set; }
        public string Symbol { get; set; }
        public long Securityid { get; set; }
        public string Currency { get; set; }
        public string UOM { get; set; }
        public string AssetClass { get; set; }
        public decimal QtyIncrement { get; set; }
        public decimal PriceIncrement { get; set; }
        public bool TickerEnabled { get; set; }
    }
}
