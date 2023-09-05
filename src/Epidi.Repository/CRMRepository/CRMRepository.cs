using Dapper;
using Epidi.Models.DBConnection;
using Epidi.Models.Paging;
using Epidi.Models.ViewModel.Users;
using Epidi.Repository.ConnectionProvider;
using Epidi.Repository.UsersRepository;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Repository.CRMRepository
{
    public class CRMRepository : RepositoryBase, ICRMRepository
    {
        public CRMRepository(IOptions<ConnectionSettings> connectionOptions,
            IConnectionProvider connectionProvider) : base(connectionOptions: connectionOptions,
                connectionProvider: connectionProvider)
        {
        }

        public Tuple<List<UsersViewModel>, long> Users_GetAll(PageParam pageParam, string search)
        {
            List<UsersViewModel> response = new List<UsersViewModel>();

            var vParams = new DynamicParameters();
            vParams.Add("@Offset", pageParam.Offset, dbType: DbType.Int64, direction: ParameterDirection.Input);
            vParams.Add("@Limit", pageParam.Limit, dbType: DbType.Int64, direction: ParameterDirection.Input);
            vParams.Add("@Search", search, dbType: DbType.String, direction: ParameterDirection.Input);

            var res = _db.QueryMultiple("[CRM_GetAll]", vParams, commandType: CommandType.StoredProcedure);

            response.AddRange(res.Read<UsersViewModel>());
            long no = res.Read<long>().SingleOrDefault();
            var tuple = new Tuple<List<UsersViewModel>, long>(response, no);
            return tuple;
        }
    }
}
