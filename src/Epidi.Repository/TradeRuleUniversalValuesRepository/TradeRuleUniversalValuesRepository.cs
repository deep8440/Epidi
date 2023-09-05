using Dapper;
using Epidi.Models.DBConnection;
using Epidi.Models.Model;
using Epidi.Models.Paging;
using Epidi.Models.ViewModel.BDM;
using Epidi.Models.ViewModel.Common;
using Epidi.Models.ViewModel.Instrument;
using Epidi.Models.ViewModel.Question;
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

namespace Epidi.Repository.TradeRuleUniversalValuesRepository
{
    public class TradeRuleUniversalValuesRepository : RepositoryBase, ITradeRuleUniversalValuesRepository
    {
        public TradeRuleUniversalValuesRepository(IOptions<ConnectionSettings> connectionOptions,
            IConnectionProvider connectionProvider) : base(connectionOptions: connectionOptions,
            connectionProvider: connectionProvider)
        {
        }

        public Tuple<List<TradeRuleUniversalValuesViewModel>, long> GetAll_TradeRuleUniversalValues(PageParam pageParam, string search)
        {
           
            List<TradeRuleUniversalValuesViewModel> response = new List<TradeRuleUniversalValuesViewModel>();

            var vParams = new DynamicParameters();
            vParams.Add("@Offset", pageParam.Offset, dbType: DbType.Int64, direction: ParameterDirection.Input);
            vParams.Add("@Limit", pageParam.Limit, dbType: DbType.Int64, direction: ParameterDirection.Input);
            vParams.Add("@Search", search, dbType: DbType.String, direction: ParameterDirection.Input);

            var res = _db.QueryMultiple("[TradeRule_UniversalValues_GetAll]", vParams, commandType: CommandType.StoredProcedure);

            response.AddRange(res.Read<TradeRuleUniversalValuesViewModel>());
            long no = res.Read<long>().SingleOrDefault();
            var tuple = new Tuple<List<TradeRuleUniversalValuesViewModel>, long>(response, no);
            return tuple;

        }

        public async Task<TradeRuleUniversalValuesViewModel> GetTradeRuleUniversalValues_ById(int Id)
        {
            TradeRuleUniversalValuesViewModel response = new();

            var vParams = new DynamicParameters();
            vParams.Add("@Id", Id);
            var res = await _db.QueryMultipleAsync("[TradeRule_UniversalValues_GetById]", vParams, commandType: CommandType.StoredProcedure);

            response = res.Read<TradeRuleUniversalValuesViewModel>().FirstOrDefault();

            return response;

        }

        public int SaveTradeRuleUniversalValues(DataTable dataTable)
        {
            dataTable.Columns.Remove("itemlists");
            int ruleId;

            SqlConnection con = new SqlConnection(_connectionOptions.ConnectionString);
            con.Open();

            SqlCommand sqlCommand = new SqlCommand("Import_TradeRuleUniversalValues", con);
            sqlCommand.CommandType = CommandType.StoredProcedure;


            SqlParameter sqlParam = sqlCommand.Parameters.AddWithValue("@paramTable", dataTable);
            sqlParam.SqlDbType = SqlDbType.Structured;

            ruleId = Convert.ToInt32(sqlCommand.ExecuteScalar());
			dataTable.Columns.Add("itemlists");
			con.Close();
            return ruleId;
        }

        public List<ResponseViewModel> TradeRuleUniversalValuesDelete(int Id)
        {
            List<ResponseViewModel> response = new List<ResponseViewModel>();

            var param = new DynamicParameters();
            param.Add("@Id", Id, dbType: DbType.Int32, direction: ParameterDirection.Input);

            var res = _db.QueryMultiple("[dbo].[TradeRule_UniversalValues_Delete]", param, commandType: CommandType.StoredProcedure);


            response = res.Read<ResponseViewModel>().ToList();

            return response;

        }

        public async Task<List<UniversalParametersViewModel>> GetUniversalParameters()
        {
            var result = await _db.QueryAsync<UniversalParametersViewModel>(
             @"SELECT Id,Name FROM UniversalParameters"
             );

            return result.ToList();
        }



        public async Task<List<ActionsDefintiionsViewModel>> GetActionsDefintiions()
        {
            var result = await _db.QueryAsync<ActionsDefintiionsViewModel>(
             @"SELECT Id,Name FROM ActionsDefintiions"
             );

            return result.ToList();
        }

		public async Task<List<WhenToCheckViewModel>> GetWhenToCheckList()
		{
			var result = await _db.QueryAsync<WhenToCheckViewModel>(
			 @"SELECT Id,Name FROM WhenToCheck"
			 );

			return result.ToList();
		}

		public async Task<List<FunctionsViewModel>> GetFunctionsList()
		{
			var result = await _db.QueryAsync<FunctionsViewModel>(
			 @"SELECT Id,Name FROM Functions"
			 );

			return result.ToList();
		}

		public async Task<List<TablesViewModel>> GetTablesList()
		{
			var result = await _db.QueryAsync<TablesViewModel>(
			 @"SELECT Id,Name FROM Tables"
			 );

			return result.ToList();
		}

		public async Task<List<TradeRuleUniversalValuesViewModel>> ExportTradeRuleUniversalValues()
        {
            List<TradeRuleUniversalValuesViewModel> instrumentMasterModel = new();
            
            var res = await _db.QueryMultipleAsync("[dbo].[TradeRule_UniversalValues_Export]",  commandType: CommandType.StoredProcedure).ConfigureAwait(false);

            instrumentMasterModel.AddRange(res.Read<TradeRuleUniversalValuesViewModel>());
            return instrumentMasterModel;
        }
    }
}
