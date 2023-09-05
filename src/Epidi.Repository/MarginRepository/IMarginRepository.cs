using Epidi.Models.Paging;
using Epidi.Models.ViewModel.Common;
using Epidi.Models.ViewModel.Margin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Repository.MarginRepository
{
    public interface IMarginRepository
    {
        Tuple<List<MarginViewModel>, long> Get_All(PageParam pageParam, string search);
        Task<ResponseViewModel> Margin_Upsert(MarginViewModel marginViewModel);
        Task<List<DropDownItems>> GetMarginTypes();
        Task<List<DropDownItems>> GetInstruments();
        Task<List<DropDownItems>> GetSymbolGroups();
        Task<List<DropDownItems>> GetInstrumentToConvertTypes();
        List<ResponseViewModel> DeleteMargin(int id);
        Task<ResponseViewModel> MarginDetails_Upsert(List<MarginDetails> model, int MarginId);
        Task<List<MarginDetails>> GetMarginDetails_All(int MarginId);
        Task<long> ImportMargin(DataTable dataTable);
    }
}
