using Dapper;
using Epidi.Models.DBConnection;
using Epidi.Models.Paging;
using Epidi.Models.ViewModel.CommissionRule;
using Epidi.Models.ViewModel.Common;
using Epidi.Models.ViewModel.TradeRule;
using Epidi.Repository.ConnectionProvider;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Repository.TradeRuleRepository
{
    public class TradeRuleRepository : RepositoryBase, ITradeRuleRepository
    {
        public TradeRuleRepository(IOptions<ConnectionSettings> connectionOptions, 
            IConnectionProvider connectionProvider) : 
            base(connectionOptions, connectionProvider)
        {
        }
        public Tuple<List<TradeRuleViewModel>, long> GetAllTradeRule(PageParam pageParam, string search)
        {
            List<TradeRuleViewModel> response = new();

            var vParams = new DynamicParameters();
            vParams.Add("@Offset", pageParam.Offset, dbType: DbType.Int64, direction: ParameterDirection.Input);
            vParams.Add("@Limit", pageParam.Limit, dbType: DbType.Int64, direction: ParameterDirection.Input);
            vParams.Add("@Search", search, dbType: DbType.String, direction: ParameterDirection.Input);
            var res = _db.QueryMultiple("[TradeRule_GetAll]", vParams, commandType: CommandType.StoredProcedure);

            response.AddRange(res.Read<TradeRuleViewModel>());
            long no = res.Read<long>().SingleOrDefault();
            var tuple = new Tuple<List<TradeRuleViewModel>, long>(response, no);
            return tuple;
        }
        public Tuple<List<TradeStatusViewModel>, long> GetAllTradeStatus(PageParam pageParam, string search)
        {
            List<TradeStatusViewModel> response = new();

            var vParams = new DynamicParameters();
            vParams.Add("@Offset", pageParam.Offset, dbType: DbType.Int64, direction: ParameterDirection.Input);
            vParams.Add("@Limit", pageParam.Limit, dbType: DbType.Int64, direction: ParameterDirection.Input);
            vParams.Add("@Search", search, dbType: DbType.String, direction: ParameterDirection.Input);
            var res = _db.QueryMultiple("[TradeRuleStatus_GetAll]", vParams, commandType: CommandType.StoredProcedure);

            response.AddRange(res.Read<TradeStatusViewModel>());
            long no = res.Read<long>().SingleOrDefault();
            var tuple = new Tuple<List<TradeStatusViewModel>, long>(response, no);
            return tuple;
        }
        public async Task<long> TradeRule_InsertByImport(DataTable dataTable, int tradeRuleId, string tradeRuleName, int tradeRulePriority)
        {
            long roleId = 0;
            try
            {
                SqlConnection con = new SqlConnection(_connectionOptions.ConnectionString);
                await con.OpenAsync();

                SqlCommand sqlCommand = new SqlCommand("TradeRule_InsertBy_Import", con);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandTimeout = 3600;

                var vParams = new DynamicParameters();

                SqlParameter sqlParam = sqlCommand.Parameters.AddWithValue("@paramTable", dataTable);
                SqlParameter sqlParam1 = sqlCommand.Parameters.AddWithValue("@tradeRuleId", tradeRuleId);
                SqlParameter sqlParam2 = sqlCommand.Parameters.AddWithValue("@tradeRuleName", tradeRuleName);
                SqlParameter sqlParam3 = sqlCommand.Parameters.AddWithValue("@tradeRulePriority", tradeRulePriority);
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
        public async Task<TradeRuleViewModel> GetById(int Id)
        {
            TradeRuleViewModel response = new();

            var vParams = new DynamicParameters();
            vParams.Add("@Id", Id);
            var res = await _db.QueryMultipleAsync("[TradeRule_GetById]", vParams, commandType: CommandType.StoredProcedure);

            response = res.Read<TradeRuleViewModel>().FirstOrDefault();

            return response;
        }
        //public async Task<List<TradeRuleInstrumentViewModelGet>> GetByInstrumentTradeRuleByRuleId(int RuleId, int lp_filter = 0,
        //    int instrument_filter = 0, int day_filter = 0)
        //{
        //    try
        //    {
        //        List<TradeRuleInstrumentViewModelGet> response = new();

        //        var vParams = new DynamicParameters();
        //        vParams.Add("@RuleId", RuleId);
        //        vParams.Add("@lp_filter", lp_filter);
        //        vParams.Add("@instrument_filter", instrument_filter);
        //        vParams.Add("@day_filter", day_filter);
        //        var res = await _db.QueryMultipleAsync("[RuleInstrumentTradeRule_GetByRuleId]", vParams, commandType: CommandType.StoredProcedure);

        //        response = res.Read<TradeRuleInstrumentViewModelGet>().ToList();

        //        return response;
        //    }
        //    catch (Exception ex)
        //    {

        //        throw;
        //    }
        //}

        public async Task<Tuple<List<TradeRuleInstrumentViewModelGet>, long>> GetByInstrumentTradeRuleByRuleId(PageParam pageParam, string search, int Id,int InstrumentId,int LpId,string day)
        {
            List<TradeRuleInstrumentViewModelGet> tradeRuleInstrumentViewModelGet = new List<TradeRuleInstrumentViewModelGet>();
            var param = new DynamicParameters();
            param.Add("@RuleId", Id, dbType: DbType.Int64, direction: ParameterDirection.Input);
            param.Add("@Offset", pageParam.Offset, dbType: DbType.Int64, direction: ParameterDirection.Input);
            param.Add("@Limit", pageParam.Limit, dbType: DbType.Int64, direction: ParameterDirection.Input);
            param.Add("@Search", search, dbType: DbType.String, direction: ParameterDirection.Input);
            param.Add("@InstrumentId", InstrumentId, dbType: DbType.Int64, direction: ParameterDirection.Input);
            param.Add("@LpId", LpId, dbType: DbType.Int64, direction: ParameterDirection.Input);
            param.Add("@Day", day, dbType: DbType.Int64, direction: ParameterDirection.Input);

            var res = _db.QueryMultiple("[dbo].[RuleInstrumentTradeRule_GetByRuleId]", param, commandType: CommandType.StoredProcedure);

            tradeRuleInstrumentViewModelGet.AddRange(res.Read<TradeRuleInstrumentViewModelGet>());
            long no = res.Read<long>().SingleOrDefault();
            var tuple = new Tuple<List<TradeRuleInstrumentViewModelGet>, long>(tradeRuleInstrumentViewModelGet, no);
            return tuple;
        }
        public async Task<TradeRuleViewModel> Upsert(TradeRuleViewModel model)
        {
            TradeRuleViewModel response = new();

            var vParams = new DynamicParameters();
            vParams.Add("@Id", model.Id);
            vParams.Add("@Name", model.Name);
            vParams.Add("@Priority", model.Priority);
            vParams.Add("@CreatedBy", model.CreatedBy);
            vParams.Add("@UpdatedBy", model.UpdatedBy);
            var res = await _db.QueryMultipleAsync("[TradeRule_Upsert]", vParams, commandType: CommandType.StoredProcedure);

            response = res.Read<TradeRuleViewModel>().FirstOrDefault();

            return response;
        }
        public async Task<long> TradeRule_UpdateByDt(DataTable dtRuleInstrument)
        {
            long roleId = 0;
            try
            {
                SqlConnection con = new SqlConnection(_connectionOptions.ConnectionString);
                await con.OpenAsync();

                SqlCommand sqlCommand = new SqlCommand("TradeRuleInstrument_InsertByDt", con);
                sqlCommand.CommandType = CommandType.StoredProcedure;

                SqlParameter sqlParam = sqlCommand.Parameters.AddWithValue("@paramTable", dtRuleInstrument);
                sqlParam.SqlDbType = SqlDbType.Structured;

                roleId = Convert.ToInt32(await sqlCommand.ExecuteScalarAsync());

                await con.CloseAsync();
                return roleId;
            }
            catch (Exception ex)
            {
                throw;
                return 0;
            }
        }
        public async Task<List<TradeRuleUniversalValues>> Get_TradeUniversalValues()
        {
            try
            {
                List<TradeRuleUniversalValues> response = new();
                var res = await _db.QueryMultipleAsync("[TradeRuleUniversalValues_GetAllActive]", null, commandType: CommandType.StoredProcedure);

                response = res.Read<TradeRuleUniversalValues>().ToList();
                return response;
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        public async Task<long> DeleteTradeRuleInstrumentPriority(int RuleId)
        {
            try
            {
                var vParams = new DynamicParameters();
                vParams.Add("@RuleId", RuleId);                
                var res = await _db.QueryMultipleAsync("[TradeRuleInstrument_Priority_DeleteByRuleId]", vParams, commandType: CommandType.StoredProcedure);
                return 1;
            }
            catch (Exception ex)
            {
                throw;
                return 0;
            }
        }
        public async Task<bool> CheckTradeRuleUniversalValues(string Values)
        {
            bool isAvailable  ;
            try
            {
                SqlConnection con = new SqlConnection(_connectionOptions.ConnectionString);
                await con.OpenAsync();

                SqlCommand sqlCommand = new SqlCommand("Check_TradeRuleUniversalValues", con);
                sqlCommand.CommandType = CommandType.StoredProcedure;

                SqlParameter sqlParam = sqlCommand.Parameters.AddWithValue("@Vaules", Values);
                sqlParam.SqlDbType = SqlDbType.VarChar;

                isAvailable = Convert.ToBoolean(await sqlCommand.ExecuteScalarAsync());

                await con.CloseAsync();
                return isAvailable;
            }
            catch (Exception EX)
            {
                throw;
            }
        }
        public async Task<ResponseViewModel> CopyTradeRule(int Id)
        {
            ResponseViewModel response = new ResponseViewModel();
            var vParams = new DynamicParameters();
            vParams.Add("@Id", Id);
            var res = _db.QueryMultiple("[TradeRuleCopy]", vParams, commandType: CommandType.StoredProcedure);

            response = res.Read<ResponseViewModel>().FirstOrDefault();

            return response;
        }
        public async Task<int> DeleteTradeRuleInstrumentById(int RuleId, int RulePriorityId)
        {
            var vParams = new DynamicParameters();
            vParams.Add("@RuleId", RuleId);
            vParams.Add("@RulePriorityId", RulePriorityId);
            var res = await _db.QueryMultipleAsync("[Trade_RuleInstrument_Delete]", vParams, commandType: CommandType.StoredProcedure);

            return 1;
        }
        public List<ResponseViewModel> Delete(int Id)
        {

            List<ResponseViewModel> response = new List<ResponseViewModel>();

            var param = new DynamicParameters();
            param.Add("@Id", Id, dbType: DbType.Int32, direction: ParameterDirection.Input);

            var res = _db.QueryMultiple("[dbo].[TradeRule_Delete]", param, commandType: CommandType.StoredProcedure);


            response = res.Read<ResponseViewModel>().ToList();

            return response;
        }


    }
}
