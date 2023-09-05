using Epidi.Models.Paging;
using Epidi.Models.ViewModel.Common;
using Epidi.Models.ViewModel.Instrument;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Repository.InstrumentRepository
{
    public interface IInstrumentSpecificationRepository
    {
        Task<Tuple<List<InstrumentSpecificationViewModel>, long>> GetAllnstrumentSpecification(PageParam pageParam, string search);
        Task<InstrumentSpecificationViewModel> GetInstrumentSpecification(EditInstrumentSpecificationViewModel model);
        List<ResponseViewModel> UpdateInstrumentSpecification(InstrumentSpecificationViewModel model);

        List<ResponseViewModel> DeleteInstrumentSpecification(int Id);

    }
}
