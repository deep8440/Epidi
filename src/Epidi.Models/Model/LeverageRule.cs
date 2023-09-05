using Epidi.Models.ViewModel.LeverageRules;
using Epidi.Models.ViewModel.RuleCondition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Models.Model
{
    public class LeverageRule : CommonField
    {
        public int RuleId { get; set; }
        public string RuleName { get; set; }
        public int Priority { get; set; }
        public string Comment { get; set; }
        public string objleverageRuleImportstr { get; set; }
        public string objRuleconditionsstr { get; set; }
        public List<LeverageRuleImport> objleverageRuleImport { get; set; }
        public List<LeverageRulesDtlViewModel> objRuleconditions { get; set; }
    }
}
