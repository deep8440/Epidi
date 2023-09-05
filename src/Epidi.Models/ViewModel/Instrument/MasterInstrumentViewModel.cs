using Epidi.Models.ViewModel.LeverageRules;
using Epidi.Models.ViewModel.SwapRule;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Models.ViewModel.Instrument
{
    public class MasterInstrumentViewModel
    {
        public int Id { get; set; }
        public string InstrumentName { get; set; }
        public string SymbolGroup { get; set; }
        public int ZerosTobeGrouped { get; set; }
        public string QTFrom { get; set; }
        public string QTTo { get; set; }
        public string Day { get; set; }
        public string TTFrom { get; set; }
        public string TTTo { get; set; }
        public string SymbolDenomination { get; set; }
        public string TradeStatus { get; set; }
        public decimal AverageSpread { get; set; }
        public decimal Decimals { get; set; }
        public string UnitDescription { get; set; }
        public int TradeId { get; set; }
        public string Path { get; set; }
        public byte[] Image { get; set; }
        public List<TradeTiming> tradeTimings { get; set; }
    }
    public class TradeTiming
    {
        public string QTFrom { get; set; }
        public string QTTo { get; set; }
        public string Day { get; set; }
        public string TTFrom { get; set; }
        public string TTTo { get; set; }
    }

    public class EditSpecificationInstrumentViewModel
    {
        public int InstrumentId { get; set; }
        public string InstrumentName { get; set; }  
        public string SymbolGroup { get; set; }
        public int SymbolGroupId { get; set; }
        public string TradeStatus { get; set; }
        public decimal AverageSpread { get; set; }
        public decimal Decimals { get; set; }
        public string SymbolDenomination { get; set; }
        public string Day { get; set; }
        public string Path { get; set; }

        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy hh:mm:ss tt}")]
        public TimeSpan QTFrom { get; set; }

        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy hh:mm:ss tt}")]
        public TimeSpan QTTo { get; set; }

        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy hh:mm:ss tt}")]
        public TimeSpan TTFrom { get; set; }

        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy hh:mm:ss tt}")]
        public TimeSpan TTTo { get; set; }
        public int TradeId { get; set; }
        public string UnitDescription { get; set; }
        public int ZerosToBeGrouped { get; set; }
        public string Logo { get; set; }

    }

    public class SpecificationRuleViewModel
    {
        public int Id { get; set; }
        public string Comment { get; set; }
        public int Priority { get; set; }
        public string objRuleInstrumentViewModelstr { get; set; }
        public string objRuleConditionViewModelstr { get; set; }
        public List<UpdateSpecificationRuleInstrumentViewModel> objRuleInstrumentViewModel { get; set; }
        public List<LeverageRulesDtlViewModel> objRuleConditionViewModel { get; set; }
    }

    public class UpdateSpecificationRuleInstrumentViewModel
    {
        public int MasterInstrumentId { get; set; }
        public int SymbolGroupId { get; set; }
        public int TradeId { get; set; }
        public TimeSpan QTFrom { get; set; }
        public TimeSpan QTTo { get; set; }
        public TimeSpan TTFrom { get; set; }
        public TimeSpan TTTo { get; set; }
        public int Day { get; set; }
        public string SymbolDenomination { get; set; }
        public string TradeStatus { get; set; }
        public decimal AverageSpread { get; set; }
        public decimal Decimals { get; set; }
        public string UnitDescription { get; set; }
        public int ZerosToBeGrouped { get; set; }
        public string Logo { get; set; }
    }
}
