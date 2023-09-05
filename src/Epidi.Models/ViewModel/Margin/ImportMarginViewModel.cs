using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Models.ViewModel.Margin
{
    public class ImportMarginViewModel
    {
        public int Id { get; set; }
        public string MarginType { get; set; }
        public string InstrumentEnd { get; set; }
        public string MasterInstrumentName { get; set; }
        public string SymbolGroup { get; set; }
        public string ConversionInstrumentName { get; set; }
        public decimal ManualConversion { get; set; }
        public string InstrumentToConvert { get; set; }
    }
}
