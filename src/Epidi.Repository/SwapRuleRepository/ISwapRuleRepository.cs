using Epidi.Models.Model;
using Epidi.Models.Paging;
using Epidi.Models.ViewModel.Common;
using Epidi.Models.ViewModel.SwapRule;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Repository.SwapRuleRepository
{
    public interface ISwapRuleRepository
    {
        Task<Tuple<List<SwapRuleViewModel>, long>> GetAllSwapRules(PageParam pageParam, string search);
        Task<long> AddSwapRule(DataTable dataTable, SwapRuleViewModel model);
        Task<SwapRuleViewModel> GetSwapRuleById(int Id);
        Task<Tuple<List<SwapRuleInstrumentViewModel>,long>> GetSwapRuleInstrumentById(PageParam pageParam, string search, int Id);
        Task<long> UpdateSwapRuleInstrument(DataTable dataTable, SwapRuleViewModel model);
        Task<int> DeleteSwapRuleInstrumentById( int Id ,int SwapRuleId);
        Task<int> DeleteSwapRuleById(int SwapRuleId);
        Task<ResponseViewModel> CopyRule(int Id);

        Task<long> AddSwapRuleNew(DataTable dataTable, SwapRuleViewModel model);

    }
}
