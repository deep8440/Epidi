using Epidi.Models.Paging;
using Epidi.Models.ViewModel.Common;
using Epidi.Models.ViewModel.Margin;
using Epidi.Repository.MarginRepository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Service.MarginService
{
    public class MarginService : IMarginService
    {
        private readonly IMarginRepository _marginRepository;
        public MarginService(IMarginRepository marginRepository) 
        { 
            _marginRepository= marginRepository;
        }
        public Tuple<List<MarginViewModel>, long> Get_All(PageParam pageParam, string search)
        {
            return _marginRepository.Get_All(pageParam, search);
        }
        public List<ResponseViewModel> DeleteMargin(int id)
        {
            return  _marginRepository.DeleteMargin(id);
        }

        public async Task<ResponseViewModel> Margin_Upsert(MarginViewModel marginViewModel)
        {
            return await _marginRepository.Margin_Upsert(marginViewModel);
        }
        public async Task<List<DropDownItems>> GetMarginTypes()
        {
            return await _marginRepository.GetMarginTypes();
        }
        public async Task<List<DropDownItems>> GetInstrumentToConvertTypes()
        {
            return await _marginRepository.GetInstrumentToConvertTypes();
        }
        public async Task<List<DropDownItems>> GetInstruments()
        {
            return await _marginRepository.GetInstruments();
        }
        public async Task<List<DropDownItems>> GetSymbolGroups()
        {
            return await _marginRepository.GetSymbolGroups();
        }
        public async Task<ResponseViewModel> MarginDetails_Upsert(List<MarginDetails> model, int MarginId)
        {
            return await _marginRepository.MarginDetails_Upsert(model, MarginId);
        }
        public Task<List<MarginDetails>> GetMarginDetails_All(int MarginId)
        {
            return _marginRepository.GetMarginDetails_All(MarginId);
        }
        public async Task<long> ImportMargin(DataTable dataTable)
        {
            return await _marginRepository.ImportMargin(dataTable);
        }
    }
}
