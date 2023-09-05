using Epidi.Models.Paging;
using Epidi.Models.ViewModel.Common;
using Epidi.Models.ViewModel.Instrument;
using Epidi.Models.ViewModel.TradeRule;
using Epidi.Repository.IBRepository;
using Epidi.Repository.TradeRuleUniversalValuesRepository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Service.TradeRuleUniversalValuesService
{
    public class TradeRuleUniversalValuesService : ITradeRuleUniversalValuesService
    {
        private readonly ITradeRuleUniversalValuesRepository _tradeRuleUniversalValuesRepository;
        public TradeRuleUniversalValuesService(ITradeRuleUniversalValuesRepository tradeRuleUniversalValuesRepository)
        {
            _tradeRuleUniversalValuesRepository = tradeRuleUniversalValuesRepository;
        }

        public Tuple<List<TradeRuleUniversalValuesViewModel>, long> GetAll_TradeRuleUniversalValues(PageParam pageParam, string search)
        {
            return _tradeRuleUniversalValuesRepository.GetAll_TradeRuleUniversalValues(pageParam, search);
        }

        public Task<TradeRuleUniversalValuesViewModel> GetTradeRuleUniversalValues_ById(int Id)
        {
            return _tradeRuleUniversalValuesRepository.GetTradeRuleUniversalValues_ById(Id);
        }
        public int SaveTradeRuleUniversalValues(DataTable dataTable)
        {
            return _tradeRuleUniversalValuesRepository.SaveTradeRuleUniversalValues(dataTable);
        }

        public List<ResponseViewModel> TradeRuleUniversalValuesDelete(int Id)
        {
            return _tradeRuleUniversalValuesRepository.TradeRuleUniversalValuesDelete(Id);
        }
        public Task<List<UniversalParametersViewModel>> GetUniversalParameters()
        {
            return _tradeRuleUniversalValuesRepository.GetUniversalParameters();
        }

        public Task<List<ActionsDefintiionsViewModel>> GetActionsDefintiions()
        {
            return _tradeRuleUniversalValuesRepository.GetActionsDefintiions();
        }

		public Task<List<WhenToCheckViewModel>> GetWhenToCheckList()
		{
			return _tradeRuleUniversalValuesRepository.GetWhenToCheckList();
		}

		public Task<List<FunctionsViewModel>> GetFunctionsList()
		{
			return _tradeRuleUniversalValuesRepository.GetFunctionsList();
		}

		public Task<List<TablesViewModel>> GetTablesList()
		{
			return _tradeRuleUniversalValuesRepository.GetTablesList();
		}

		public async Task<List<TradeRuleUniversalValuesViewModel>> ExportTradeRuleUniversalValues()
        {
            return await _tradeRuleUniversalValuesRepository.ExportTradeRuleUniversalValues();
        }

    }
}
