using Epidi.Models.Paging;
using Epidi.Models.ViewModel.Users;
using Epidi.Repository.CRMRepository;
using Epidi.Repository.UsersRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Service.CRMService
{
    public class CRMService : ICRMService
    {
        private readonly ICRMRepository _crmRepository;
        public CRMService(ICRMRepository crmRepository)
        {
            _crmRepository = crmRepository;
        }
        public Tuple<List<UsersViewModel>, long> Users_GetAll(PageParam pageParam, string search)
        {
            return _crmRepository.Users_GetAll(pageParam, search);
        }
    }
}
