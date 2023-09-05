using Epidi.Models.ViewModel.RuleCondition;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Models.ViewModel.Instrument
{
    public class InstrumentSpecificationRule
    {
        public int Id { get; set; }
        public string Comment { get; set; }
        public int Priority { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsActive { get; set; }
        public RuleConditionViewModel objRuleConditionViewModel { get; set; }
    }
    public class InstrumentMaster
    {
        public int id { get; set; }
        public string name { get; set; }
        public string Code { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }
        [NotMapped]
        public string GroupName { get; set; }
        public bool? isFavourite { get; set; }
    }
    public class FavouriteInstrument
    {
        public int id { get; set; }
        public int Userid { get; set; }
        public int master_id { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }
    }

    public class GateIO
    {
        public int id { get; set; }
        public string Name { get; set; }
    }
    public class LMax
    {
        public int id { get; set; }
        public string symbol { get; set; }
    }
    public class InstrumentMasterMapping
    {
        public int id { get; set; }
        public int master_id { get; set; }
        public int GateIO { get; set; }
        public int Lmax { get; set; }
    }

    public class InstrumentMasterMappingViewModel
    {
        public int id { get; set; }
        public int master_id { get; set; }
        public string  lpName { get; set; }
        public int instrumentId { get; set; }
    }

    public class CommissionRuleInstrument
    {
        public int Id { get; set; }
        public string NAME { get; set; }
        public decimal BuyFeePer1000Notional { get; set; }
        public decimal SellFeePerNotional1000 { get; set; }
        public decimal BuyFeeNotional { get; set; }
        public decimal SellFeeNotional { get; set; }
        public decimal FeeBuyPerUnits { get; set; }
        public decimal FeeSellUnits { get; set; }
        public string BuySwapPer1000Notional { get; set; }
        public string SellSwapPerNotional1000 { get; set; }
        public string BuySwapNotional { get; set; }
        public string SellSwapNotional { get; set; }
    }

    public class CalculateCommissionValueByInstrument
    {
        public int InstrumentId { get; set; }
        public decimal Value { get; set; }
        public string Type { get; set; }
    }
}
