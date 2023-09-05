using Epidi.Models.ViewModel.QuoteTiming;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Service.QuoteTimingService
{
    public interface IQuoteTimingService
    {
        Task<List<QuoteTiming>> GetQuoteTiming();
        Task<QuoteTiming> GetQuoteTimingById(int id);
        Task<long> AddQuoteTiming(QuoteTiming quoteTiming);
        Task<long> RemoveQuoteTiming(int id);
        Task<long> EditQuoteTiming(QuoteTiming quoteTiming);
    }
}
