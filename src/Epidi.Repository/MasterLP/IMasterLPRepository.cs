using Epidi.Models.Paging;
using Epidi.Models.ViewModel.Common;
using Epidi.Models.ViewModel.MasterLP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Repository.MasterLP
{
    public interface IMasterLPRepository
    {
        Task<MasterLPViewModel> Upsert(MasterLPViewModel model);
        Tuple<List<MasterLPViewModel>, long> GetAll(PageParam pageParam, string search);
        Task<MasterLPViewModel> GetById(int Id);
        List<ResponseViewModel> Delete(int Id);
    }
}
