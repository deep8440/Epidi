using Epidi.Models.Paging;
using Epidi.Models.ViewModel.CommissionRule;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Service.CommissionRuleService
{
    public interface ICommissionRuleService
    {
        Task<Tuple<List<CommissionRuleViewModel>, long>> GetAllCommissionRules(PageParam pageParam, string search);
        Task<long> AddCommissionRule(DataTable dataTable, CommissionRuleViewModel model);
        Task<long> SaveCommissionRuleWithoutFile(CommissionRuleViewModel model);
        Task<long> UpdateCommissionRuleInstrument(DataTable dataTable, CommissionRuleViewModel model);
        Task<CommissionRuleViewModel> GetCommissionRuleById(int Id);
        Task<Tuple<List<CommissionRuleInstrumentViewModel>, long>> GetCommissionRuleInstrumentById(PageParam pageParam, string search, int Id);

		Task<int> DeleteCommissionRuleInstrumentById(int CommisssionRuleId, int CommisssionRuleInstrumentId);
        Task<int> DeleteCommissionRuleById(int CommissionRuleId);
        Task<List<WhenToApplyComboViewModel>> GetWhenToApplyCombo();
        Task<List<UnitsTypeViewModel>> GetUnitsType();

        Task<List<WhereToApplyIdViewModel>> GetWhereToApplyId();
    }
}
