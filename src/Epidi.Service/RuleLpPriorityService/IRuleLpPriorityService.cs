using Epidi.Models.Paging;
using Epidi.Models.ViewModel.Common;
using Epidi.Models.ViewModel.RuleLpPriority;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Service.RuleLpPriorityService
{
    public interface IRuleLpPriorityService
    {
        Task<RuleLpPriorityViewModel> Upsert(RuleLpPriorityViewModel model);
        Tuple<List<RuleLpPriorityViewModel>, long> GetAll(PageParam pageParam, string search);
        Task<RuleLpPriorityViewModel> GetById(int Id);
        Task<List<RuleLpPriorityViewModel>> GetByRuleId(int Id);
        List<ResponseViewModel> Delete(int Id);
    }
}
