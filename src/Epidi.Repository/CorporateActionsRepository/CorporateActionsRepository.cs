using Dapper;
using Epidi.Models.DBConnection;
using Epidi.Models.Paging;
using Epidi.Models.ViewModel.BDM;
using Epidi.Models.ViewModel.BDMCommisionMarkUpSettingInstrumentWise;
using Epidi.Models.ViewModel.CorporateActions;
using Epidi.Models.ViewModel.Country;
using Epidi.Models.ViewModel.InstrumentMaster;
using Epidi.Repository.ConnectionProvider;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Repository.CorporateActionsRepository
{
    public class CorporateActionsRepository : RepositoryBase, ICorporateActionsRepository   
    {
        public CorporateActionsRepository(IOptions<ConnectionSettings> connectionOptions,
             IConnectionProvider connectionProvider
            ) : base(connectionOptions: connectionOptions,
                 connectionProvider: connectionProvider)
        {
        }
        public async Task<List<InstrumentMasterViewModel>> Get_MasterInstrument()
        {
            List<InstrumentMasterViewModel> response = new List<InstrumentMasterViewModel>();

            var res = await _db.QueryMultipleAsync("[Instrument_GetAll]", commandType: CommandType.StoredProcedure);

            response.AddRange(res.Read<InstrumentMasterViewModel>());
            return response.ToList();
        }

        public async Task<List<StockDetailsViewModel>> GetStockDetails(long InstrumentId)
        {
            List<StockDetailsViewModel> response = new List<StockDetailsViewModel>();
            var param = new DynamicParameters();
            param.Add("@InstrumentId", InstrumentId, dbType: DbType.Int64, direction: ParameterDirection.Input);

            var res = await _db.QueryMultipleAsync("[dbo].[GetStockDetails]", param, commandType: CommandType.StoredProcedure);

            response.AddRange(res.Read<StockDetailsViewModel>());
            return response.ToList();
        }

        public async Task<List<StockDetailsViewModel>> SaveNewEquityStockDetails(long OldInstrumentId, long NewInstrumentId, string OrderIds)
        {
            List<StockDetailsViewModel> response = new List<StockDetailsViewModel>();

            var vParams = new DynamicParameters();
            vParams.Add("@OldInstrumentId", OldInstrumentId);
            vParams.Add("@NewInstrumentId", NewInstrumentId);
            vParams.Add("@OrderIds", OrderIds);

            var res = await _db.QueryMultipleAsync("[Upsert_StockDetailsForNewEquity]", vParams, commandType: CommandType.StoredProcedure);

            response.AddRange(res.Read<StockDetailsViewModel>());
             
            return response;
        }
        public async Task<List<StockDetailsViewModel>> SaveDelstingComment(string OrderIds)
        {
            List<StockDetailsViewModel> response = new List<StockDetailsViewModel>();

            var vParams = new DynamicParameters();
            vParams.Add("@OrderIds", OrderIds);

            var res = await _db.QueryMultipleAsync("[Upsert_DelstingComment]", vParams, commandType: CommandType.StoredProcedure);

            response.AddRange(res.Read<StockDetailsViewModel>());
             
            return response;
        }

        public async Task<List<SplitCalculatedOrderViewModel>> GetCalculatedSplitOrderDetails(SplitViewModel splitViewModel)
        {
            List<SplitCalculatedOrderViewModel> response = new List<SplitCalculatedOrderViewModel>();
            var param = new DynamicParameters();
            param.Add("@InstrumentId", splitViewModel.InstrumentId, dbType: DbType.Int64, direction: ParameterDirection.Input);
            param.Add("@OldValue", splitViewModel.OldValue, dbType: DbType.Decimal, direction: ParameterDirection.Input);
            param.Add("@SplitValue", splitViewModel.SplitValue, dbType: DbType.Decimal, direction: ParameterDirection.Input);
            param.Add("@MDU", splitViewModel.MDU, dbType: DbType.Int32, direction: ParameterDirection.Input);
            param.Add("@IsAdjustment", splitViewModel.IsAdjustment, dbType: DbType.Boolean, direction: ParameterDirection.Input);
            param.Add("@RoundPriceAndUnitsBuy", splitViewModel.RoundPriceNUnitsBuy, dbType: DbType.String, direction: ParameterDirection.Input);
            param.Add("@RoundPriceAndUnitsSell", splitViewModel.RoundPriceNUnitsSell, dbType: DbType.String, direction: ParameterDirection.Input);

            var res = await _db.QueryMultipleAsync("[dbo].[GetCalculatedSplitOrderDetails]", param, commandType: CommandType.StoredProcedure);

            response.AddRange(res.Read<SplitCalculatedOrderViewModel>());
            return response.ToList();
        }
        public async Task<List<DelistingOrderDetailsViewModel>> GetCalculatedDelistingOrderDetails(DelistingViewModel delistingViewModel)
        {
            List<DelistingOrderDetailsViewModel> response = new List<DelistingOrderDetailsViewModel>();
            var param = new DynamicParameters();
            param.Add("@InstrumentId", delistingViewModel.InstrumentId, dbType: DbType.Int64, direction: ParameterDirection.Input);
            
            var res = await _db.QueryMultipleAsync("[dbo].[GetCalculatedDelistingDetails]", param, commandType: CommandType.StoredProcedure);

            response.AddRange(res.Read<DelistingOrderDetailsViewModel>());
            return response.ToList();
        }
        public async Task<List<SplitCalculatedOrderViewModel>> GetCalculatedReverseSplitOrderDetails(SplitViewModel splitViewModel)
        {
            List<SplitCalculatedOrderViewModel> response = new List<SplitCalculatedOrderViewModel>();
            var param = new DynamicParameters();
            param.Add("@InstrumentId", splitViewModel.InstrumentId, dbType: DbType.Int64, direction: ParameterDirection.Input);
            param.Add("@OldValue", splitViewModel.OldValue, dbType: DbType.Decimal, direction: ParameterDirection.Input);
            param.Add("@SplitValue", splitViewModel.SplitValue, dbType: DbType.Decimal, direction: ParameterDirection.Input);
            param.Add("@MDU", splitViewModel.MDU, dbType: DbType.Int32, direction: ParameterDirection.Input);
            param.Add("@IsAdjustment", splitViewModel.IsAdjustment, dbType: DbType.Boolean, direction: ParameterDirection.Input);
            param.Add("@RoundPriceAndUnitsBuy", splitViewModel.RoundPriceNUnitsBuy, dbType: DbType.String, direction: ParameterDirection.Input);
            param.Add("@RoundPriceAndUnitsSell", splitViewModel.RoundPriceNUnitsSell, dbType: DbType.String, direction: ParameterDirection.Input);

            var res = await _db.QueryMultipleAsync("[dbo].[GetCalculatedReverseSplitOrderDetails]", param, commandType: CommandType.StoredProcedure);

            response.AddRange(res.Read<SplitCalculatedOrderViewModel>());
            return response.ToList();
        }
        public Tuple<List<SpinOffOldOrderViewModel>, List<SpinOffNewOrderViewModel>> GetCalculatedSpinOffOrderDetails(SpinoffViewModel spinoffViewModel)
        {
            List<SpinOffOldOrderViewModel> spinOffOldOrderViewModels = new List<SpinOffOldOrderViewModel>();
            List<SpinOffNewOrderViewModel> spinOffNewOrderViews = new List<SpinOffNewOrderViewModel>();
            var param = new DynamicParameters();
            param.Add("@InstrumentId", spinoffViewModel.InstrumentId, dbType: DbType.Int64, direction: ParameterDirection.Input);
            param.Add("@NewInstrumentId", spinoffViewModel.NewInstrumentId, dbType: DbType.Int64, direction: ParameterDirection.Input);
            param.Add("@NewRatio", spinoffViewModel.NewRatio, dbType: DbType.Decimal, direction: ParameterDirection.Input);
            param.Add("@SpinOffRatio", spinoffViewModel.SpinOffRatio, dbType: DbType.Decimal, direction: ParameterDirection.Input);
            param.Add("@MDU", spinoffViewModel.SpinOffMDU, dbType: DbType.Int32, direction: ParameterDirection.Input);
            param.Add("@IsAdjustment", spinoffViewModel.SpinOffAdjustment, dbType: DbType.Boolean, direction: ParameterDirection.Input);
            param.Add("@RoundUnitsBuy", spinoffViewModel.SpinOffRoundUnitsBuy, dbType: DbType.String, direction: ParameterDirection.Input);
            param.Add("@RoundUnitsSell", spinoffViewModel.SpinOffRoundUnitsSell, dbType: DbType.String, direction: ParameterDirection.Input);

            var res = _db.QueryMultiple("[dbo].[GetCalculatedSpinOffOrderDetails]", param, commandType: CommandType.StoredProcedure);

            spinOffOldOrderViewModels.AddRange(res.Read<SpinOffOldOrderViewModel>());
            spinOffNewOrderViews.AddRange(res.Read<SpinOffNewOrderViewModel>());
            var tuple = new Tuple<List<SpinOffOldOrderViewModel>, List<SpinOffNewOrderViewModel>>(spinOffOldOrderViewModels, spinOffNewOrderViews);
            return tuple;
        }

        public List<DividendDetailsViewModel> GetDivedendDetails(string InstrumentId, string FromDate)
        {
            List<DividendDetailsViewModel> response = new List<DividendDetailsViewModel>();
            var param = new DynamicParameters();
            param.Add("@InstrumentId", InstrumentId, dbType: DbType.String, direction: ParameterDirection.Input);
            param.Add("@FromDate", FromDate, dbType: DbType.String, direction: ParameterDirection.Input);
           
            var res =  _db.QueryMultiple("[dbo].[DividendDetails_GET]", param, commandType: CommandType.StoredProcedure);

            response.AddRange(res.Read<DividendDetailsViewModel>());
            return response.ToList();
        }

        public List<DividendDetailsViewModel> ExportDivedendDetails()
        {
            List<DividendDetailsViewModel> response = new List<DividendDetailsViewModel>();
            var param = new DynamicParameters();
            
            var res = _db.QueryMultiple("[dbo].[Export_DividendDetails_GET]", param, commandType: CommandType.StoredProcedure);

            response.AddRange(res.Read<DividendDetailsViewModel>());
            return response.ToList();
        }

        public List<DividendOrdersViewModel> GetDivedendOrderDetails(int InstrumentId,string FromDate,string ToDate)
        {
            List<DividendOrdersViewModel> response = new List<DividendOrdersViewModel>();
            var param = new DynamicParameters();
            param.Add("@InstrumentId", InstrumentId, dbType: DbType.String, direction: ParameterDirection.Input);
            param.Add("@FromDate", FromDate, dbType: DbType.String, direction: ParameterDirection.Input);
            param.Add("@ToDate", ToDate, dbType: DbType.String, direction: ParameterDirection.Input);

            var res = _db.QueryMultiple("[dbo].[GetCalculatedDividendOrderDetails]", param, commandType: CommandType.StoredProcedure);

            response.AddRange(res.Read<DividendOrdersViewModel>());
            return response.ToList();
        }

        public async Task<long> AddUpdateSplitOrder(DataTable dataTable)
        {
            SqlConnection con = new SqlConnection(_connectionOptions.ConnectionString);
            await con.OpenAsync();

            SqlCommand sqlCommand = new SqlCommand("AddUpdate_SplitData_Order", con);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandTimeout = 0;
            SqlParameter sqlParam = sqlCommand.Parameters.AddWithValue("@paramTable", dataTable);
            sqlParam.SqlDbType = SqlDbType.Structured;

            await sqlCommand.ExecuteNonQueryAsync();

            await con.CloseAsync();
            return 1;
        }

        public async Task<long> AddUpdateDividenMaster(DataTable dataTable)
        {
            SqlConnection con = new SqlConnection(_connectionOptions.ConnectionString);
            await con.OpenAsync();

            SqlCommand sqlCommand = new SqlCommand("AddUpdate_Dividend_Master", con);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandTimeout = 0;
            SqlParameter sqlParam = sqlCommand.Parameters.AddWithValue("@paramTable", dataTable);
            sqlParam.SqlDbType = SqlDbType.Structured;

            await sqlCommand.ExecuteNonQueryAsync();

            await con.CloseAsync();
            return 1;
        }

        public async Task<long> AddUpdateReverseSplitOrder(DataTable dataTable)
        {
            SqlConnection con = new SqlConnection(_connectionOptions.ConnectionString);
            await con.OpenAsync();

            SqlCommand sqlCommand = new SqlCommand("AddUpdate_ReverseSplitData_Order", con);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandTimeout = 0;
            SqlParameter sqlParam = sqlCommand.Parameters.AddWithValue("@paramTable", dataTable);
            sqlParam.SqlDbType = SqlDbType.Structured;

            await sqlCommand.ExecuteNonQueryAsync();

            await con.CloseAsync();
            return 1;
        }      
        public async Task<long> AddUpdateSpinOffOrder(DataTable dataTable)
        {
            SqlConnection con = new SqlConnection(_connectionOptions.ConnectionString);
            await con.OpenAsync();

            SqlCommand sqlCommand = new SqlCommand("AddUpdate_SpinOff_Order", con);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandTimeout = 0;
            SqlParameter sqlParam = sqlCommand.Parameters.AddWithValue("@paramTable", dataTable);
            sqlParam.SqlDbType = SqlDbType.Structured;

            await sqlCommand.ExecuteNonQueryAsync();

            await con.CloseAsync();
            return 1;
        }
    }
}
