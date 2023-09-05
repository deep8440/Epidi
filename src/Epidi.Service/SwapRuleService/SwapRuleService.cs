using Epidi.Models.Model;
using Epidi.Models.Paging;
using Epidi.Models.ViewModel.Common;
using Epidi.Models.ViewModel.SwapRule;
using Epidi.Repository.SwapRuleRepository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Service.SwapRuleService
{
    public class SwapRuleService : ISwapRuleService
    {
        private readonly ISwapRuleRepository _swapRuleRepository;
        public SwapRuleService(ISwapRuleRepository swapRuleRepository)
        {
            _swapRuleRepository = swapRuleRepository;
        }
        public async Task<long> AddSwapRule(DataTable dataTable, SwapRuleViewModel model)
        {
            return await _swapRuleRepository.AddSwapRule(dataTable,model);
        }
        public async Task<long> UpdateSwapRuleInstrument(DataTable dataTable, SwapRuleViewModel model)
        {
            return await _swapRuleRepository.UpdateSwapRuleInstrument(dataTable, model);
        }

        public async Task<Tuple<List<SwapRuleViewModel>, long>> GetAllSwapRules(PageParam pageParam, string search)
        {
            return await _swapRuleRepository.GetAllSwapRules(pageParam,search);
        }
        public async Task<SwapRuleViewModel> GetSwapRuleById(int Id)
        {
            return await _swapRuleRepository.GetSwapRuleById(Id);
        }
        public async Task <Tuple<List<SwapRuleInstrumentViewModel>,long>> GetSwapRuleInstrumentById(PageParam pageParam, string search, int Id)
        {
            return await _swapRuleRepository.GetSwapRuleInstrumentById(pageParam, search, Id);
        }
        public async Task<int> DeleteSwapRuleInstrumentById(int Id,int SwapRuleId)
        {
            return await _swapRuleRepository.DeleteSwapRuleInstrumentById(Id, SwapRuleId);
        }
        public async Task<int> DeleteSwapRuleById(int SwapRuleId)
        {
            return await _swapRuleRepository.DeleteSwapRuleById(SwapRuleId);
        }
        public async Task<ResponseViewModel> CopyRule(int Id)
        {
            return await _swapRuleRepository.CopyRule(Id);
        }

        public async Task<long> AddSwapRuleNew(DataTable dataTable, SwapRuleViewModel model)
        {
            return await _swapRuleRepository.AddSwapRuleNew(dataTable, model);
        }


    }
}
