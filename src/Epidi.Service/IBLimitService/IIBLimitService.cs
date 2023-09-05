using Epidi.Models.Paging;
using Epidi.Models.ViewModel.CommissionRule;
using Epidi.Models.ViewModel.Common;
using Epidi.Models.ViewModel.Country;
using Epidi.Models.ViewModel.IB;
using Epidi.Models.ViewModel.Instrument;
using Epidi.Models.ViewModel.MarginRuleViewModel;
using Epidi.Models.ViewModel.Question;
using Epidi.Models.ViewModel.SkipRule;
using Epidi.Models.ViewModel.TermsCondition;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Service.IBLimitService
{
    public interface IIBLimitService
    {

        Tuple<List<IBLimitViewModel>, long> Get_All(PageParam pageParam, string search);

        Task<IBLimitViewModel> Limit_Upsert(IBLimitViewModel model);


        public Task<IBLimitViewModel> GetBy_Id(int MasterInstrumentId);

        public long GetSymbolGroupId(int MasterInstrumentId);

        public List<ResponseViewModel> Delete(int MarkUpPer1000);


        public List<IBLimitViewModel> GetIBLimit_Data();

        Task<long>ImportIBLimitData_UpsertByDt (DataTable dt_IBLimitData);

        public List<IBLimitViewModel> GetIBLimit_All_Data();

    }
}
