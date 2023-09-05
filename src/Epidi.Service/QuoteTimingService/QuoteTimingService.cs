using Epidi.Models.ViewModel.Instrument;
using Epidi.Models.ViewModel.QuoteTiming;
using Epidi.Repository.CountryRepository;
using Epidi.Repository.InstrumentRepository;
using Epidi.Repository.QuoteTimingRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Service.QuoteTimingService
{
    public class QuoteTimingService : IQuoteTimingService
    {
        private readonly IQuoteTimingRepository _repository;

        public QuoteTimingService(IQuoteTimingRepository repository)
        {
            _repository = repository;
        }
        public async Task<long> AddQuoteTiming(QuoteTiming quoteTiming)
        {
            return await _repository.AddQuoteTiming(quoteTiming);
        }

        public async Task<long> EditQuoteTiming(QuoteTiming quoteTiming)
        {
            return await _repository.EditQuoteTiming(quoteTiming);
        }
        public Task<QuoteTiming> GetQuoteTimingById(int Id)
        {
           return _repository.GetQuoteTimingById(Id);
        }

        public async Task<List<QuoteTiming>> GetQuoteTiming()
        {
            return await _repository.GetQuoteTiming();
        }

        public async Task<long> RemoveQuoteTiming(int id)
        {
            return await _repository.RemoveQuoteTiming(id);
        }
    }
}
