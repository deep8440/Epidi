using Dapper;
using Epidi.Models.DBConnection;
using Epidi.Models.Paging;
using Epidi.Models.ViewModel.Common;
using Epidi.Models.ViewModel.MasterLP;
using Epidi.Repository.ConnectionProvider;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Repository.MasterLP
{
    public class MasterLPRepository : RepositoryBase, IMasterLPRepository
    {
        public MasterLPRepository(IOptions<ConnectionSettings> connectionOptions,
            IConnectionProvider connectionProvider) : base(connectionOptions: connectionOptions,
                connectionProvider: connectionProvider)
        {
        }
        public List<ResponseViewModel> Delete(int Id)
        {
            List<ResponseViewModel> response = new List<ResponseViewModel>();

            var vParams = new DynamicParameters();
            vParams.Add("@Id", Id);
            vParams.Add("@DeletedBy", 1);
            var res = _db.QueryMultiple("[MasterLP_Delete]", vParams, commandType: CommandType.StoredProcedure);

            response = res.Read<ResponseViewModel>().ToList();

            return response;
        }

        public Tuple<List<MasterLPViewModel>, long> GetAll(PageParam pageParam, string search)
        {
            List<MasterLPViewModel> response = new();

            var vParams = new DynamicParameters();
            vParams.Add("@Offset", pageParam.Offset, dbType: DbType.Int64, direction: ParameterDirection.Input);
            vParams.Add("@Limit", pageParam.Limit, dbType: DbType.Int64, direction: ParameterDirection.Input);
            vParams.Add("@Search", search, dbType: DbType.String, direction: ParameterDirection.Input);
            var res = _db.QueryMultiple("[MasterLP_GetAll]", vParams, commandType: CommandType.StoredProcedure);

            response.AddRange(res.Read<MasterLPViewModel>());
            long no = res.Read<long>().SingleOrDefault();
            var tuple = new Tuple<List<MasterLPViewModel>, long>(response, no);
            return tuple;
        }

        public async Task<MasterLPViewModel> GetById(int Id)
        {
            MasterLPViewModel response = new();

            var vParams = new DynamicParameters();
            vParams.Add("@Id", Id);
            var res = await _db.QueryMultipleAsync("[MasterLP_GetById]", vParams, commandType: CommandType.StoredProcedure);

            response = res.Read<MasterLPViewModel>().FirstOrDefault();

            return response;
        }

        public async Task<MasterLPViewModel> Upsert(MasterLPViewModel aMasterLP)
        {
            MasterLPViewModel response = new();

            var vParams = new DynamicParameters();
            vParams.Add("@Id", aMasterLP.Id);
            vParams.Add("@Name", aMasterLP.Name);
            vParams.Add("@HealthCheckURL", aMasterLP.HealthCheckURL);
            vParams.Add("@IsDown", aMasterLP.IsDown);
            vParams.Add("@LastHealthCheckTime", aMasterLP.LastHealthCheckTime);
            vParams.Add("@Username", aMasterLP.Username);
            vParams.Add("@Password", aMasterLP.Password);
            vParams.Add("@DNS", aMasterLP.DNS);
            vParams.Add("@APIKey", aMasterLP.APIKey);
            vParams.Add("@IsActive", aMasterLP.IsActive);
            vParams.Add("@CreatedBy", aMasterLP.CreatedBy);
            vParams.Add("@UpdatedBy", aMasterLP.UpdatedBy);
            var res = await _db.QueryMultipleAsync("[MasterLP_Upsert]", vParams, commandType: CommandType.StoredProcedure);

            response = res.Read<MasterLPViewModel>().FirstOrDefault();

            return response;
        }
    }
}
