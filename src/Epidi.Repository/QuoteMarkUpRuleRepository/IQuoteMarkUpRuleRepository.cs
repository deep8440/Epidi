using Epidi.Models.Paging;
using Epidi.Models.ViewModel.Common;
using Epidi.Models.ViewModel.QuoteMarkUpRule;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Repository.QuoteMarkUpRuleRepository
{
    public interface IQuoteMarkUpRuleRepository
    {
        Task<QuoteMarkUpRuleViewModel> Upsert(QuoteMarkUpRuleViewModel model);
        Tuple<List<QuoteMarkUpRuleViewModel>, long> GetAll(PageParam pageParam, string search);
        Task<QuoteMarkUpRuleViewModel> GetById(int Id);
        List<ResponseViewModel> Delete(int Id);
        Task<ResponseViewModel> CopyRule(int Id);
        Task<long> QuoteMarkUpRule_InsertByImport(DataTable dataTable, int RuleId);
        Task<long> QuoteMarkUpRule_UpdateByDt(DataTable dtRuleInstrument, DataTable dtRuleLpPriority, DataTable dtRuleConditionsDtl);
    }
}
