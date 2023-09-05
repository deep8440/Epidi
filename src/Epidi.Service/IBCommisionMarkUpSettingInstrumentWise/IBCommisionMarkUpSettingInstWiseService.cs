using Epidi.Models.Paging;
using Epidi.Models.ViewModel.Common;
using Epidi.Models.ViewModel.IB;
using Epidi.Models.ViewModel.IBCommissionMarkUpSettingInstrumentWise;
using Epidi.Repository.IBCommisionMarkUpSettingInstrumentWise;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Service.IBCommisionMarkUpSettingInstrumentWise
{
    public class IBCommisionMarkUpSettingInstWiseService : IIBCommisionMarkUpSettingInstWiseService
    {
        private readonly IIBCommisionMarkUpSettingInstrumentWise _IIBCommisionMarkUpSettingInstrumentWise;
        public IBCommisionMarkUpSettingInstWiseService(IIBCommisionMarkUpSettingInstrumentWise iIBCommisionMarkUpSettingInstrumentWise)
        {
            _IIBCommisionMarkUpSettingInstrumentWise= iIBCommisionMarkUpSettingInstrumentWise;
        }

        public Tuple<List<IBCommisionMarkUpSettingInstrumentWiseViewModel>, long> GetAll(PageParam pageParam, string search)
        {
            return _IIBCommisionMarkUpSettingInstrumentWise.GetAll(pageParam, search);
        }

        public async Task<List<CommisionMarkUpSetting>> GetIBMCommision_IBID(int IBId,int instrument_filter)
        {
            return await _IIBCommisionMarkUpSettingInstrumentWise.GetIBMCommision_IBID(IBId, instrument_filter);
        }
        public async Task<ResponseViewModel> Upsert(IBCommisionMarkUpSettingInstrumentWiseViewModel model)
        {
            return await _IIBCommisionMarkUpSettingInstrumentWise.Upsert(model);
        }
       
        
        public Tuple<List<CommisionMarkUpSetting>, long> GetAll_Commission(PageParam pageParam, string search)
        {
            return _IIBCommisionMarkUpSettingInstrumentWise.GetAll_Commission(pageParam, search);
        }

        public async Task<long> CommisionMarkUpSetting_UpsertByDt(DataTable dtRuleInstrument)
        {
            return await _IIBCommisionMarkUpSettingInstrumentWise.CommisionMarkUpSetting_UpsertByDt(dtRuleInstrument);
        }
    }
}
