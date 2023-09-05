using Epidi.Models.Paging;
using Epidi.Models.ViewModel.Users;
using Epidi.Models.ViewModel.UsersTransactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Repository.UsersTransactionsRepository
{
    public interface IUsersTransactionsRepository
    {
        Task<UsersTransactionViewModel> Upsert(UsersTransactionViewModel model);
        Tuple<List<UsersTransactionViewModel>, long> Get_All_Users_Transaction(PageParam pageParam, string search, string userId, string fromDate, string toDate,string type);
        List<UsersViewModel> UserTransaction_Users_GetAll();
	}
}
