using Dapper;
using Epidi.Models.DBConnection;
using Epidi.Models.Model;
using Epidi.Models.Paging;
using Epidi.Models.ViewModel.CommissionRule;
using Epidi.Models.ViewModel.Common;
using Epidi.Models.ViewModel.QuoteMarkUpRule;
using Epidi.Models.ViewModel.Steps;
using Epidi.Models.ViewModel.SwapRule;
using Epidi.Repository.ConnectionProvider;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Repository.SwapRuleRepository
{
    public class SwapRuleRepository : RepositoryBase, ISwapRuleRepository
    {
        public SwapRuleRepository(IOptions<ConnectionSettings> connectionOptions,
            IConnectionProvider connectionProvider
           ) : base(connectionOptions: connectionOptions,
                connectionProvider: connectionProvider)
        {
        }

        public async Task<Tuple<List<SwapRuleViewModel>, long>> GetAllSwapRules(PageParam pageParam, string search)
        {
            List<SwapRuleViewModel> swapRuleViewModels = new List<SwapRuleViewModel>();
            var param = new DynamicParameters();
            param.Add("@Offset", pageParam.Offset, dbType: DbType.Int64, direction: ParameterDirection.Input);
            param.Add("@Limit", pageParam.Limit, dbType: DbType.Int64, direction: ParameterDirection.Input);
            param.Add("@Search", search, dbType: DbType.String, direction: ParameterDirection.Input);

            var res = _db.QueryMultiple("[dbo].[SwapRule_GetAll]", param, commandType: CommandType.StoredProcedure);

            swapRuleViewModels.AddRange(res.Read<SwapRuleViewModel>());
            long no = res.Read<long>().SingleOrDefault();
            var tuple = new Tuple<List<SwapRuleViewModel>, long>(swapRuleViewModels, no);
            return tuple;
        }
        public async Task<long> AddSwapRule(DataTable dataTable, SwapRuleViewModel model)
        {
            long ruleId = 0;
            SqlConnection con = new SqlConnection(_connectionOptions.ConnectionString);
            await con.OpenAsync();

            SqlCommand sqlCommand = new SqlCommand("SwapRule_Add", con);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandTimeout = 1200;
            SqlParameter sqlParam1 = sqlCommand.Parameters.AddWithValue("@Id", model.Id);
            SqlParameter sqlParam5 = sqlCommand.Parameters.AddWithValue("@RuleName", model.RuleName);
            SqlParameter sqlParam2 = sqlCommand.Parameters.AddWithValue("@Comment", model.Comment);
            SqlParameter sqlParam3 = sqlCommand.Parameters.AddWithValue("@Priority", model.Priority);
            SqlParameter sqlParam4 = sqlCommand.Parameters.AddWithValue("@TimeToApply", model.TimeToApply);
            SqlParameter sqlParam = sqlCommand.Parameters.AddWithValue("@paramTable", dataTable);
            sqlParam.SqlDbType = SqlDbType.Structured;

            ruleId = Convert.ToInt32(await sqlCommand.ExecuteScalarAsync());

            //await sqlCommand.ExecuteNonQueryAsync();

            await con.CloseAsync();
            return ruleId;
        }
        public async Task<long> UpdateSwapRuleInstrument(DataTable dataTable, SwapRuleViewModel model)
        {
            long ruleId = 0;
            SqlConnection con = new SqlConnection(_connectionOptions.ConnectionString);
            await con.OpenAsync();

            SqlCommand sqlCommand = new SqlCommand("SwapRuleInstrument_Update", con);
            sqlCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter sqlParam1 = sqlCommand.Parameters.AddWithValue("@Id", model.Id);
            SqlParameter sqlParam5 = sqlCommand.Parameters.AddWithValue("@RuleName", model.RuleName);
            SqlParameter sqlParam2 = sqlCommand.Parameters.AddWithValue("@Comment", model.Comment);
            SqlParameter sqlParam3 = sqlCommand.Parameters.AddWithValue("@Priority", model.Priority);
            SqlParameter sqlParam4 = sqlCommand.Parameters.AddWithValue("@TimeToApply", model.TimeToApply);
            SqlParameter sqlParam = sqlCommand.Parameters.AddWithValue("@paramTable", dataTable);
            sqlParam.SqlDbType = SqlDbType.Structured;

            ruleId = Convert.ToInt32(await sqlCommand.ExecuteScalarAsync());

            //await sqlCommand.ExecuteNonQueryAsync();

            await con.CloseAsync();
            return ruleId;
        }
        public async Task<SwapRuleViewModel> GetSwapRuleById(int Id)
        {
            SwapRuleViewModel response = new();

            var vParams = new DynamicParameters();
            vParams.Add("@RuleId", Id);
            var res = await _db.QueryMultipleAsync("[GET_SwapRule_ById]", vParams, commandType: CommandType.StoredProcedure);

            response = res.Read<SwapRuleViewModel>().FirstOrDefault();

            return response;
        }

        public async Task<Tuple<List<SwapRuleInstrumentViewModel>, long>> GetSwapRuleInstrumentById(PageParam pageParam, string search, int Id)
        {
            List<SwapRuleInstrumentViewModel> response = new List<SwapRuleInstrumentViewModel>();

            var vParams = new DynamicParameters();
            vParams.Add("@RuleId", Id);

            vParams.Add("@Offset", pageParam.Offset, dbType: DbType.Int64, direction: ParameterDirection.Input);
            vParams.Add("@Limit", pageParam.Limit, dbType: DbType.Int64, direction: ParameterDirection.Input);
            vParams.Add("@Search", search, dbType: DbType.String, direction: ParameterDirection.Input);


            var res = await _db.QueryMultipleAsync("[GET_SwapRule_Instrument_ById]", vParams, commandType: CommandType.StoredProcedure);

            response.AddRange(res.Read<SwapRuleInstrumentViewModel>());
            long no = res.Read<long>().SingleOrDefault();
            var tuple = new Tuple<List<SwapRuleInstrumentViewModel>, long>(response, no);
            return tuple;


        }

        public async Task<int> DeleteSwapRuleInstrumentById(int Id, int SwapRuleId)
        {
            var vParams = new DynamicParameters();
            vParams.Add("@Id", Id);
            // vParams.Add("@instrumentId", instrumentId);
            var res = await _db.QueryMultipleAsync("[Swap_RuleInstrument_Delete]", vParams, commandType: CommandType.StoredProcedure);

            return 1;
        }

        public async Task<int> DeleteSwapRuleById(int SwapRuleId)
        {
            var vParams = new DynamicParameters();
            vParams.Add("@RuleId", SwapRuleId);
            var res = await _db.QueryMultipleAsync("[Delete_SwapRule_ById]", vParams, commandType: CommandType.StoredProcedure);

            return 1;
        }
        public async Task<ResponseViewModel> CopyRule(int Id)
        {
            ResponseViewModel response = new ResponseViewModel();
            var vParams = new DynamicParameters();
            vParams.Add("@Id", Id);
            var res = _db.QueryMultiple("[SwapeRule_Copy]", vParams, commandType: CommandType.StoredProcedure);

            response = res.Read<ResponseViewModel>().FirstOrDefault();

            return response;
        }

        public async Task<long> AddSwapRuleNew(DataTable dataTable, SwapRuleViewModel model)
        {
            long ruleId = 0;
            SqlConnection con = new SqlConnection(_connectionOptions.ConnectionString);
            await con.OpenAsync();

            SqlCommand sqlCommand = new SqlCommand("[SwapRuleInstrument_InsertNew]", con);
            sqlCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter sqlParam1 = sqlCommand.Parameters.AddWithValue("@Id", model.Id);
            SqlParameter sqlParam5 = sqlCommand.Parameters.AddWithValue("@RuleName", model.RuleName);
            SqlParameter sqlParam2 = sqlCommand.Parameters.AddWithValue("@Comment", model.Comment);
            SqlParameter sqlParam3 = sqlCommand.Parameters.AddWithValue("@Priority", model.Priority);
            SqlParameter sqlParam4 = sqlCommand.Parameters.AddWithValue("@TimeToApply", model.TimeToApply);
            SqlParameter sqlParam = sqlCommand.Parameters.AddWithValue("@paramTable", dataTable);
            sqlParam.SqlDbType = SqlDbType.Structured;

            ruleId = Convert.ToInt32(await sqlCommand.ExecuteScalarAsync());

            //await sqlCommand.ExecuteNonQueryAsync();

            await con.CloseAsync();
            return ruleId;
        }


    }

    }
