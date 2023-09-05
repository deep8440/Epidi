using Epidi.Models.ViewModel.TradeTiming;
using Epidi.Repository.TradeTimingRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Service.TradeTimingService
{
    public class TradeTimingService : ITradeTimingService
    {
        private readonly ITradeTimingRepository _repository;

        public TradeTimingService(ITradeTimingRepository repository)
        {
            _repository = repository;
        }
        public async Task<long> AddTradeTiming(TradeTiming tradeTiming)
        {
            return await _repository.AddTradeTiming(tradeTiming);
        }

        public async Task<long> EditTradeTiming(TradeTiming tradeTiming)
        {
            return await _repository.EditTradeTiming(tradeTiming);
        }

        
        public Task<List<TradeTiming>> GetTradeTiming()
        {
           return _repository.GetTradeTiming();
        }

        public async Task<long> RemoveTradeTiming(int id)
        {
            return await _repository.RemoveTradeTiming(id);
        }
        public async Task<TradeTiming> GetTradeTimingById(int id)
        {
            return await _repository.GetTradeTimingById(id);
        }
        
    }
}
