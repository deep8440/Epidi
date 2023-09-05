using Epidi.Models.Paging;
using Epidi.Models.ViewModel.Common;
using Epidi.Models.ViewModel.MarginRuleViewModel;
using Epidi.Repository.MarginRuleRepository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Service.MarginRuleService
{
    public class MarginRuleService : IMarginRuleService
    {
        private readonly IMarginRuleRepository _IMarginRuleRepository;

        public MarginRuleService(IMarginRuleRepository iMarginRuleRepository)
        {
            _IMarginRuleRepository = iMarginRuleRepository;
        }

        public List<ResponseViewModel> Delete(int Id)
        {
            return _IMarginRuleRepository.Delete(Id);
        }
        public Tuple<List<MarginRuleViewModel>, long> GetAll(PageParam pageParam, string search)
        {
            return _IMarginRuleRepository.GetAll(pageParam, search);
        }

        public Task<MarginRuleViewModel> GetById(int Id)
        {
            return _IMarginRuleRepository.GetById(Id);
        }

        public Task<MarginRuleViewModel> Upsert(MarginRuleViewModel model)
        {
            return _IMarginRuleRepository.Upsert(model);
        }
    }
}
