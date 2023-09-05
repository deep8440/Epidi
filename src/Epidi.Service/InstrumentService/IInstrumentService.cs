using Epidi.Models.Paging;
using Epidi.Models.ViewModel.Instrument;
using Epidi.Models.ViewModel.InstrumentMaster;
using Epidi.Models.ViewModel.Question;
using Epidi.Models.ViewModel.SymbolGroup;
using Epidi.Models.ViewModel.TradeStatus;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Service.InstrumentService
{
    public interface IInstrumentService
    {
        Task<Tuple<List<InstrumentMaster>, long>> GetAllInstrument(PageParam pageParam, string search);
        Task<List<InstrumentMaster>> GetInstruments();
        Task<List<GateIO>> GetGateIOs();
        Task<List<LMax>> GetLMaxes();
        Task<long> UpdateInstrumentMaster(MasterInstrumentViewModel model);
        Task<long> AddMapping(InstrumentMasterMapping instrumentMasterMapping);
        Task<long> AddMultipleMapping(DataTable dataTable);
        Task<long> RemoveMapping(int id);
        Task<long> EditMapping(InstrumentMasterMapping instrumentMasterMapping);
        Task<long> AddFavourite(FavouriteInstrument favouriteInstrument);
        Task<long> AddGateIOInstrument(DataTable dataTable);
        Task<long> AddLMaxInstrument(DataTable dataTable);
        Task<long> AddInstrumentMapping(DataTable dataTable);
        Task<long> AddMasterInstrumentMapping(DataTable dataTable, InstrumentSpecificationRule model);
        Task<Tuple<List<EditSpecificationInstrumentViewModel>, long>> GetAllImportMasterInstrument(int RuleId, PageParam pageParam,string search);
        Task<Tuple<List<ImportGateIOInstrumentMappingViewModel>, long>> GetAllImportGATEIO(PageParam pageParam, string search);
        Task<Tuple<List<ImportLMaxInstrumentsViewModel>, long>> GetAllImportLMax(PageParam pageParam, string search);
        Task<List<TradeStatusViewModel>> GetAllTradeStatus();
        Task<List<SymbolGroupViewModel>> GetAllSymbolGroup();
        Task<List<InstrumentMasterDDViewModel>> GetInstrument_DD_Data(int MasterInstrumentId, string LPName);
        Task<List<MasterInstrumentViewModel>> GetByID(int MasterInstrumentId, int TradeStatusId);
        Task<List<InstrumentMaster>> GetAllInstrumentPopular();
        Task<List<CommissionRuleInstrument>> GetInstrumentsWithCommission();
        Task<Tuple<List<InstrumentMaster>, long>> GetAllInstrumentByUserId(PageParam pageParam, string search, int UserId);
        Task<Tuple<List<InstrumentSpecificationRule>, long>> GetAllSpecificationRule(PageParam pageParam, string search);
        Task<InstrumentSpecificationRule> GetSpecificationRuleByID(int Id);
        Task<long> UpdateSpecificationRuleInstrument(DataTable dataTable, InstrumentSpecificationRule model);
        Task<int> DeleteSpecificationRuleInstrumentById(int SpecificationRuleId, int InstrumentId);
        Task<int> DeleteSpecificationRuleById(int RuleId);


        Task<List<InstrumentMaster>> GetInstrumentListAll();
        Task<List<SymbolGroupViewModel>> GetSymbolListALL();
        Task<List<InstrumentWithIBCommision>> GetInstrumentsWithIBCommission(int IBID);


    }
}
