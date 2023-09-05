using Epidi.Models.Paging;
using Epidi.Models.ViewModel.Common;
using Epidi.Models.ViewModel.MasterLP;
using Epidi.Repository.MasterLP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Service.MasterLPService
{
    public class MasterLPService : IMasterLPService
    {
        private readonly IMasterLPRepository _masterLPRepository;

        public MasterLPService(IMasterLPRepository masterLPRepository)
        {
            _masterLPRepository = masterLPRepository;
        }

        public List<ResponseViewModel> Delete(int Id)
        {
            return _masterLPRepository.Delete(Id);
        }

        public Tuple<List<MasterLPViewModel>, long> GetAll(PageParam pageParam, string search)
        {
            return _masterLPRepository.GetAll(pageParam, search);
        }

        public Task<MasterLPViewModel> GetById(int Id)
        {
            return _masterLPRepository.GetById(Id);
        }

        public Task<MasterLPViewModel> Upsert(MasterLPViewModel model)
        {
            return _masterLPRepository.Upsert(model);
        }
    }
}
