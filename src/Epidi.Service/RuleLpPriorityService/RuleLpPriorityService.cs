using Epidi.Models.Paging;
using Epidi.Models.ViewModel.Common;
using Epidi.Models.ViewModel.RuleLpPriority;
using Epidi.Repository.RuleLpPriorityRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Service.RuleLpPriorityService
{
    public class RuleLpPriorityService : IRuleLpPriorityService
    {
        private readonly IRuleLpPriorityRepository _ruleLpPriorityRepository;

        public RuleLpPriorityService(IRuleLpPriorityRepository ruleLpPriorityRepository)
        {
            _ruleLpPriorityRepository = ruleLpPriorityRepository;
        }

        public List<ResponseViewModel> Delete(int Id)
        {
            return _ruleLpPriorityRepository.Delete(Id);
        }

        public Tuple<List<RuleLpPriorityViewModel>, long> GetAll(PageParam pageParam, string search)
        {
            return _ruleLpPriorityRepository.GetAll(pageParam,search);
        }

        public async Task<RuleLpPriorityViewModel> GetById(int Id)
        {
            return await _ruleLpPriorityRepository.GetById(Id);
        }
        public async Task<List<RuleLpPriorityViewModel>> GetByRuleId(int Id)
        {
            return await _ruleLpPriorityRepository.GetByRuleId(Id);
        }

        public async Task<RuleLpPriorityViewModel> Upsert(RuleLpPriorityViewModel model)
        {
            return await _ruleLpPriorityRepository.Upsert(model);
        }
    }
}
