using Dapper;
using Epidi.Models.DBConnection;
using Epidi.Models.Paging;
using Epidi.Models.ViewModel.SwapRule;
using Epidi.Repository.ConnectionProvider;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Epidi.Models.ViewModel.CommissionRule;
using Epidi.Models.ViewModel.Country;

namespace Epidi.Repository.CommissionRuleRepository
{
    public class CommissionRuleRepository : RepositoryBase, ICommissionRuleRepository
    {
        public CommissionRuleRepository(IOptions<ConnectionSettings> connectionOptions,
            IConnectionProvider connectionProvider
             ) : base(connectionOptions: connectionOptions,
        connectionProvider: connectionProvider)
        {
        }

        public async Task<Tuple<List<CommissionRuleViewModel>, long>> GetAllCommissionRules(PageParam pageParam, string search)
        {
            List<CommissionRuleViewModel> commissionRuleViewModels = new List<CommissionRuleViewModel>();
            var param = new DynamicParameters();
            param.Add("@Offset", pageParam.Offset, dbType: DbType.Int64, direction: ParameterDirection.Input);
            param.Add("@Limit", pageParam.Limit, dbType: DbType.Int64, direction: ParameterDirection.Input);
            param.Add("@Search", search, dbType: DbType.String, direction: ParameterDirection.Input);

            var res = _db.QueryMultiple("[dbo].[CommissionRule_GetAll]", param, commandType: CommandType.StoredProcedure);

            commissionRuleViewModels.AddRange(res.Read<CommissionRuleViewModel>());
            long no = res.Read<long>().SingleOrDefault();
            var tuple = new Tuple<List<CommissionRuleViewModel>, long>(commissionRuleViewModels, no);
            return tuple;
        }
        public async Task<long> AddCommissionRule(DataTable dataTable, CommissionRuleViewModel model)
        {
            long ruleId = 0;
            try
            {
                
                SqlConnection con = new SqlConnection(_connectionOptions.ConnectionString);
                await con.OpenAsync();

                SqlCommand sqlCommand = new SqlCommand("CommissionRule_Add", con);
                sqlCommand.CommandType = CommandType.StoredProcedure;

                SqlParameter sqlParam1 = sqlCommand.Parameters.AddWithValue("@Id", model.Id);
                SqlParameter sqlParam2 = sqlCommand.Parameters.AddWithValue("@Comment", model.Comment);
                SqlParameter sqlParam3 = sqlCommand.Parameters.AddWithValue("@Priority", model.Priority);
                SqlParameter sqlParam4 = sqlCommand.Parameters.AddWithValue("@TimeToApply", model.TimeToApply);
                SqlParameter sqlParam5 = sqlCommand.Parameters.AddWithValue("@WhenToApplyComboId", model.WhenToApplyComboId);
                SqlParameter sqlParam6 = sqlCommand.Parameters.AddWithValue("@OrderComment", model.OrderComment);
                SqlParameter sqlParam7 = sqlCommand.Parameters.AddWithValue("@Leverage", model.Leverage);
                SqlParameter sqlParam8 = sqlCommand.Parameters.AddWithValue("@UnitsTypeId", model.UnitsTypeId);
                SqlParameter sqlParam9 = sqlCommand.Parameters.AddWithValue("@WhereToApplyId", model.WhereToApplyId);
                SqlParameter sqlParam10 = sqlCommand.Parameters.AddWithValue("@PercentageValue", model.PercentageValue);
                SqlParameter sqlParam = sqlCommand.Parameters.AddWithValue("@paramTable", dataTable);
                SqlParameter sqlParam11 = sqlCommand.Parameters.AddWithValue("@IsMobileView", model.IsMobileView);
                SqlParameter sqlParam12 = sqlCommand.Parameters.AddWithValue("@IsBrokerCommission", model.IsBrokerCommission);


                sqlParam.SqlDbType = SqlDbType.Structured;

                ruleId = Convert.ToInt32(await sqlCommand.ExecuteScalarAsync());

                await con.CloseAsync();
            }
            catch (Exception ex)
            {

                throw;
            }
            
            return ruleId;
        }

        public async Task<long> SaveCommissionRuleWithoutFile(CommissionRuleViewModel model)
        {
            long ruleId = 0;
            try
            {

                SqlConnection con = new SqlConnection(_connectionOptions.ConnectionString);
                await con.OpenAsync();

                SqlCommand sqlCommand = new SqlCommand("CommissionRule_Save", con);
                sqlCommand.CommandType = CommandType.StoredProcedure;

                SqlParameter sqlParam1 = sqlCommand.Parameters.AddWithValue("@Id", model.Id);
                SqlParameter sqlParam2 = sqlCommand.Parameters.AddWithValue("@Comment", model.Comment);
                SqlParameter sqlParam3 = sqlCommand.Parameters.AddWithValue("@Priority", model.Priority);
                SqlParameter sqlParam4 = sqlCommand.Parameters.AddWithValue("@TimeToApply", model.TimeToApply);
                SqlParameter sqlParam5 = sqlCommand.Parameters.AddWithValue("@WhenToApplyComboId", model.WhenToApplyComboId);
                SqlParameter sqlParam6 = sqlCommand.Parameters.AddWithValue("@OrderComment", model.OrderComment);
                SqlParameter sqlParam7 = sqlCommand.Parameters.AddWithValue("@Leverage", model.Leverage);
                SqlParameter sqlParam8 = sqlCommand.Parameters.AddWithValue("@UnitsTypeId", model.UnitsTypeId);
                SqlParameter sqlParam9 = sqlCommand.Parameters.AddWithValue("@WhereToApplyId", model.WhereToApplyId);
                SqlParameter sqlParam10 = sqlCommand.Parameters.AddWithValue("@PercentageValue", model.PercentageValue);
                SqlParameter sqlParam11 = sqlCommand.Parameters.AddWithValue("@IsMobileView", model.IsMobileView);
                SqlParameter sqlParam12 = sqlCommand.Parameters.AddWithValue("@IsBrokerCommission", model.IsBrokerCommission);

                ruleId = Convert.ToInt32(await sqlCommand.ExecuteScalarAsync());

                await con.CloseAsync();
            }
            catch (Exception ex)
            {

                throw;
            }

            return ruleId;
        }
        public async Task<long> UpdateCommissionRuleInstrument(DataTable dataTable, CommissionRuleViewModel model)
        {
            long ruleId = 0;
            SqlConnection con = new SqlConnection(_connectionOptions.ConnectionString);
            await con.OpenAsync();

            SqlCommand sqlCommand = new SqlCommand("CommissionRuleInstrument_Update", con);
            sqlCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter sqlParam1 = sqlCommand.Parameters.AddWithValue("@Id", model.Id);
            SqlParameter sqlParam2 = sqlCommand.Parameters.AddWithValue("@Comment", model.Comment);
            SqlParameter sqlParam3 = sqlCommand.Parameters.AddWithValue("@Priority", model.Priority);
            SqlParameter sqlParam4 = sqlCommand.Parameters.AddWithValue("@TimeToApply", model.TimeToApply);
            SqlParameter sqlParam5 = sqlCommand.Parameters.AddWithValue("@WhenToApplyComboId", model.WhenToApplyComboId);
            SqlParameter sqlParam6 = sqlCommand.Parameters.AddWithValue("@OrderComment", model.OrderComment);
            SqlParameter sqlParam7 = sqlCommand.Parameters.AddWithValue("@Leverage", model.Leverage);
            SqlParameter sqlParam8 = sqlCommand.Parameters.AddWithValue("@UnitsTypeId", model.UnitsTypeId);
            SqlParameter sqlParam9 = sqlCommand.Parameters.AddWithValue("@WhereToApplyId", model.WhereToApplyId);
            SqlParameter sqlParam10 = sqlCommand.Parameters.AddWithValue("@PercentageValue", model.PercentageValue);
            SqlParameter sqlParam = sqlCommand.Parameters.AddWithValue("@paramTable", dataTable);
            SqlParameter sqlParam11 = sqlCommand.Parameters.AddWithValue("@IsMobileView", model.IsMobileView);
            SqlParameter sqlParam12 = sqlCommand.Parameters.AddWithValue("@IsBrokerCommission", model.IsBrokerCommission);

            sqlParam.SqlDbType = SqlDbType.Structured;

            ruleId = Convert.ToInt32(await sqlCommand.ExecuteScalarAsync());

            //await sqlCommand.ExecuteNonQueryAsync();

            await con.CloseAsync();
            return ruleId;
        }
        public async Task<CommissionRuleViewModel> GetCommissionRuleById(int Id)
        {
            CommissionRuleViewModel response = new();

            var vParams = new DynamicParameters();
            vParams.Add("@RuleId", Id);
            var res = await _db.QueryMultipleAsync("[GET_CommissionRule_ById]", vParams, commandType: CommandType.StoredProcedure);

            response = res.Read<CommissionRuleViewModel>().FirstOrDefault();

            return response;
        }

        public async Task<Tuple<List<CommissionRuleInstrumentViewModel>, long>> GetCommissionRuleInstrumentById(PageParam pageParam,string search, int Id)
        {
			List<CommissionRuleInstrumentViewModel> commissionRuleViewModels = new List<CommissionRuleInstrumentViewModel>();
			var param = new DynamicParameters();
			param.Add("@RuleId", Id, dbType: DbType.Int64, direction: ParameterDirection.Input);
			param.Add("@Offset", pageParam.Offset, dbType: DbType.Int64, direction: ParameterDirection.Input);
			param.Add("@Limit", pageParam.Limit, dbType: DbType.Int64, direction: ParameterDirection.Input);
			param.Add("@Search", search, dbType: DbType.String, direction: ParameterDirection.Input);

			var res = _db.QueryMultiple("[dbo].[GET_Commission_Instrument_ById]", param, commandType: CommandType.StoredProcedure);

			commissionRuleViewModels.AddRange(res.Read<CommissionRuleInstrumentViewModel>());
			long no = res.Read<long>().SingleOrDefault();
			var tuple = new Tuple<List<CommissionRuleInstrumentViewModel>, long>(commissionRuleViewModels, no);
			return tuple;
		}

        public async Task<int> DeleteCommissionRuleInstrumentById(int CommisssionRuleId, int CommisssionRuleInstrumentId)
        {
            var vParams = new DynamicParameters();
            vParams.Add("@CommissionRuleId", CommisssionRuleId);
            vParams.Add("@CommissionRuleInstrumentId", CommisssionRuleInstrumentId);
            var res = await _db.QueryMultipleAsync("[Commisson_RuleInstrument_Delete]", vParams, commandType: CommandType.StoredProcedure);

            return 1;
        }

        public async Task<int> DeleteCommissionRuleById(int CommissionRuleId)
        {
            var vParams = new DynamicParameters();
            vParams.Add("@RuleId", CommissionRuleId);
            var res = await _db.QueryMultipleAsync("[Delete_CommissionRule_ById]", vParams, commandType: CommandType.StoredProcedure);

            return 1;
        }

        public async Task<List<WhenToApplyComboViewModel>> GetWhenToApplyCombo()
        {
            var result = await _db.QueryAsync<WhenToApplyComboViewModel>(
               @"SELECT Id, Name FROM WhenToApplyCombo WHERE IsActive = 1"
               );

            return result.ToList();
        }

        public async Task<List<UnitsTypeViewModel>> GetUnitsType()
        {
            var result = await _db.QueryAsync<UnitsTypeViewModel>(
               @"SELECT Id, Name FROM UnitTypes WHERE IsActive = 1"
               );

            return result.ToList();
        }

        public async Task<List<WhereToApplyIdViewModel>> GetWhereToApplyId()
        {
            var result = await _db.QueryAsync<WhereToApplyIdViewModel>(
               @"SELECT Id, Name FROM WhereToApplyCommission WHERE IsActive = 1"
               );

            return result.ToList();
        }
    }
}
