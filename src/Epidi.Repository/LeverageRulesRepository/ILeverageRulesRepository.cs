using Epidi.Models.Model;
using Epidi.Models.Paging;
using Epidi.Models.ViewModel.Common;
using Epidi.Models.ViewModel.LeverageRule;
using Epidi.Models.ViewModel.LeverageRules;
using Epidi.Models.ViewModel.RuleCondition;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Repository.LeverageRulesRepository
{
    public interface ILeverageRulesRepository
    {
        List<ResponseViewModel> Upsert(LeverageRulesViewModel model);
        Task<List<RuleConditionViewModel_Dt>> GetRuleConditionsByRuleId(int id);
        Tuple<List<LeverageRulesViewModel>, long> GetAll(PageParam pageParam, string search);
        Tuple<List<LeverageRuleImport>, long> GetLeverageRuleImports(PageParam pageParam, int id, string search, int name);
        ResponseViewModel UpdateBandMapping(LeverageRuleImport leverageRule);
        ResponseViewModel UpdateLeverage(LeverageRuleImport leverageRule);
        Task<LeverageRulesViewModel> GetById(int Id);
        ResponseViewModel InsertLeverageRule(LeverageRuleImport leverageRuleImport);
        ResponseViewModel ImportLevarageRule(LeverageRuleImport leverageRuleImport);
		Task<long> ImportLevarageRule(DataTable dataTable);
		List<LeverageRuleImport> GetImportLeverageRules(int LeverageRuleId);
        Task<LevarageRuleImportViewModel> Edit(int id);

        List<ResponseViewModel> RemoveRule(int leverageId);
     
        ResponseViewModel ImportLevarageBand(List<Models.Model.LevarageBand> levarageBand);
        Tuple<List<LeverageRuleImport>, long> GetAllLeverageRules(PageParam pageParam, string search);
        Task<bool> DeleteRuleConditionDtlById(int Id, int RuleConditionId);

        Task<LeverageRuleImport> GetLeverageRuleData(int id);
    }
}
