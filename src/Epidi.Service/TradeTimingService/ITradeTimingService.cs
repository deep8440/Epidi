using Epidi.Models.ViewModel.TradeTiming;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Service.TradeTimingService
{
    public interface ITradeTimingService
    {
        Task<List<TradeTiming>> GetTradeTiming();
        Task<long> AddTradeTiming(TradeTiming tradeTiming);
        Task<long> RemoveTradeTiming(int id);
        Task<TradeTiming> GetTradeTimingById(int id);
        Task<long> EditTradeTiming(TradeTiming tradeTiming);
    }
}
