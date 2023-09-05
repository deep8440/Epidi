using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Models.ViewModel.CorporateActions
{
    public class ReverseSplitViewModel
    {
        public int InstrumentId { get; set; }
        public string EquityName { get; set; }
        public decimal ReverseSplitOldValue { get; set; }
        public decimal ReverseSplitValue { get; set; }
        public int ReverseSplitMDU { get; set; }
        public bool ReverseSplitIsAdjustment { get; set; }
        public string ReverseSplitRoundPriceNUnitsBuy { get; set; }
        public string ReverseSplitRoundPriceNUnitsSell { get; set; }
    }
}
