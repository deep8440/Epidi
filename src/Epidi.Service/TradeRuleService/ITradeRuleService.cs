using Epidi.Models.Paging;
using Epidi.Models.ViewModel.CommissionRule;
using Epidi.Models.ViewModel.Common;
using Epidi.Models.ViewModel.TradeRule;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Service.TradeRuleService
{
    public interface ITradeRuleService
    {
        Tuple<List<TradeStatusViewModel>, long> GetAllTradeStatus(PageParam pageParam, string search);
        Tuple<List<TradeRuleViewModel>, long> GetAllTradeRule(PageParam pageParam, string search);
        Task<long> TradeRule_InsertByImport(DataTable dataTable,int tradeRuleId,string tradeRuleName, int tradeRulePriority);
        Task<TradeRuleViewModel> GetById(int Id);
        //Task<List<TradeRuleInstrumentViewModelGet>> GetByInstrumentTradeRuleByRuleId(int RuleId, int lp_filter = 0,
        //    int instrument_filter = 0, int day_filter = 0);
        Task<Tuple<List<TradeRuleInstrumentViewModelGet>, long>> GetByInstrumentTradeRuleByRuleId(PageParam pageParam, string search, int Id, int InstrumentId, int LpId, string day);
        Task<TradeRuleViewModel> Upsert(TradeRuleViewModel model);
        Task<long> TradeRule_UpdateByDt(DataTable dtRuleInstrument);
        Task<List<TradeRuleUniversalValues>> Get_TradeUniversalValues();
        Task<long> DeleteTradeRuleInstrumentPriority(int RuleId);
        Task<bool> CheckTradeRuleUniversalValues(string Values);
        Task<ResponseViewModel> CopyTradeRule(int Id);

        Task<int> DeleteTradeRuleInstrumentById(int RuleId, int RulePriorityId);
        List<ResponseViewModel> Delete(int Id);
    }
}
