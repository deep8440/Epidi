using Epidi.Models.Paging;
using Epidi.Models.ViewModel.Country;
using Epidi.Models.ViewModel.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Repository.CRMRepository
{
    public interface ICRMRepository
    {
        Tuple<List<UsersViewModel>, long> Users_GetAll(PageParam pageParam, string search);
    }
}
