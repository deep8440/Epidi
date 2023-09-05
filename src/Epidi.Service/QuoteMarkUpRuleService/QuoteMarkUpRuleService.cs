using Epidi.Models.Paging;
using Epidi.Models.ViewModel.Common;
using Epidi.Models.ViewModel.QuoteMarkUpRule;
using Epidi.Repository.QuoteMarkUpRuleRepository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Service.QuoteMarkUpRuleService
{
    public class QuoteMarkUpRuleService : IQuoteMarkUpRuleService
    {
        private readonly IQuoteMarkUpRuleRepository _IQuoteMarkUpRuleRepository;

        public QuoteMarkUpRuleService(IQuoteMarkUpRuleRepository iQuoteMarkUpRuleRepository)
        {
            _IQuoteMarkUpRuleRepository = iQuoteMarkUpRuleRepository;
        }

        public List<ResponseViewModel> Delete(int Id)
        {
            return _IQuoteMarkUpRuleRepository.Delete(Id);
        }
        public async Task<ResponseViewModel> CopyRule(int Id)
        {
            return await _IQuoteMarkUpRuleRepository.CopyRule(Id);
        }

        public Tuple<List<QuoteMarkUpRuleViewModel>, long> GetAll(PageParam pageParam, string search)
        {
            return _IQuoteMarkUpRuleRepository.GetAll(pageParam, search);
        }

        public Task<QuoteMarkUpRuleViewModel> GetById(int Id)
        {
            return _IQuoteMarkUpRuleRepository.GetById(Id);
        }

        public Task<QuoteMarkUpRuleViewModel> Upsert(QuoteMarkUpRuleViewModel model)
        {
            return _IQuoteMarkUpRuleRepository.Upsert(model);
        }

        public async Task<long> QuoteMarkUpRule_InsertByImport(DataTable dataTable, int RuleId)
        {
            return await _IQuoteMarkUpRuleRepository.QuoteMarkUpRule_InsertByImport(dataTable,RuleId);
        }

        public async Task<long> QuoteMarkUpRule_UpdateByDt(DataTable dtRuleInstrument, DataTable dtRuleLpPriority, DataTable dtRuleConditionsDtl)
        {
            return await _IQuoteMarkUpRuleRepository.QuoteMarkUpRule_UpdateByDt(dtRuleInstrument, dtRuleLpPriority, dtRuleConditionsDtl);
        }
    }
}
