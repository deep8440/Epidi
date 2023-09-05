using Epidi.Models.Paging;
using Epidi.Models.ViewModel.BDM;
using Epidi.Models.ViewModel.Common;
using Epidi.Models.ViewModel.Question;
using Epidi.Models.ViewModel.TradeRule;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Repository.TradeRuleUniversalValuesRepository
{
    public interface ITradeRuleUniversalValuesRepository
    {
        Tuple<List<TradeRuleUniversalValuesViewModel>, long> GetAll_TradeRuleUniversalValues(PageParam pageParam, string search);
        Task<TradeRuleUniversalValuesViewModel> GetTradeRuleUniversalValues_ById(int Id);
        List<ResponseViewModel> TradeRuleUniversalValuesDelete(int Id);
        int SaveTradeRuleUniversalValues(DataTable dataTable);
        Task<List<UniversalParametersViewModel>> GetUniversalParameters();
        Task<List<ActionsDefintiionsViewModel>> GetActionsDefintiions();
		Task<List<WhenToCheckViewModel>> GetWhenToCheckList();
		Task<List<FunctionsViewModel>> GetFunctionsList();
		Task<List<TablesViewModel>> GetTablesList();
		Task<List<TradeRuleUniversalValuesViewModel>> ExportTradeRuleUniversalValues();
    }
}
