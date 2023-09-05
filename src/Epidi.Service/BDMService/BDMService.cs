using Epidi.Models.Paging;
using Epidi.Models.ViewModel.BDM;
using Epidi.Models.ViewModel.BDMCommisionMarkUpSettingInstrumentWise;
using Epidi.Models.ViewModel.InstrumentMaster;
using Epidi.Repository.BDM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Service.BDM
{
    public class BDMService : IBDMService
    {
        private readonly IBDMRepository _BDMRepository;

        public BDMService(IBDMRepository bDMRepository)
        {
            _BDMRepository = bDMRepository;
        }

        public Tuple<List<BDMViewModel>, long> GetAll(PageParam pageParam, string search)
        {
            return _BDMRepository.GetAll(pageParam, search);
        }

        public Task<BDMViewModel> Upsert(BDMViewModel model)
        {
            return _BDMRepository.Upsert(model);
        }
        public async Task<List<InstrumentMasterViewModel>> Get_All_Instrument()
        {
            return await _BDMRepository.Get_All_Instrument();
        }

        public async Task<BDMViewModel> GetById(int Id)
        {
            return await _BDMRepository.GetById(Id);
        }

        public async Task<BDMCommisionMarkUpSettingInstrumentWiseViewModel> GetBDMCommision_BDMID(int Id)
        {
            return await _BDMRepository.GetBDMCommision_BDMID(Id);
        }

        public async Task<BDMCommisionMarkUpSettingInstrumentWiseViewModel> SaveBDMCommision(BDMCommisionMarkUpSettingInstrumentWiseViewModel model)
        {
            return await _BDMRepository.SaveBDMCommision(model);
        }

        public async Task<List<BDMViewModel>> GetBDMListALL()
        {
            return await _BDMRepository.GetBDMListALL();
        }
    }
}
