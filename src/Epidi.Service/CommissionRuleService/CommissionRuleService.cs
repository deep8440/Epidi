using Epidi.Models.Paging;
using Epidi.Models.ViewModel.CommissionRule;
using Epidi.Repository.CommissionRuleRepository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Service.CommissionRuleService
{
    public class CommissionRuleService : ICommissionRuleService
    {
        private readonly ICommissionRuleRepository _commissionRuleRepository;
        public CommissionRuleService(ICommissionRuleRepository commissionRuleRepository)
        {
            _commissionRuleRepository = commissionRuleRepository;
        }
        public async Task<long> AddCommissionRule(DataTable dataTable, CommissionRuleViewModel model)
        {
            return await _commissionRuleRepository.AddCommissionRule(dataTable, model);
        }
        public async Task<long> SaveCommissionRuleWithoutFile(CommissionRuleViewModel model)
        {
            return await _commissionRuleRepository.SaveCommissionRuleWithoutFile(model);
        }

        public async Task<int> DeleteCommissionRuleById(int CommissionRuleId)
        {
            return await _commissionRuleRepository.DeleteCommissionRuleById(CommissionRuleId);
        }

        public async Task<int> DeleteCommissionRuleInstrumentById(int CommisssionRuleId, int CommisssionRuleInstrumentId)
        {
            return await _commissionRuleRepository.DeleteCommissionRuleInstrumentById(CommisssionRuleId,CommisssionRuleInstrumentId);
        }

        public async Task<Tuple<List<CommissionRuleViewModel>, long>> GetAllCommissionRules(PageParam pageParam, string search)
        {
            return await _commissionRuleRepository.GetAllCommissionRules(pageParam,search);
        }

        public async Task<CommissionRuleViewModel> GetCommissionRuleById(int Id)
        {
            return await _commissionRuleRepository.GetCommissionRuleById(Id);
        }

        public async Task<Tuple<List<CommissionRuleInstrumentViewModel>, long>> GetCommissionRuleInstrumentById(PageParam pageParam, string search, int Id)
		{
            return await _commissionRuleRepository.GetCommissionRuleInstrumentById(pageParam, search, Id);

        }
        public async Task<long> UpdateCommissionRuleInstrument(DataTable dataTable, CommissionRuleViewModel model)
        {
            return await _commissionRuleRepository.UpdateCommissionRuleInstrument(dataTable, model);

        }

        public async Task<List<UnitsTypeViewModel>> GetUnitsType()
        {
            return await _commissionRuleRepository.GetUnitsType();
        }

        public async Task<List<WhenToApplyComboViewModel>> GetWhenToApplyCombo()
        {
            return await _commissionRuleRepository.GetWhenToApplyCombo();
        }

        public async Task<List<WhereToApplyIdViewModel>> GetWhereToApplyId()
        {
            return await _commissionRuleRepository.GetWhereToApplyId();
        }
    }
}
