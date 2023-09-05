using Epidi.Models.Model;
using Epidi.Models.Paging;
using Epidi.Models.ViewModel.Common;
using Epidi.Models.ViewModel.LeverageRule;
using Epidi.Models.ViewModel.LeverageRules;
using Epidi.Models.ViewModel.RuleCondition;
using Epidi.Repository.LeverageRulesRepository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Service.LeverageRulesService
{
    public class LeverageRulesService : ILeverageRulesService
    {
        private readonly ILeverageRulesRepository _leverageRulesRepository;

        public LeverageRulesService(ILeverageRulesRepository leverageRulesRepository)
        {
            _leverageRulesRepository = leverageRulesRepository;
        }

        public Tuple<List<LeverageRulesViewModel>, long> GetAll(PageParam pageParam, string search)
        {
            return _leverageRulesRepository.GetAll(pageParam, search);
        }

        public async Task<LeverageRulesViewModel> GetById(int Id)
        {
            return await _leverageRulesRepository.GetById(Id);
        }
        public ResponseViewModel ImportLevarageRule(LeverageRuleImport leverageRuleImport)
        {
            return _leverageRulesRepository.ImportLevarageRule(leverageRuleImport);
        }
		public Task<long> ImportLevarageRule(DataTable dataTable)
		{
			return _leverageRulesRepository.ImportLevarageRule(dataTable);
		}
		public List<LeverageRuleImport> GetImportLeverageRules(int LeverageRuleId)
        {
            return _leverageRulesRepository.GetImportLeverageRules(LeverageRuleId);
        }
        public async Task<LevarageRuleImportViewModel> Edit(int id)
        {
            return await _leverageRulesRepository.Edit(id);
        }



        public List<ResponseViewModel> RemoveRule(int Id)
        {
            return _leverageRulesRepository.RemoveRule(Id);
        }
    
        public ResponseViewModel ImportLevarageBand(List<Models.Model.LevarageBand> levarageBand)
        {
            return _leverageRulesRepository.ImportLevarageBand(levarageBand);
        }
        public ResponseViewModel InsertLeverageRule(LeverageRuleImport leverageRuleImport)
        {
            return _leverageRulesRepository.InsertLeverageRule(leverageRuleImport);
        }
        public List<ResponseViewModel> Upsert(LeverageRulesViewModel model)
        {
            return _leverageRulesRepository.Upsert(model);
        }
        
        public async Task<List<RuleConditionViewModel_Dt>> GetRuleConditionsByRuleId(int id)
        {
            return await _leverageRulesRepository.GetRuleConditionsByRuleId(id);
        }
        
        public ResponseViewModel UpdateBandMapping(LeverageRuleImport leverageRule)
        {
            return _leverageRulesRepository.UpdateBandMapping(leverageRule);
        }

        public ResponseViewModel UpdateLeverage(LeverageRuleImport leverageRule)
        {
            return _leverageRulesRepository.UpdateLeverage(leverageRule);
        }
        public Tuple<List<LeverageRuleImport>, long> GetLeverageRuleImports(PageParam pageParam, int id, string search, int name)
        {
            return _leverageRulesRepository.GetLeverageRuleImports(pageParam,id, search,name);
        }
        public async Task<LeverageRuleImport> GetLeverageRuleData(int id)
        {
            return await _leverageRulesRepository.GetLeverageRuleData(id);
        }
        public Tuple<List<LeverageRuleImport>, long> GetAllLeverageRules(PageParam pageParam, string search)
        {
            return _leverageRulesRepository.GetAllLeverageRules(pageParam, search);
        }
        public async Task<bool> DeleteRuleConditionDtlById(int Id, int RuleConditionId)
        {
            return await _leverageRulesRepository.DeleteRuleConditionDtlById(Id, RuleConditionId);
        }
    }
}
