using Epidi.Models.Paging;
using Epidi.Models.ViewModel.Common;
using Epidi.Models.ViewModel.MarginRuleViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Service.MarginRuleService
{
    public interface IMarginRuleService
    {
        Task<MarginRuleViewModel> Upsert(MarginRuleViewModel model);
        Tuple<List<MarginRuleViewModel>, long> GetAll(PageParam pageParam, string search);
        Task<MarginRuleViewModel> GetById(int Id);
        List<ResponseViewModel> Delete(int Id);
    }
}
