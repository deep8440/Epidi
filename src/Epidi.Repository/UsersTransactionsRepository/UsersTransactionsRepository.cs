using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Epidi.Models.DBConnection;
using Epidi.Repository.ConnectionProvider;
using Microsoft.Extensions.Options;
using Dapper;
using Epidi.Models.ViewModel.Users;
using System.Data;
using Epidi.Models.ViewModel.UsersTransactions;
using Epidi.Models.Paging;
using Epidi.Models.ViewModel.Question;

namespace Epidi.Repository.UsersTransactionsRepository
{
    public class UsersTransactionsRepository : RepositoryBase, IUsersTransactionsRepository
    {
        public UsersTransactionsRepository(IOptions<ConnectionSettings> connectionOptions,
           IConnectionProvider connectionProvider) : base(connectionOptions: connectionOptions,
               connectionProvider: connectionProvider)
        {
        }
        // <summary>
        /// Saves a record to the UsersTransaction table.
        /// returns INSERTED values saved successfully from TABLE
        /// </summary>
        public async Task<UsersTransactionViewModel> Upsert(UsersTransactionViewModel model)
        {
            UsersTransactionViewModel response = new();

            var vParams = new DynamicParameters();
            vParams.Add("@Id", model.Id);
            vParams.Add("@UserId", model.UserId);
            vParams.Add("@Type", model.Type);            
            vParams.Add("@CreatedBy", model.CreatedBy);
            vParams.Add("@Comment", model.Comment);
            vParams.Add("@Amount", model.Amount);

            var res = await _db.QueryMultipleAsync("[UsersTransactions_Upsert]", vParams, commandType: CommandType.StoredProcedure);

            response = res.Read<UsersTransactionViewModel>().FirstOrDefault();
            return response;
        }

		public Tuple<List<UsersTransactionViewModel>, long> Get_All_Users_Transaction(PageParam pageParam, string search, string userId,string fromDate,string toDate,string type)
		{
			List<UsersTransactionViewModel> usersTransactionViewModels = new List<UsersTransactionViewModel>();
			var param = new DynamicParameters();
			param.Add("@Offset", pageParam.Offset, dbType: DbType.Int64, direction: ParameterDirection.Input);
			param.Add("@Limit", pageParam.Limit, dbType: DbType.Int64, direction: ParameterDirection.Input);
			param.Add("@Search", search, dbType: DbType.String, direction: ParameterDirection.Input);
			param.Add("@FromDate", fromDate, dbType: DbType.String, direction: ParameterDirection.Input);
			param.Add("@ToDate", toDate, dbType: DbType.String, direction: ParameterDirection.Input);
			param.Add("@UserId", userId, dbType: DbType.String, direction: ParameterDirection.Input);
			param.Add("@Type", type, dbType: DbType.String, direction: ParameterDirection.Input);

			var res = _db.QueryMultiple("[UsersTransaction_GetAll]", param, commandType: CommandType.StoredProcedure);

			usersTransactionViewModels.AddRange(res.Read<UsersTransactionViewModel>());
			long no = res.Read<long>().SingleOrDefault();
			var tuple = new Tuple<List<UsersTransactionViewModel>, long>(usersTransactionViewModels, no);
			return tuple;
		}

		public List<UsersViewModel> UserTransaction_Users_GetAll()
		{
			List<UsersViewModel> response = new List<UsersViewModel>();

			var vParams = new DynamicParameters();
			vParams.Add("@Search", "");

			var res = _db.QueryMultiple("[UserTransactions_Users_GetAll]", vParams, commandType: CommandType.StoredProcedure);

			response = res.Read<UsersViewModel>().ToList();

			return response;
		}

	}
}
