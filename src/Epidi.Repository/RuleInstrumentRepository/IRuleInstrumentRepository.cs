using Epidi.Models.Paging;
using Epidi.Models.ViewModel.Common;
using Epidi.Models.ViewModel.InstrumentMaster;
using Epidi.Models.ViewModel.RuleInstrument;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Repository.RuleInstrumentRepository
{
    public interface IRuleInstrumentRepository
    {
        Task<RuleInstrumentViewModel> Upsert(RuleInstrumentViewModel model);
        Tuple<List<RuleInstrumentViewModel>, long> GetAll(PageParam pageParam, string search);
        Task<RuleInstrumentViewModel> GetById(int Id);
        List<ResponseViewModel> Delete(int Id);
        Task<List<RuleInstrumentViewModel>> GetByRuleId(int RuleId);
        List<ResponseViewModel> DeleteByRuleId(int RuleId);
        Task<Tuple<List<RuleInstrumentLPViewModel>, long>> GetByInstrumentLPRuleByRuleId(PageParam pageParam, string search, int RuleId, int lp_filter=0, int instrument_filter=0,int day_filter=0);
        Task<InstrumentMasterViewModel> Get_InstrumentByRuleId(int RuleId);
        Task<List<RuleInstrumentLPViewModelExport>> GetExPortQuoteMarkUpRule_Data(int RuleId);



    }
}
