﻿using Epidi.Models.Model;
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

namespace Epidi.Service.LeverageRulesService
{
    public interface ILeverageRulesService
    {
        List<ResponseViewModel> Upsert(LeverageRulesViewModel model);
        Tuple<List<LeverageRulesViewModel>, long> GetAll(PageParam pageParam, string Id);
        Task<LeverageRulesViewModel> GetById(int Id);
        Tuple<List<LeverageRuleImport>, long> GetLeverageRuleImports(PageParam pageParam, int id, string search, int name);
        Task<List<RuleConditionViewModel_Dt>> GetRuleConditionsByRuleId(int id);
        ResponseViewModel InsertLeverageRule(LeverageRuleImport leverageRuleImport);
        ResponseViewModel UpdateLeverage(LeverageRuleImport leverageRule);
        ResponseViewModel UpdateBandMapping(LeverageRuleImport leverageRule);
        ResponseViewModel ImportLevarageRule(LeverageRuleImport leverageRuleImport);
        Task<long> ImportLevarageRule(DataTable dataTable);
		List<LeverageRuleImport> GetImportLeverageRules(int LeverageRuleId);
        ResponseViewModel ImportLevarageBand(List<Models.Model.LevarageBand> levarageBand);
        Task<LevarageRuleImportViewModel> Edit(int id);

        List<ResponseViewModel> RemoveRule(int Id);
       
        Tuple<List<LeverageRuleImport>, long> GetAllLeverageRules(PageParam pageParam, string search);
        Task<bool> DeleteRuleConditionDtlById(int Id, int RuleConditionId);
        Task<LeverageRuleImport> GetLeverageRuleData(int id);

    }
}
