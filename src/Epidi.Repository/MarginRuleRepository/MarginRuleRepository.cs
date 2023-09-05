using Dapper;
using Epidi.Models.DBConnection;
using Epidi.Models.Paging;
using Epidi.Models.ViewModel.Common;
using Epidi.Models.ViewModel.MarginRuleViewModel;
using Epidi.Repository.ConnectionProvider;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Repository.MarginRuleRepository
{
    public class MarginRuleRepository : RepositoryBase, IMarginRuleRepository
    {
        public MarginRuleRepository(IOptions<ConnectionSettings> connectionOptions,
            IConnectionProvider connectionProvider) : base(connectionOptions: connectionOptions,
                connectionProvider: connectionProvider)
        {
        }
        public List<ResponseViewModel> Delete(int Id)
        {
            List<ResponseViewModel> response = new List<ResponseViewModel>();

            var vParams = new DynamicParameters();
            vParams.Add("@Id", Id);
            var res = _db.QueryMultiple("[MarginRule_Delete]", vParams, commandType: CommandType.StoredProcedure);

            response = res.Read<ResponseViewModel>().ToList();

            return response;
        }

        public Tuple<List<MarginRuleViewModel>, long> GetAll(PageParam pageParam, string search)
        {
            List<MarginRuleViewModel> response = new();

            var vParams = new DynamicParameters();
            vParams.Add("@Offset", pageParam.Offset, dbType: DbType.Int64, direction: ParameterDirection.Input);
            vParams.Add("@Limit", pageParam.Limit, dbType: DbType.Int64, direction: ParameterDirection.Input);
            vParams.Add("@Search", search, dbType: DbType.String, direction: ParameterDirection.Input);
            var res = _db.QueryMultiple("[MarginRule_GetAll]", vParams, commandType: CommandType.StoredProcedure);

            response.AddRange(res.Read<MarginRuleViewModel>());
            long no = res.Read<long>().SingleOrDefault();
            var tuple = new Tuple<List<MarginRuleViewModel>, long>(response, no);
            return tuple;
        }

        public async Task<MarginRuleViewModel> GetById(int Id)
        {
            MarginRuleViewModel response = new();

            var vParams = new DynamicParameters();
            vParams.Add("@Id", Id);
            var res = await _db.QueryMultipleAsync("[MarginRule_GetById]", vParams, commandType: CommandType.StoredProcedure);

            response = res.Read<MarginRuleViewModel>().FirstOrDefault();

            return response;
        }
         
        public async Task<MarginRuleViewModel> Upsert(MarginRuleViewModel model)
        {
            MarginRuleViewModel response = new();

            var vParams = new DynamicParameters();
            vParams.Add("@MarginRuleId", model.MarginRuleId);
            vParams.Add("@RuleName", model.RuleName);            
            vParams.Add("@Priority", model.Priority);            
            vParams.Add("@StopOutPercent", model.StopOutPercent);
            vParams.Add("@MarginCallPercent", model.MarginCallPercent);
            vParams.Add("@IsPartialCloseoutonStopOut", model.IsPartialCloseoutonStopOut);
            vParams.Add("@LeveltotheStopOut", model.LeveltotheStopOut);
            vParams.Add("@CreatedBy", model.CreatedBy);
            vParams.Add("@UpdatedBy", model.UpdatedBy);
            var res = await _db.QueryMultipleAsync("[MarginRule_Upsert]", vParams, commandType: CommandType.StoredProcedure);

            response = res.Read<MarginRuleViewModel>().FirstOrDefault();

            return response;
        }
    }
}
