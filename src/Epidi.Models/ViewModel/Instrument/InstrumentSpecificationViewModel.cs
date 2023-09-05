using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Models.ViewModel.Instrument
{
    public class InstrumentSpecificationViewModel
    {
        public int Id { get; set; }
        public int LPId { get; set; }
        public int MasterInstrumentId { get; set; }
        public int LPInstrumentId { get; set; }
        public int TradeId { get; set; }
        public string MasterInstrumentName { get; set; }
        public string LPName { get; set; }
        public string LPInstrumentName { get; set; }
        public bool IsActive { get; set; }  

        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy hh:mm:ss tt}")]
        public TimeSpan TTFrom { get; set; }

        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy hh:mm:ss tt}")]
        public TimeSpan TTTo { get; set; }
        public string TradeDays { get; set; }

        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy hh:mm:ss tt}")]
        public TimeSpan QTFrom { get; set; }

        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy hh:mm:ss tt}")]
        public TimeSpan QTTo { get; set; }
        public string QuoteDays { get; set; }
        public long MinValue { get; set; }
        public decimal Multiplier { get; set; }
        public string  OnOff { get; set; }
        public decimal ContractSize { get; set; }
        public string ISIN { get; set; }
    }
    public class EditInstrumentSpecificationViewModel
    {
        public int LPId { get; set; }
        public int MasterInstrumentId { get; set; }
        public int LPInstrumentId { get; set; }
        public int TradeId { get; set; }
    }
}
