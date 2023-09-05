using Dapper;
using Epidi.Models.DBConnection;
using Epidi.Models.Paging;
using Epidi.Models.ViewModel.Common;
using Epidi.Models.ViewModel.RuleLpPriority;
using Epidi.Repository.ConnectionProvider;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Repository.RuleLpPriorityRepository
{
    public class RuleLpPriorityRepository : RepositoryBase, IRuleLpPriorityRepository
    {
        public RuleLpPriorityRepository(IOptions<ConnectionSettings> connectionOptions, 
            IConnectionProvider connectionProvider) : base(connectionOptions : connectionOptions, 
                connectionProvider: connectionProvider)
        {
        }

        public List<ResponseViewModel> Delete(int Id)
        {
            List<ResponseViewModel> response = new List<ResponseViewModel>();

            var vParams = new DynamicParameters();
            vParams.Add("@Id", Id);
            var res = _db.QueryMultiple("[RuleLpPriority_Delete]", vParams, commandType: CommandType.StoredProcedure);

            response = res.Read<ResponseViewModel>().ToList();

            return response;
        }

        public Tuple<List<RuleLpPriorityViewModel>, long> GetAll(PageParam pageParam, string search)
        {
            List<RuleLpPriorityViewModel> response = new();

            var vParams = new DynamicParameters();
            vParams.Add("@Offset", pageParam.Offset, dbType: DbType.Int64, direction: ParameterDirection.Input);
            vParams.Add("@Limit", pageParam.Limit, dbType: DbType.Int64, direction: ParameterDirection.Input);
            vParams.Add("@Search", search, dbType: DbType.String, direction: ParameterDirection.Input);
            var res = _db.QueryMultiple("[RuleLpPriority_GetAll]", vParams, commandType: CommandType.StoredProcedure);

            response.AddRange(res.Read<RuleLpPriorityViewModel>());
            long no = res.Read<long>().SingleOrDefault();
            var tuple = new Tuple<List<RuleLpPriorityViewModel>, long>(response, no);
            return tuple;
        }

        public async Task<RuleLpPriorityViewModel> GetById(int Id)
        {
            RuleLpPriorityViewModel response = new();

            var vParams = new DynamicParameters();
            vParams.Add("@Id", Id);
            var res = await _db.QueryMultipleAsync("[RuleLpPriority_GetById]", vParams, commandType: CommandType.StoredProcedure);

            response = res.Read<RuleLpPriorityViewModel>().FirstOrDefault();

            return response;
        }
        public async Task<List<RuleLpPriorityViewModel>> GetByRuleId(int Id)
        {
            List<RuleLpPriorityViewModel> response = new();

            var vParams = new DynamicParameters();
            vParams.Add("@Id", Id);
            var res = await _db.QueryMultipleAsync("[RuleLpPriority_GetByRuleId]", vParams, commandType: CommandType.StoredProcedure);

            response = res.Read<RuleLpPriorityViewModel>().ToList();

            return response;
        }
        public async Task<RuleLpPriorityViewModel> Upsert(RuleLpPriorityViewModel aRuleLpPriority)
        {
            RuleLpPriorityViewModel response = new();

            var vParams = new DynamicParameters();
            vParams.Add("@Id", aRuleLpPriority.Id);
            vParams.Add("@RuleId", aRuleLpPriority.RuleId);
            vParams.Add("@LPId", aRuleLpPriority.LPId);
            vParams.Add("@Priority", aRuleLpPriority.Priority);
            vParams.Add("@MarkUpValue", aRuleLpPriority.MarkUpValue);
            vParams.Add("@CreatedBy", aRuleLpPriority.CreatedBy);
            vParams.Add("@UpdatedBy", aRuleLpPriority.UpdatedBy);
            var res = await _db.QueryMultipleAsync("[RuleLpPriority_Upsert]", vParams, commandType: CommandType.StoredProcedure);

            response = res.Read<RuleLpPriorityViewModel>().FirstOrDefault();

            return response;
        }
    }
}
