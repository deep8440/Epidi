using Epidi.Models.Paging;
using Epidi.Models.ViewModel.CommissionRule;
using Epidi.Models.ViewModel.Common;
using Epidi.Models.ViewModel.TradeRule;
using Epidi.Repository.IBRepository;
using Epidi.Repository.TradeRuleRepository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Service.TradeRuleService
{
    public class TradeRuleService : ITradeRuleService
    {
        private readonly ITradeRuleRepository _tradeRuleRepository;
        public TradeRuleService(ITradeRuleRepository tradeRuleRepository)
        {
            _tradeRuleRepository = tradeRuleRepository;
        }
        public Tuple<List<TradeRuleViewModel>, long> GetAllTradeRule(PageParam pageParam, string search)
        {
            return _tradeRuleRepository.GetAllTradeRule(pageParam, search);
        }

        public Tuple<List<TradeStatusViewModel>, long> GetAllTradeStatus(PageParam pageParam, string search)
        {
            return _tradeRuleRepository.GetAllTradeStatus(pageParam, search);
        }

        public async Task<long> TradeRule_InsertByImport(DataTable dataTable, int tradeRuleId, string tradeRuleName, int tradeRulePriority)
        {
            return await _tradeRuleRepository.TradeRule_InsertByImport(dataTable,tradeRuleId,tradeRuleName,tradeRulePriority);
        }

        public async Task<TradeRuleViewModel> GetById(int Id)
        {
            return await _tradeRuleRepository.GetById(Id);
        }

        //public async Task<List<TradeRuleInstrumentViewModelGet>> GetByInstrumentTradeRuleByRuleId(int RuleId, int lp_filter = 0, int instrument_filter = 0, int day_filter = 0)
        //{
        //    return await _tradeRuleRepository.GetByInstrumentTradeRuleByRuleId(RuleId, lp_filter, instrument_filter, day_filter);
        //}

        public async Task<Tuple<List<TradeRuleInstrumentViewModelGet>, long>> GetByInstrumentTradeRuleByRuleId(PageParam pageParam, string search, int Id, int InstrumentId, int LpId, string day)
        {
            return await _tradeRuleRepository.GetByInstrumentTradeRuleByRuleId(pageParam, search, Id, InstrumentId,LpId,day);

        }

        public async Task<TradeRuleViewModel> Upsert(TradeRuleViewModel model)
        {
            return await _tradeRuleRepository.Upsert(model);
        }

        public async Task<long> TradeRule_UpdateByDt(DataTable dtRuleInstrument)
        {
            return await _tradeRuleRepository.TradeRule_UpdateByDt(dtRuleInstrument);
        }

        public async Task<List<TradeRuleUniversalValues>> Get_TradeUniversalValues()
        {
            return await _tradeRuleRepository.Get_TradeUniversalValues();
        }

        public async Task<long> DeleteTradeRuleInstrumentPriority(int RuleId)
        {
            return await _tradeRuleRepository.DeleteTradeRuleInstrumentPriority(RuleId);
        } 
        public async Task<bool> CheckTradeRuleUniversalValues(string Values)
        {
            return await _tradeRuleRepository.CheckTradeRuleUniversalValues(Values);
        }
        public async Task<ResponseViewModel> CopyTradeRule(int Id)
        {
            return await _tradeRuleRepository.CopyTradeRule(Id);
        }

        public async Task<int> DeleteTradeRuleInstrumentById(int RuleId, int RulePriorityId)
        {
            return await _tradeRuleRepository.DeleteTradeRuleInstrumentById(RuleId, RulePriorityId);
        }
        public List<ResponseViewModel> Delete(int Id)
        {
            return _tradeRuleRepository.Delete(Id);
        }

    }
}
