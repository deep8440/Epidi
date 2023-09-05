using Dapper;
using Epidi.Models.DBConnection;
using Epidi.Models.Paging;
using Epidi.Models.ViewModel.Common;
using Epidi.Models.ViewModel.QuoteMarkUpRule;
using Epidi.Repository.ConnectionProvider;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Repository.QuoteMarkUpRuleRepository
{
    public class QuoteMarkUpRuleRepository : RepositoryBase, IQuoteMarkUpRuleRepository
    {
        public QuoteMarkUpRuleRepository(IOptions<ConnectionSettings> connectionOptions,
            IConnectionProvider connectionProvider) : base(connectionOptions: connectionOptions,
                connectionProvider: connectionProvider)
        {
        }
        public List<ResponseViewModel> Delete(int Id)
        {
            List<ResponseViewModel> response = new List<ResponseViewModel>();

            var vParams = new DynamicParameters();
            vParams.Add("@Id", Id);
            var res = _db.QueryMultiple("[QuoteMarkUpRule_Delete]", vParams, commandType: CommandType.StoredProcedure);

            response = res.Read<ResponseViewModel>().ToList();

            return response;
        }

        public Tuple<List<QuoteMarkUpRuleViewModel>, long> GetAll(PageParam pageParam, string search)
        {
            List<QuoteMarkUpRuleViewModel> response = new();

            var vParams = new DynamicParameters();
            vParams.Add("@Offset", pageParam.Offset, dbType: DbType.Int64, direction: ParameterDirection.Input);
            vParams.Add("@Limit", pageParam.Limit, dbType: DbType.Int64, direction: ParameterDirection.Input);
            vParams.Add("@Search", search, dbType: DbType.String, direction: ParameterDirection.Input);
            var res = _db.QueryMultiple("[QuoteMarkUpRule_GetAll]", vParams, commandType: CommandType.StoredProcedure);

            response.AddRange(res.Read<QuoteMarkUpRuleViewModel>());
            long no = res.Read<long>().SingleOrDefault();
            var tuple = new Tuple<List<QuoteMarkUpRuleViewModel>, long>(response, no);
            return tuple;
        }

        public async Task<QuoteMarkUpRuleViewModel> GetById(int Id)
        {
            QuoteMarkUpRuleViewModel response = new();

            var vParams = new DynamicParameters();
            vParams.Add("@Id", Id);
            var res = await _db.QueryMultipleAsync("[QuoteMarkUpRule_GetById]", vParams, commandType: CommandType.StoredProcedure);

            response = res.Read<QuoteMarkUpRuleViewModel>().FirstOrDefault();

            return response;
        }
        public async Task<ResponseViewModel> CopyRule(int Id)
        {
            ResponseViewModel response = new ResponseViewModel();
            var vParams = new DynamicParameters();
            vParams.Add("@Id", Id);
            var res = _db.QueryMultiple("[QuoteMarkUpRule_Copy]", vParams, commandType: CommandType.StoredProcedure);

            response = res.Read<ResponseViewModel>().FirstOrDefault();

            return response;
        }

        public async Task<QuoteMarkUpRuleViewModel> Upsert(QuoteMarkUpRuleViewModel model)
        {
            QuoteMarkUpRuleViewModel response = new();

            var vParams = new DynamicParameters();
            vParams.Add("@Id", model.Id);
            vParams.Add("@Name", model.Name);            
            vParams.Add("@AutoupdateLPPriorityUp", model.AutoupdateLPPriorityUp);
            vParams.Add("@Priority", model.Priority);
            vParams.Add("@CreatedBy", model.CreatedBy);
            vParams.Add("@UpdatedBy", model.UpdatedBy);
            var res = await _db.QueryMultipleAsync("[QuoteMarkUpRule_Upsert]", vParams, commandType: CommandType.StoredProcedure);

            response = res.Read<QuoteMarkUpRuleViewModel>().FirstOrDefault();

            return response;
        }

        public async Task<long> QuoteMarkUpRule_InsertByImport(DataTable dataTable,int RuleId)
        {
            long roleId = 0;
            try
            {
                SqlConnection con = new SqlConnection(_connectionOptions.ConnectionString);
                await con.OpenAsync();

                SqlCommand sqlCommand = new SqlCommand("QuoteMarkUpRule_InsertByImport", con);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandTimeout = 900;

                SqlParameter sqlParam = sqlCommand.Parameters.AddWithValue("@paramTable", dataTable);
                SqlParameter sqlParam1 = sqlCommand.Parameters.AddWithValue("@RuleId", RuleId);
                sqlParam.SqlDbType = SqlDbType.Structured;

                roleId = Convert.ToInt32(await sqlCommand.ExecuteScalarAsync());

                await con.CloseAsync();
                return roleId;
            }
            catch (Exception EX)
            {
                throw;
            }
        }
        public async Task<long> QuoteMarkUpRule_UpdateByDt(DataTable dtRuleInstrument,DataTable dtRuleLpPriority,DataTable dtRuleConditionsDtl)
        {
            long roleId = 0;
            try
            {
                SqlConnection con = new SqlConnection(_connectionOptions.ConnectionString);
                await con.OpenAsync();

                SqlCommand sqlCommand = new SqlCommand("RuleInstrument_InsertByDt", con);
                sqlCommand.CommandType = CommandType.StoredProcedure;

                SqlParameter sqlParam = sqlCommand.Parameters.AddWithValue("@paramTable", dtRuleInstrument);
                sqlParam.SqlDbType = SqlDbType.Structured;

                roleId = Convert.ToInt32(await sqlCommand.ExecuteScalarAsync());


                //sqlCommand = new SqlCommand("RuleLpPriority_InsertByDt", con);
                //sqlCommand.CommandType = CommandType.StoredProcedure;
                //sqlParam = sqlCommand.Parameters.AddWithValue("@paramTable", dtRuleLpPriority);
                //sqlParam.SqlDbType = SqlDbType.Structured;

                //roleId = Convert.ToInt32(await sqlCommand.ExecuteScalarAsync());

                //sqlCommand = new SqlCommand("RuleLpPriority_InsertByDt", con);
                //sqlCommand.CommandType = CommandType.StoredProcedure;
                //sqlParam = sqlCommand.Parameters.AddWithValue("@paramTable", dtRuleLpPriority);
                //sqlParam.SqlDbType = SqlDbType.Structured;

                //roleId = Convert.ToInt32(await sqlCommand.ExecuteScalarAsync());

                await con.CloseAsync();
                return roleId;
            }
            catch (Exception ex)
            {
                throw;
                return 0;
            }
        }
    }
}
