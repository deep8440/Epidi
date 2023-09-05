using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Models.ViewModel.SkipRule
{
    public class SkipRuleModelView
    {
        public int id { get; set; }
        public int MasterInstrumentId { get; set; }
        public string  MasterInstrumentIds { get; set; }
        public string Name { get; set; }
        public string Equation { get; set; }
        public bool IsSkip { get; set; }
        public float Value { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime DeletedAt { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        [NotMapped]
        public string MasterInstrumentName { get; set; }
    }
    public class SkipRuleImportModelView
    {
        public string Name { get; set; }
        public string MasterInstrumentName { get; set; }
        public string SymbolGroups { get; set; }
        public string Equation { get; set; }
        public bool IsSkip { get; set; }
        public float Value { get; set; }
        
    }
    
}
