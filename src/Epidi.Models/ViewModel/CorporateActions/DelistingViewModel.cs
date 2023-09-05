using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Models.ViewModel.CorporateActions
{
    public class DelistingViewModel
    {
        public int Id { get; set; }
        public int InstrumentId { get; set; }
        public string EquityName { get; set; }
    }

    public class DelistingOrderDetailsViewModel
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
        public string OpenPrice { get; set; }
        public string OpenUnits{ get; set; }
        public string ClosePrice { get; set; }
        public string Swap { get; set; }
        public string Commission { get; set; }
        public string Profit { get; set; }
        public string Leverage { get; set; }
        public string Position { get; set; }
    }
}
