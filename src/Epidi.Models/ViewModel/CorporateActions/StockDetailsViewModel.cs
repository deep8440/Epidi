using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Models.ViewModel.CorporateActions
{
    public class StockDetailsViewModel
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string TradingAccount { get; set; }
        public int SideId { get; set; }
        public string OpenTimingStr { get; set; }
        public decimal OpenPrice { get; set; }
        public decimal OpenUnits { get; set; }
        public long MasterInstrumentId { get; set; }
        public string Position { get; set; }
        public string CloseTimingStr { get; set; }
        public string SideName { get; set; }
        public string EquityName { get; set; }
        public string LegalEntity { get; set; }
        public double ClosePrice { get; set; }
        public double Swap { get; set; }
        public double Leverage { get; set; }
        public double Commission { get; set; }
        public double Profit { get; set; }
    }
}
