using Epidi.Models.Paging;
using Epidi.Models.ViewModel.Instrument;
using Epidi.Models.ViewModel.InstrumentMaster;
using Epidi.Models.ViewModel.SymbolGroup;
using Epidi.Models.ViewModel.TradeStatus;
using Epidi.Repository.CountryRepository;
using Epidi.Repository.InstrumentRepository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Service.InstrumentService
{
    public class InstrumentService : IInstrumentService
    {
        private readonly IInstrumentRepository _repository;

        public InstrumentService(IInstrumentRepository repository)
        {
            _repository = repository;
        }
        public async Task<long> AddMapping(InstrumentMasterMapping instrumentMasterMapping)
        {
            return await _repository.AddMapping(instrumentMasterMapping);
        }
        public async Task<long> AddMultipleMapping(DataTable dataTable)
        {
            return await _repository.AddMultipleMapping(dataTable);
        }
        public async Task<long> AddGateIOInstrument(DataTable dataTable)
        {
            return await _repository.AddGateIOInstrument(dataTable);
        }
        public async Task<long> AddLMaxInstrument(DataTable dataTable)
        {
            return await _repository.AddLMaxInstrument(dataTable);
        }
        public async Task<long> AddInstrumentMapping(DataTable dataTable)
        {
            return await _repository.AddInstrumentMapping(dataTable);
        }
        public async Task<long> AddMasterInstrumentMapping(DataTable dataTable, InstrumentSpecificationRule model)
        {
            return await _repository.AddMasterInstrumentMapping(dataTable, model);
        }
        public async Task<long> UpdateSpecificationRuleInstrument(DataTable dataTable, InstrumentSpecificationRule model)
        {
            return await _repository.UpdateSpecificationRuleInstrument(dataTable,model);
        }
        public async Task<int> DeleteSpecificationRuleInstrumentById(int SpecificationRuleId, int InstrumentId)
        {
            return await _repository.DeleteSpecificationRuleInstrumentById(SpecificationRuleId, InstrumentId);
        } 
        public async Task<int> DeleteSpecificationRuleById(int RuleId)
        {
            return await _repository.DeleteSpecificationRuleById(RuleId);
        }
        
        public async Task<long> EditMapping(InstrumentMasterMapping instrumentMasterMapping)
        {
            return await _repository.EditMapping(instrumentMasterMapping);
        }

        public async Task<List<GateIO>> GetGateIOs()
        {
            return await _repository.GetGateIOs();
        }

        public Task<List<InstrumentMaster>> GetInstruments()
        {
           return _repository.GetInstruments();
        }

        public async Task<List<LMax>> GetLMaxes()
        {
            return await _repository.GetLMaxes();
        }

        public async Task<long> RemoveMapping(int id)
        {
            return await _repository.RemoveMapping(id);
        }
        public async Task<long> AddFavourite(FavouriteInstrument favouriteInstrument)
        {
            return 0;
        }
        public async Task<long> UpdateInstrumentMaster(MasterInstrumentViewModel model)
        {
            return await _repository.UpdateInstrumentMaster(model);
        }
        public async Task<Tuple<List<InstrumentMaster>, long>> GetAllInstrument(PageParam pageParam, string search)
        {
            return await _repository.GetAllInstrument(pageParam,search);
        }
        public async Task<Tuple<List<InstrumentMaster>, long>> GetAllInstrumentByUserId(PageParam pageParam, string search,int UserId)
        {
            return await _repository.GetAllInstrumentByUserId(pageParam, search, UserId);
        }
        public async Task<Tuple<List<EditSpecificationInstrumentViewModel>, long>> GetAllImportMasterInstrument(int RuleId, PageParam pageParam, string search)
        {
            return await _repository.GetAllImportMasterInstrument(RuleId, pageParam, search);
        }
        public async Task<Tuple<List<ImportGateIOInstrumentMappingViewModel>, long>> GetAllImportGATEIO(PageParam pageParam, string search)
        {
            return await _repository.GetAllImportGATEIO(pageParam, search);
        }
        public async Task<Tuple<List<ImportLMaxInstrumentsViewModel>, long>> GetAllImportLMax(PageParam pageParam, string search)
        {
            return await _repository.GetAllImportLMax(pageParam, search);
        }
        public async Task<List<InstrumentMasterDDViewModel>> GetInstrument_DD_Data(int MasterInstrumentId, string LPName)
        {
            return await  _repository.GetInstrument_DD_Data(MasterInstrumentId,LPName);
        }
        public async Task<List<TradeStatusViewModel>> GetAllTradeStatus()
        {
            return await _repository.GetAllTradeStatus();
        }
        public async Task<List<MasterInstrumentViewModel>> GetByID(int MasterInstrumentId,int TradeStatusId)
        {
            return await _repository.GetByID(MasterInstrumentId, TradeStatusId);
        }
        public async Task<List<SymbolGroupViewModel>> GetAllSymbolGroup()
        {
            return await _repository.GetAllSymbolGroup();
        }
        public async Task<List<InstrumentMaster>> GetAllInstrumentPopular()
        {
            return await _repository.GetAllInstrumentPopular();
        }
        
        public async Task<List<CommissionRuleInstrument>> GetInstrumentsWithCommission()
        {
            return await _repository.GetInstrumentsWithCommission();
        }
        public async Task<List<InstrumentWithIBCommision>> GetInstrumentsWithIBCommission(int IBID)
        {
            return await _repository.GetInstrumentsWithIBCommission(IBID);
        }
        public async Task<Tuple<List<InstrumentSpecificationRule>, long>> GetAllSpecificationRule(PageParam pageParam, string search)
        {
            return await _repository.GetAllSpecificationRule(pageParam, search);
        }

        public async Task<InstrumentSpecificationRule> GetSpecificationRuleByID(int Id)
        {
            return await _repository.GetSpecificationRuleByID(Id);

            }

        public Task<List<InstrumentMaster>> GetInstrumentListAll()
        {
            return _repository.GetInstrumentListAll();
        }
        public async Task<List<SymbolGroupViewModel>> GetSymbolListALL()
        {
            return await _repository.GetSymbolListALL();
        }
    }
}
