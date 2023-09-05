using Epidi.Models.Paging;
using Epidi.Models.ViewModel.Common;
using Epidi.Models.ViewModel.Instrument;
using Epidi.Repository.InstrumentRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Service.InstrumentService
{
    public class InstrumentSpecificationService : IInstrumentSpecificationService
    {
        private readonly IInstrumentSpecificationRepository _instrumentSpecificationRepository;
        public InstrumentSpecificationService(IInstrumentSpecificationRepository instrumentSpecificationRepository)
        {
            _instrumentSpecificationRepository = instrumentSpecificationRepository;
        }
        public Task<Tuple<List<InstrumentSpecificationViewModel>, long>> GetAllnstrumentSpecification(PageParam pageParam, string search)
        {
            return _instrumentSpecificationRepository.GetAllnstrumentSpecification(pageParam,search);
        }
        public Task<InstrumentSpecificationViewModel> GetInstrumentSpecification(EditInstrumentSpecificationViewModel model)
        {
            return _instrumentSpecificationRepository.GetInstrumentSpecification(model);
        }
        public List<ResponseViewModel> UpdateInstrumentSpecification(InstrumentSpecificationViewModel model)
        {
            return _instrumentSpecificationRepository.UpdateInstrumentSpecification(model);
        }

        public List<ResponseViewModel> DeleteInstrumentSpecification(int Id)
        {
            return _instrumentSpecificationRepository.DeleteInstrumentSpecification(Id);
        }
    }
}
