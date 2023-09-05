using Epidi.Models.Paging;
using Epidi.Models.ViewModel.Users;
using Epidi.Models.ViewModel.UsersTransactions;
using Epidi.Repository.UsersTransactionsRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Epidi.Service.UsersTransactionsService
{
    public class UsersTransactionsService : IUsersTransactionsService
    {
        private readonly IUsersTransactionsRepository _usersTransactionsRepository;

        public UsersTransactionsService(IUsersTransactionsRepository usersTransactionsRepository)
        {
            _usersTransactionsRepository = usersTransactionsRepository;
        }

        public Task<UsersTransactionViewModel> Upsert(UsersTransactionViewModel model)
        {
            return _usersTransactionsRepository.Upsert(model);
        }

        public Tuple<List<UsersTransactionViewModel>, long> Get_All_Users_Transaction(PageParam pageParam, string search, string userId, string fromDate, string toDate,string type)
        {
			return _usersTransactionsRepository.Get_All_Users_Transaction(pageParam,search,userId,fromDate,toDate, type);
		}

        public List<UsersViewModel> UserTransaction_Users_GetAll()
        {
			return _usersTransactionsRepository.UserTransaction_Users_GetAll();
		}

	}
}
