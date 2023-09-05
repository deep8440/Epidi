using Epidi.Models.Paging;
using Epidi.Models.ViewModel.BDM;
using Epidi.Models.ViewModel.Common;
using Epidi.Models.ViewModel.IB;
using Epidi.Models.ViewModel.IBCommissionMarkUpSettingInstrumentWise;
using Epidi.Models.ViewModel.IBLimit;
using Epidi.Models.ViewModel.LegalEntity;
using Epidi.Models.ViewModel.Steps;
using Epidi.Models.ViewModel.SwapRule;
using Epidi.Repository.IBRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Service.IBService
{
    public class IBService : IIBService
    {
        private readonly IIBRepository _IIBRepository;

        public IBService(IIBRepository iIBRepository)
        {
            _IIBRepository = iIBRepository;
        }

        public Tuple<List<IBViewModel>, long> GetAll(PageParam pageParam, string search)
        {
            return _IIBRepository.GetAll(pageParam, search);
        }

        public async Task<IBViewModel> Upsert(IBViewModel model)
        {
            return await _IIBRepository.Upsert(model);
        }
        public async Task<ResponseViewModel> Import(Tuple<string, decimal, decimal, decimal, decimal, decimal, decimal, Tuple<int>> model, int BDMId)
        {
            return await _IIBRepository.Import(model, BDMId);
        }

        //public Tuple<List<IBViewModel>, long> GetAllBy_BDMID(PageParam pageParam, string search, string BDMID)
        //{
        //    return _IIBRepository.GetAllBy_BDMID(pageParam, search, BDMID);
        //}
        public async Task<IBViewModel> GetById(int Id)
        {
            return await _IIBRepository.GetById(Id);
        }
        public async Task<List<IBCommisionMarkUpSettingInstrumentWiseViewModel>> GetIBCommitionById(int Id)
        {
            return await _IIBRepository.GetIBCommitionById(Id);
        }

		public List<ResponseViewModel> DeleteIB(int Id)
		{
			return _IIBRepository.DeleteIB(Id);
		}

        public async Task<int> DeleteIBRuleInstrumentById(int IBId, int MasterInstrumentalID)
        {
            return await _IIBRepository.DeleteIBRuleInstrumentById(IBId, MasterInstrumentalID);
        }

        public async Task<IBCommissionInstrument> GetIBRuleInstrumentById(int IBId, int MasterInstrumentalID)
        {
            return await _IIBRepository.GetIBRuleInstrumentById(IBId, MasterInstrumentalID);
        }

        public Task<IBCommissionInstrument> SaveIBCommissionInstrument(IBCommissionInstrument model)
        {
            return _IIBRepository.SaveIBCommissionInstrument(model);
        }

        public Task<ResponseViewModel> SaveIBInstrumentRule(IBCommissionInstrument model)
        {
            return _IIBRepository.SaveIBInstrumentRule(model);
        }

		public async Task<bool> CheckIBRule(int Id,int MasterInstrumentId)
		{
			return await _IIBRepository.CheckIBRule(Id, MasterInstrumentId);
		}

        public async Task<IBLimitValidateRespViewModel> ValidateIBLimit(IBLimitValidateViewModel model)
        {
            return await _IIBRepository.ValidateIBLimit(model);
        }
    }
}
