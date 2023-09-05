using Epidi.Models.Paging;
using Epidi.Models.ViewModel.Common;
using Epidi.Models.ViewModel.InstrumentMaster;
using Epidi.Models.ViewModel.RuleInstrument;
using Epidi.Repository.RuleInstrumentRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Service.RuleInstrumentService
{
    public class RuleInstrumentService : IRuleInstrumentService
    {
        private readonly IRuleInstrumentRepository _ruleInstrumentRepository;

        public RuleInstrumentService(IRuleInstrumentRepository ruleInstrumentRepository)
        {
            _ruleInstrumentRepository = ruleInstrumentRepository;
        }

        public List<ResponseViewModel> Delete(int Id)
        {
            return _ruleInstrumentRepository.Delete(Id);
        }

        public List<ResponseViewModel> DeleteByRuleId(int RuleId)
        {
            return _ruleInstrumentRepository.DeleteByRuleId(RuleId);
        }

        public Tuple<List<RuleInstrumentViewModel>, long> GetAll(PageParam pageParam, string search)
        {
            return _ruleInstrumentRepository.GetAll(pageParam, search);
        }

        public Task<RuleInstrumentViewModel> GetById(int Id)
        {
            return _ruleInstrumentRepository.GetById(Id);
        }

        public async Task<List<RuleInstrumentViewModel>> GetByRuleId(int RuleId)
        {
            return await _ruleInstrumentRepository.GetByRuleId(RuleId);
        }

        public async Task<RuleInstrumentViewModel> Upsert(RuleInstrumentViewModel model)
        {
            return await _ruleInstrumentRepository.Upsert(model);
        }
        public async Task<Tuple<List<RuleInstrumentLPViewModel>, long>> GetByInstrumentLPRuleByRuleId(PageParam pageParam, string search, int RuleId, int lp_filter = 0, int instrument_filter = 0, int day_filter=0)
        {
            return await _ruleInstrumentRepository.GetByInstrumentLPRuleByRuleId(pageParam, search, RuleId, lp_filter, instrument_filter, day_filter);
        }
        public async Task<InstrumentMasterViewModel> Get_InstrumentByRuleId(int RuleId)
        {
            return await _ruleInstrumentRepository.Get_InstrumentByRuleId(RuleId);
        }

        public async Task<List<RuleInstrumentLPViewModelExport>> GetExPortQuoteMarkUpRule_Data(int RuleId)
        {
            return await _ruleInstrumentRepository.GetExPortQuoteMarkUpRule_Data(RuleId);
        }




    }
}
