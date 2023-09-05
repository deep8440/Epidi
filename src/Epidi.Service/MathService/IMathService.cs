using Epidi.Models.Paging;
using Epidi.Models.ViewModel.Common;
using Epidi.Models.ViewModel.SkipRule;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Service.MathService
{
    public interface IMathService
    {
        Tuple<List<SkipRuleModelView>, long> GetMath(PageParam pageParam, string search);
        Task<long> AddMath(SkipRuleModelView Math);

        List<ResponseViewModel> RemoveMath(int Id);
      
        Task<SkipRuleModelView> GetMathById(int id);
        Task<long> EditMath(SkipRuleModelView Math);
        Task<long> SkipRule_InsertByImport(DataTable dataTable);
        Task<ResponseViewModel> CopyMathRule(int Id);
    }
}
