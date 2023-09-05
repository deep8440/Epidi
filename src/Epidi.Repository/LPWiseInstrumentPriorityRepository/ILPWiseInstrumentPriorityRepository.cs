using Epidi.Models.ViewModel.Instrument;
using Epidi.Models.ViewModel.InstrumentMaster;
using Epidi.Models.ViewModel.LP;
using Epidi.Models.ViewModel.LPWiseInstrumentPriority;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Repository.LPWiseInstrumentPriorityRepository
{
    public interface ILPWiseInstrumentPriorityRepository
    {
        Task<List<LPWiseInstrumentPriority>> GetLPWiseInstrumentPriority();
        Task<long> AddLPWiseInstrumentPriority(LPWiseInstrumentPriority lPWiseInstrumentPriority);
        Task<long> RemoveLPWiseInstrumentPriority(int id,int DeletedBy);
        Task<long> ActiveInActiveLPWiseInstrumentPriority(int id,bool IsActive);
        Task<LPWiseInstrumentPriority> GetLPWiseInstrumentPriorityById(int id);
        Task<long> EditLPWiseInstrumentPriority(LPWiseInstrumentPriority lPWiseInstrumentPriority);
        Task<List<LPViewModel>> Get_All_LP();
    }
}
