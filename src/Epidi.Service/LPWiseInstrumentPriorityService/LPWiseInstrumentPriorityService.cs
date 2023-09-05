using Epidi.Models.ViewModel.InstrumentMaster;
using Epidi.Models.ViewModel.LP;
using Epidi.Models.ViewModel.LPWiseInstrumentPriority;
using Epidi.Repository.LPWiseInstrumentPriorityRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Service.LPWiseInstrumentPriorityService
{
    public class LPWiseInstrumentPriorityService : ILPWiseInstrumentPriorityService
    {
        private readonly ILPWiseInstrumentPriorityRepository _repository;

        public LPWiseInstrumentPriorityService(ILPWiseInstrumentPriorityRepository repository)
        {
            _repository = repository;
        }
        public async Task<long> AddLPWiseInstrumentPriority(LPWiseInstrumentPriority lPWiseInstrumentPriority)
        {
            return await _repository.AddLPWiseInstrumentPriority(lPWiseInstrumentPriority);
        }

        public async Task<long> EditLPWiseInstrumentPriority(LPWiseInstrumentPriority lPWiseInstrumentPriority)
        {
            return await _repository.EditLPWiseInstrumentPriority(lPWiseInstrumentPriority);
        }

        
        public Task<List<LPWiseInstrumentPriority>> GetLPWiseInstrumentPriority()
        {
           return _repository.GetLPWiseInstrumentPriority();
        }

        public async Task<long> RemoveLPWiseInstrumentPriority(int id,int DeletedBy)
        {
            return await _repository.RemoveLPWiseInstrumentPriority(id, DeletedBy);
        }
        public async Task<long> ActiveInActiveLPWiseInstrumentPriority(int id,bool IsActive)
        {
            return await _repository.ActiveInActiveLPWiseInstrumentPriority(id, IsActive);
        }
        public async Task<List<LPViewModel>> Get_All_LP()
        {
            return await _repository.Get_All_LP();
        }
        public async Task<LPWiseInstrumentPriority> GetLPWiseInstrumentPriorityById(int id)
        {
            return await _repository.GetLPWiseInstrumentPriorityById(id);
        }
        
    }
}
