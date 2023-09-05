using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Models.ViewModel.Instrument
{
    public class InstrumentMappingViewModel
    {
        public int Id { get; set; }
        public string EpidiInstrumentName { get; set; }
        public string LP { get; set; }
        public string LPInstrumentName { get; set; }
        public string QTFrom { get; set; }
        public string QTTo { get; set; }
        public string Days { get; set; }
        public string TTFrom { get; set; }
        public string TTTo { get; set; }
        public long MinValue { get; set; }
        public decimal Multiplier { get; set; }
        public string OnOff { get; set; }
        public decimal ContractSize { get; set; }
        public string ISIN { get; set; }
    }
}
