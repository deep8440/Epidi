using Epidi.Models.Paging;
using Epidi.Models.ViewModel.BDM;
using Epidi.Models.ViewModel.BDMCommisionMarkUpSettingInstrumentWise;
using Epidi.Models.ViewModel.InstrumentMaster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Repository.BDM
{
    public interface IBDMRepository
    {
        Task<BDMViewModel> Upsert(BDMViewModel model);
        Tuple<List<BDMViewModel>, long> GetAll(PageParam pageParam, string search);
        Task<List<InstrumentMasterViewModel>> Get_All_Instrument();
        Task<BDMViewModel> GetById(int Id);
        Task<BDMCommisionMarkUpSettingInstrumentWiseViewModel> GetBDMCommision_BDMID(int Id);
        Task<BDMCommisionMarkUpSettingInstrumentWiseViewModel> SaveBDMCommision(BDMCommisionMarkUpSettingInstrumentWiseViewModel model);
        Task<List<BDMViewModel>> GetBDMListALL();
    }
}
