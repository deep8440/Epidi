using Epidi.Models.ViewModel.Instrument;
using Epidi.Models.ViewModel.LegalEntity;
using Epidi.Models.ViewModel.SymbolGroup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Models.Model
{
    public class LeverageRuleImport:CommonField
    {
        public int Id { get; set; }
        public int LRuleId { get; set; }
        public string LRuleName { get; set; }
        public int LRulePriority { get; set; }

        public string LComment { get; set; }
        public int LeverageId { get; set; }
        public int LeverageRuleId { get; set; }
        public string LeverageName { get; set; }
        public int MasterInstrumentId { get; set; }
        public string MasterInstrumentName { get; set; }
        public int SymbolGroupId { get; set; }
        public string SymbolGroupName { get; set; }
        public Decimal Day { get; set; }
        public int Priority { get; set; }
        public int Priority1 { get; set;}
        public int Priority2 { get; set; }
        public string LegalEntity { get; set; }
        public string TimeFrom { get; set; }
        public string TimeTo { get; set; }
        public string IsNewPosition { get; set; }
        public decimal BandFrom { get; set; }
        public decimal BandTo { get; set; }
        public int LegalEntityId { get; set; }
        public decimal LeverageAmount { get; set; }
        public string RuleConditionStr { get; set; }

        public int LeverageBandId { get; set; }
        public List<LevarageBand> levarageBand { get; set; }
        public int LeverageBandMappingId { get; set; }
        public List<InstrumentMaster> instrumentMasters { get; set; }
        public List<SymbolGroupViewModel> symbolGroups { get; set; }
        public List<LegalEntityViewModel> legalEntities { get; set; }
        
    }
    public class LevarageBand
    {
        public int LeverageId { get; set; }
        public decimal BandFrom { get; set; }
        public decimal BandTo { get; set; }
        public decimal LeverageAmount { get; set; }
    }
}
