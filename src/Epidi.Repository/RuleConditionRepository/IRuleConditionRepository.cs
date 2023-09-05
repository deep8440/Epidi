using Epidi.Models.Paging;
using Epidi.Models.ViewModel.Common;
using Epidi.Models.ViewModel.RuleCondition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Repository.RuleConditionRepository
{
    public interface IRuleConditionRepository
    {
        Task<RuleConditionViewModel> Upsert(RuleConditionViewModel model,bool Skip=false);
        Tuple<List<RuleConditionViewModel>, long> GetAll(PageParam pageParam, string search);
        Task<RuleConditionViewModel> GetById(int Id);
        Task<RuleConditionViewModel> GetByRuleId(int Id);
        Task<RuleConditionViewModel> GetByMarginRuleId(int Id);
        List<ResponseViewModel> Delete(int Id);
        Task<bool> DeleteConditionRulesDtlById(int Id, int RuleConditionId);
        Task<RuleConditionViewModel> GetRuleConditionBySwapRuleId(int Id);
        Task<bool> DeleteSwapRuleConditionDtlById(int RuleConditionId, int RuleConditionDtlId);
        Task<RuleConditionViewModel> GetRuleConditionByCommissionRuleId(int Id);
        Task<RuleConditionViewModel> GetRuleConditionBySpecificationRuleId(int Id);
        Task<RuleConditionViewModel> GetRuleConditionByTradeRuleId(int Id);
        Task<bool> DeleteConditionRulesDtlByTraedRuleId(int Id, int RuleConditionId);
        Task<bool> DeleteMarginRuleDtlById(int RuleConditionId, int RuleConditionDtlId);
        Task<int> DeleteQuotemarkUpRuleInstrument(int Id, int RuleId, int tLP_ID);
    }
}
