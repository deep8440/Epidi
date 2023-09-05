using Epidi.Models.Paging;
using Epidi.Models.ViewModel.Common;
using Epidi.Models.ViewModel.IB;
using Epidi.Models.ViewModel.IBCommissionMarkUpSettingInstrumentWise;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Service.IBCommisionMarkUpSettingInstrumentWise
{
    public interface IIBCommisionMarkUpSettingInstWiseService
    {
        Task<ResponseViewModel> Upsert(IBCommisionMarkUpSettingInstrumentWiseViewModel model);
        Tuple<List<IBCommisionMarkUpSettingInstrumentWiseViewModel>, long> GetAll(PageParam pageParam, string search);
        Task<List<CommisionMarkUpSetting>> GetIBMCommision_IBID(int IBId, int instrument_filter);
        Tuple<List<CommisionMarkUpSetting>, long> GetAll_Commission(PageParam pageParam, string search);
        Task<long> CommisionMarkUpSetting_UpsertByDt(DataTable dtRuleInstrument);
        
    }
}
