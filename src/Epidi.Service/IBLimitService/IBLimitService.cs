using Epidi.Models.Paging;
using Epidi.Models.ViewModel.Common;
using Epidi.Models.ViewModel.Country;
using Epidi.Models.ViewModel.IB;
using Epidi.Models.ViewModel.Instrument;
using Epidi.Models.ViewModel.MarginRuleViewModel;
using Epidi.Models.ViewModel.Question;
using Epidi.Models.ViewModel.SkipRule;
using Epidi.Models.ViewModel.TermsCondition;
using Epidi.Repository.IBLimitRepository;
using Epidi.Repository.IBRepository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Service.IBLimitService
{
    public class IBLimitService : IIBLimitService
    {
        private readonly IIBLimitRepository _IIBLimitRepository;

        public IBLimitService(IIBLimitRepository iIBLimitRepository)
        {
            _IIBLimitRepository = iIBLimitRepository;
        }


        public Tuple<List<IBLimitViewModel>, long> Get_All(PageParam pageParam, string search)
        {
            return _IIBLimitRepository.Get_All(pageParam, search);
        }


        public async Task<IBLimitViewModel> Limit_Upsert(IBLimitViewModel model)
        {
            return await _IIBLimitRepository.Limit_Upsert(model);
        }

        public Task<IBLimitViewModel> GetBy_Id(int MasterInstrumentId)
        {
            return _IIBLimitRepository.GetBy_Id(MasterInstrumentId);
        }

        public long GetSymbolGroupId(int MasterInstrumentId)
        {
            return _IIBLimitRepository.GetSymbolGroupId(MasterInstrumentId);
        }



        public List<ResponseViewModel> Delete(int Id)
        {
            return _IIBLimitRepository.Delete(Id);
        }

        public List<IBLimitViewModel> GetIBLimit_Data()
        {
            return _IIBLimitRepository.GetIBLimit_Data();
        }

        public async Task<long>ImportIBLimitData_UpsertByDt(DataTable dt_IBLimitData)
        {
            return await _IIBLimitRepository.ImportIBLimitData_UpsertByDt(dt_IBLimitData);
        }

        public List<IBLimitViewModel> GetIBLimit_All_Data()
        {
            return _IIBLimitRepository.GetIBLimit_All_Data();
        }
    }



}

