using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Models.ViewModel.CorporateActions
{
    public class SplitViewModel
    {
        public int InstrumentId { get; set; }
        public string EquityName { get; set; }
        public decimal OldValue { get; set; }
        public decimal SplitValue { get; set; }
        public int MDU { get; set; }
        public bool IsAdjustment { get; set; }
        public string RoundPriceNUnitsBuy { get; set; }
        public string RoundPriceNUnitsSell { get; set; }
    }

    public class AddSplitViewModel
    {
        public int OrderId { get; set; }
        public string Position { get; set; }
        public decimal OpenPrice { get; set; }
        public decimal SplitAdjustment { get; set; }
        public decimal OpenUnits { get; set; }
    }
}
