using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Models.ViewModel.Margin
{
    public class MarginViewModel
    {
        public int MarginId { get; set; }
        public int MarginTypeId { get; set; }
        public string InstrumentEnd { get; set; }
        public int MasterInstrumentId { get; set; }
        public int SymbolGroupId { get; set; }
        public string SymbolCurrencyFrom { get; set; }
        public string SymbolCurrencyTo { get; set;}
        public string MarginTypeName { get; set; }
        public string MasterInstrumentName { get; set;}
        public string SymbolGroupName { get; set;}
        public string InstrumentConversiontoUSD { get; set; }
        public string InstrumentConversiontoEUR { get; set; }
        public string InstrumentConversiontoINR { get; set; }
        public string ManualInstrumentConversiontoUSD { get; set; }
        public string ManualInstrumentConversiontoEUR { get; set; }
        public string ManualInstrumentConversiontoINR { get; set; }
        public string InstrumentToConvert { get; set; }
        public string ConversionInstrumentName { get; set; }
        public decimal ManualConversion { get; set; }
        public string conversionDataStr { get; set; }
        public List<MarginDetails> marginDetailsList { get; set; }
    }
    public class DropDownItems
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class MarginDetails
    {
        public int Id { get; set; }
        public int MarginId { get; set; }
        public int InstrumentId { get; set; }
        public double ManualConversion { get; set; }
        public int InstrumentToConvertId { get; set; }
    }
}
