using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Models.ViewModel.LeverageRules
{
    public class LevarageRuleImportViewModel
    {
        public int LeverageId { get; set; }
        public int MasterInstrumentId { get; set; }
        public Decimal Day { get; set; }
        public string TimeFrom { get; set; }
        public string TimeTo { get; set; }
        public string IsNewPosition { get; set; }
        public List<LevarageBand> ObjLeverageBand { get; set; }
        public List<LevarageBandMapping> ObjLeverageBandMapping { get; set; }
    }
    public class LevarageBand
    {
        public int LeverageBandId { get; set; }        
        public decimal BandFrom { get; set; }
        public decimal BandTo { get; set; }
    }
    public class LevarageBandMapping
    {
        public int LeverageBandMappingId { get; set; }
        public int LeverageId { get; set; }
        public int LeverageBandId { get; set; }
        public decimal LeverageAmount { get; set; }
    }
}
