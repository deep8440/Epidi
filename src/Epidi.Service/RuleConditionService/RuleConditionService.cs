using Epidi.Models.Paging;
using Epidi.Models.ViewModel.Common;
using Epidi.Models.ViewModel.RuleCondition;
using Epidi.Repository.RuleConditionRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Service.RuleConditionService
{
    public class RuleConditionService : IRuleConditionService
    {
        private readonly IRuleConditionRepository _ruleConditionRepository;
        public RuleConditionService(IRuleConditionRepository ruleConditionRepository)
        {
            _ruleConditionRepository = ruleConditionRepository;
        }

        public List<ResponseViewModel> Delete(int Id)
        {
            return _ruleConditionRepository.Delete(Id);
        }

        public Tuple<List<RuleConditionViewModel>, long> GetAll(PageParam pageParam, string search)
        {
            return _ruleConditionRepository.GetAll(pageParam, search);
        }

        public async Task<RuleConditionViewModel> GetById(int Id)
        {
            return await _ruleConditionRepository.GetById(Id);
        }
        public async Task<RuleConditionViewModel> GetByRuleId(int Id)
        {
            return await _ruleConditionRepository.GetByRuleId(Id);
        }
        public async Task<RuleConditionViewModel> GetRuleConditionByTradeRuleId(int Id)
        {
            return await _ruleConditionRepository.GetRuleConditionByTradeRuleId(Id);
        }
        public async Task<RuleConditionViewModel> GetRuleConditionBySwapRuleId(int Id)
        {
            return await _ruleConditionRepository.GetRuleConditionBySwapRuleId(Id);
        }
        public async Task<RuleConditionViewModel> GetRuleConditionBySpecificationRuleId(int Id)
        {
            return await _ruleConditionRepository.GetRuleConditionBySpecificationRuleId(Id);
        }
        public async Task<RuleConditionViewModel> GetRuleConditionByCommissionRuleId(int Id)
        {
            return await _ruleConditionRepository.GetRuleConditionByCommissionRuleId(Id);
        }
        public async Task<RuleConditionViewModel> GetByMarginRuleId(int Id)
        {
            return await _ruleConditionRepository.GetByMarginRuleId(Id);
        }
        public async Task<RuleConditionViewModel> Upsert(RuleConditionViewModel model)
        {
            return await _ruleConditionRepository.Upsert(model);
        }
        public async Task<bool> DeleteConditionRulesDtlById(int Id, int RuleConditionId)
        {
            return await _ruleConditionRepository.DeleteConditionRulesDtlById(Id, RuleConditionId);
        }
        public async Task<bool> DeleteConditionRulesDtlByTraedRuleId(int Id, int RuleConditionId)
        {
            return await _ruleConditionRepository.DeleteConditionRulesDtlByTraedRuleId(Id, RuleConditionId);
        }
        public async Task<bool> DeleteSwapRuleConditionDtlById(int RuleConditionId,int RuleConditionDtlId)
        {
            return await _ruleConditionRepository.DeleteSwapRuleConditionDtlById(RuleConditionId,RuleConditionDtlId);
        }
        public async Task<bool> DeleteMarginRuleDtlById(int Id, int RuleConditionId)
        {
            return await _ruleConditionRepository.DeleteMarginRuleDtlById(Id, RuleConditionId);
        }
        public async Task<int> DeleteQuotemarkUpRuleInstrument(int Id, int RuleId, int tLP_ID)
        {
            return await _ruleConditionRepository.DeleteQuotemarkUpRuleInstrument(Id, RuleId, tLP_ID);
        }
    }
}
