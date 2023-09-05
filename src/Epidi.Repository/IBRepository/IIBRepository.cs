using Epidi.Models.Paging;
using Epidi.Models.ViewModel.Common;
using Epidi.Models.ViewModel.IB;
using Epidi.Models.ViewModel.IBCommissionMarkUpSettingInstrumentWise;
using Epidi.Models.ViewModel.IBLimit;
using Epidi.Models.ViewModel.Steps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Repository.IBRepository
{
    public interface IIBRepository
    {
        Task<IBViewModel> Upsert(IBViewModel model);
        Task<ResponseViewModel> Import(Tuple<string, decimal, decimal, decimal, decimal, decimal, decimal, Tuple<int>> model, int BDMId);
        Tuple<List<IBViewModel>, long> GetAll(PageParam pageParam, string search);
        //Tuple<List<IBViewModel>, long> GetAllBy_BDMID(PageParam pageParam, string search, string BDMID);
        Task<IBViewModel> GetById(int Id);
        Task<List<IBCommisionMarkUpSettingInstrumentWiseViewModel>> GetIBCommitionById(int id);

		public List<ResponseViewModel> DeleteIB(int Id);

        Task<int> DeleteIBRuleInstrumentById(int IBId, int MasterInstrumentalID);
        Task<IBCommissionInstrument> GetIBRuleInstrumentById(int IBId, int MasterInstrumentalID);

        Task<IBCommissionInstrument> SaveIBCommissionInstrument(IBCommissionInstrument model);

        Task<ResponseViewModel> SaveIBInstrumentRule(IBCommissionInstrument model);

		Task<bool> CheckIBRule(int Id, int MasterInstrumentId);

        Task<IBLimitValidateRespViewModel> ValidateIBLimit(IBLimitValidateViewModel model);




    }
}
