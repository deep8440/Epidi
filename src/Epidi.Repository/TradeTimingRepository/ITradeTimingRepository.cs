using Epidi.Models.ViewModel.Instrument;
using Epidi.Models.ViewModel.TradeTiming;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Repository.TradeTimingRepository
{
    public interface ITradeTimingRepository
    {
        Task<List<Models.ViewModel.TradeTiming.TradeTiming>> GetTradeTiming();
        Task<long> AddTradeTiming(Models.ViewModel.TradeTiming.TradeTiming tradeTiming);
        Task<long> RemoveTradeTiming(int id);
        Task<Models.ViewModel.TradeTiming.TradeTiming> GetTradeTimingById(int id);
        Task<long> EditTradeTiming(Models.ViewModel.TradeTiming.TradeTiming tradeTiming);
    }
}
