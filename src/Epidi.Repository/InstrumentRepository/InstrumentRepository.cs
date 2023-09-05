using Dapper;
using Epidi.Models.DBConnection;
using Epidi.Models.Paging;
using Epidi.Models.Utility;
using Epidi.Models.ViewModel.Country;
using Epidi.Models.ViewModel.Instrument;
using Epidi.Models.ViewModel.InstrumentMaster;
using Epidi.Models.ViewModel.Question;
using Epidi.Models.ViewModel.SymbolGroup;
using Epidi.Models.ViewModel.TradeStatus;
using Epidi.Repository.ConnectionProvider;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Epidi.Repository.InstrumentRepository
{
    public class InstrumentRepository : RepositoryBase, IInstrumentRepository
    {
        public InstrumentRepository(IOptions<ConnectionSettings> connectionOptions,
           IConnectionProvider connectionProvider
          ) : base(connectionOptions: connectionOptions,
               connectionProvider: connectionProvider)
        {
        }
        public async Task<long> AddMapping(InstrumentMasterMapping instrumentMasterMapping)
        {
            long retval;


            retval = await _db.QuerySingleAsync<long>(
                    @"INSERT INTO [dbo].[InstrumentMapping]
                   ([master_id]
                   ,[GateIO]
                   ,[Lmax])
                OUTPUT inserted.Id
                   VALUES
                   (@param_master_id
                   ,@param_gate_io
                   ,@param_lmax)
                 ;", new
                    {
                        @param_master_id = instrumentMasterMapping.master_id,
                        @param_gate_io = instrumentMasterMapping.GateIO,
                        @param_lmax = instrumentMasterMapping.Lmax
                    }).ConfigureAwait(false);

            return retval;
        }
        public async Task<long> AddMultipleMapping(DataTable dataTable)
        {
            SqlConnection con = new SqlConnection(_connectionOptions.ConnectionString);
            await con.OpenAsync();

            SqlCommand sqlCommand = new SqlCommand("InstrumentMapping_Add", con);
            sqlCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter sqlParam = sqlCommand.Parameters.AddWithValue("@paramTable", dataTable);
            sqlParam.SqlDbType = SqlDbType.Structured;

            await sqlCommand.ExecuteNonQueryAsync();

            await con.CloseAsync();
            return 1;
        }

        public async Task<long> AddGateIOInstrument(DataTable dataTable)
        {
            SqlConnection con = new SqlConnection(_connectionOptions.ConnectionString);
            await con.OpenAsync();

            SqlCommand sqlCommand = new SqlCommand("GateIOInstrumentMapping_Add", con);
            sqlCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter sqlParam = sqlCommand.Parameters.AddWithValue("@paramTable", dataTable);
            sqlParam.SqlDbType = SqlDbType.Structured;

            await sqlCommand.ExecuteNonQueryAsync();

            await con.CloseAsync();
            return 1;
        }

        public async Task<long> AddLMaxInstrument(DataTable dataTable)
        {
            SqlConnection con = new SqlConnection(_connectionOptions.ConnectionString);
            await con.OpenAsync();

            SqlCommand sqlCommand = new SqlCommand("LMaxInstrument_Add", con);
            sqlCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter sqlParam = sqlCommand.Parameters.AddWithValue("@paramTable", dataTable);
            sqlParam.SqlDbType = SqlDbType.Structured;

            await sqlCommand.ExecuteNonQueryAsync();

            await con.CloseAsync();
            return 1;
        }

        public async Task<long> AddInstrumentMapping(DataTable dataTable)
        {
            SqlConnection con = new SqlConnection(_connectionOptions.ConnectionString);
            await con.OpenAsync();

            SqlCommand sqlCommand = new SqlCommand("ImportExcelInstrumentMapping_Add", con);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandTimeout = 0;
            SqlParameter sqlParam = sqlCommand.Parameters.AddWithValue("@paramTable", dataTable);
            sqlParam.SqlDbType = SqlDbType.Structured;

            var result= await sqlCommand.ExecuteNonQueryAsync();

            await con.CloseAsync();
            return 1;
        }
        public async Task<long> UpdateInstrumentMaster(MasterInstrumentViewModel model)
        {
            var param = new DynamicParameters();
            param.Add("@Id", model.Id, dbType: DbType.Int32, direction: ParameterDirection.Input);
            param.Add("@InstrumentName", model.InstrumentName.Trim(), dbType: DbType.String, direction: ParameterDirection.Input);
            param.Add("@SymbolGroup", model.SymbolGroup.Trim(), dbType: DbType.String, direction: ParameterDirection.Input);
            param.Add("@TradeStatus", model.TradeStatus.Trim(), dbType: DbType.String, direction: ParameterDirection.Input);
            param.Add("@SymbolDenomination", model.SymbolDenomination.Trim(), dbType: DbType.String, direction: ParameterDirection.Input);
            param.Add("@AverageSpread", model.AverageSpread, dbType: DbType.Decimal, direction: ParameterDirection.Input);
            param.Add("@Decimals", model.Decimals, dbType: DbType.Decimal, direction: ParameterDirection.Input);

            var res = await _db.QueryMultipleAsync("[dbo].[UpdateInstrumentmastermapping]", param, commandType: CommandType.StoredProcedure).ConfigureAwait(false);

            foreach (var item in model.tradeTimings)
            {
                param = new DynamicParameters();
                param.Add("@Id", model.Id, dbType: DbType.Int32, direction: ParameterDirection.Input);
                param.Add("@QTFrom", item.QTFrom, dbType: DbType.String, direction: ParameterDirection.Input);
                param.Add("@QTTo", item.QTTo, dbType: DbType.String, direction: ParameterDirection.Input);
                param.Add("@Day", item.Day, dbType: DbType.Int32, direction: ParameterDirection.Input);
                param.Add("@TTFrom", item.TTFrom, dbType: DbType.String, direction: ParameterDirection.Input);
                param.Add("@TTTo", item.TTTo, dbType: DbType.String, direction: ParameterDirection.Input);
                param.Add("@TradeStatus", model.TradeStatus.Trim(), dbType: DbType.String, direction: ParameterDirection.Input);
                param.Add("@TradeId", model.TradeId, dbType: DbType.Int32, direction: ParameterDirection.Input);
                var res1 = await _db.QueryMultipleAsync("[dbo].[UpdateTradeTimings]", param, commandType: CommandType.StoredProcedure).ConfigureAwait(false);
            }
            return 1;
        }
        public async Task<long> AddMasterInstrumentMapping(DataTable dataTable, InstrumentSpecificationRule model)
        {
            try
            {
                int ruleId;
                SqlConnection con = new SqlConnection(_connectionOptions.ConnectionString);
                await con.OpenAsync();

                //SqlCommand sqlCommand = new SqlCommand("ImportExcelMasterInstrumentMapping_Add", con);
                SqlCommand sqlCommand = new SqlCommand("Import_InstrumentDataForSpecifiRule", con);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandTimeout = 1800;
                SqlParameter sqlParam1 = sqlCommand.Parameters.AddWithValue("@SpecificationRuleId", model.Id);
                SqlParameter sqlParam2 = sqlCommand.Parameters.AddWithValue("@Comment", model.Comment);
                SqlParameter sqlParam3 = sqlCommand.Parameters.AddWithValue("@Priority", model.Priority);
                SqlParameter sqlParam = sqlCommand.Parameters.AddWithValue("@paramTable", dataTable);
                sqlParam.SqlDbType = SqlDbType.Structured;

                ruleId = Convert.ToInt32(await sqlCommand.ExecuteScalarAsync());

                await con.CloseAsync();
                return ruleId;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<long> UpdateSpecificationRuleInstrument(DataTable dataTable, InstrumentSpecificationRule model)
        {
            try
            {
                SqlConnection con = new SqlConnection(_connectionOptions.ConnectionString);
                await con.OpenAsync();

                SqlCommand sqlCommand = new SqlCommand("Update_SpecificationRule_Instrument", con);
                sqlCommand.CommandType = CommandType.StoredProcedure;

                SqlParameter sqlParam1 = sqlCommand.Parameters.AddWithValue("@Id", model.Id);
                SqlParameter sqlParam2 = sqlCommand.Parameters.AddWithValue("@Comment", model.Comment);
                SqlParameter sqlParam3 = sqlCommand.Parameters.AddWithValue("@Priority", model.Priority);
                SqlParameter sqlParam = sqlCommand.Parameters.AddWithValue("@paramTable", (dataTable.Rows.Count > 0 ? dataTable : null));
                sqlParam.SqlDbType = SqlDbType.Structured;

                await sqlCommand.ExecuteNonQueryAsync();

                await con.CloseAsync();
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public async Task<int> DeleteSpecificationRuleInstrumentById(int SpecificationRuleId, int InstrumentId)
        {
            var vParams = new DynamicParameters();
            vParams.Add("@SpecificationRuleId", SpecificationRuleId);
            vParams.Add("@InstrumentId", InstrumentId);
            var res = await _db.QueryMultipleAsync("[Specification_RuleInstrument_Delete]", vParams, commandType: CommandType.StoredProcedure);

            return 1;
        }

        public async Task<int> DeleteSpecificationRuleById(int RuleId)
        {
            var vParams = new DynamicParameters();
            vParams.Add("@RuleId", RuleId);
            var res = await _db.QueryMultipleAsync("[Delete_SpecificationRule_ById]", vParams, commandType: CommandType.StoredProcedure);

            return 1;
        }
        public async Task<long> AddFavourite(FavouriteInstrument favouriteInstrument)
        {
            long retval;


            retval = await _db.QuerySingleAsync<long>(
                    @"INSERT INTO [dbo].[FavouriteInstrument]
                       ([Userid]
                       ,[master_id]
                       ,[CreatedDate]
                       ,[IsActive])
                 VALUES
                       (@param_Userid
                       ,@param_master_id
                       ,@param_CreatedDate
                       ,@param_IsActive)
                 ;", new
                    {
                        @param_Userid = favouriteInstrument.Userid,
                        @param_master_id = favouriteInstrument.master_id,
                        @param_CreatedDate = DateTime.Now,
                        @param_IsActive = true
                    }).ConfigureAwait(false);

            return retval;
        }

        public async Task<long> EditMapping(InstrumentMasterMapping instrumentMasterMapping)
        {
            long retval;


            retval = await _db.QuerySingleAsync<long>(
                    @"UPDATE [dbo].[tbl_InstrumentMapping]
                   SET [master_id] = @param_master_id
                      ,[GateIO] = @param_GateIO
                      ,[Lmax] = @param_Lmax
                    WHERE Id=@param_id
                 );", new
                    {
                        @param_master_id = instrumentMasterMapping.master_id,
                        @param_GateIO = instrumentMasterMapping.GateIO,
                        @param_Lmax = instrumentMasterMapping.Lmax,
                        @param_id = instrumentMasterMapping.id
                    }).ConfigureAwait(false);

            return retval;
        }

        public async Task<List<GateIO>> GetGateIOs()
        {
            var result = await _db.QueryAsync<GateIO>(@"SELECT * FROM GateIOInstruments");
            return result.ToList();
        }

        public async Task<List<InstrumentMaster>> GetInstruments()
        {
            var result = await _db.QueryAsync<InstrumentMaster>(@"SELECT * FROM LookUpInstrument");
            return result.ToList();
        }

        public async Task<List<LMax>> GetLMaxes()
        {
            var result = await _db.QueryAsync<LMax>(@"SELECT * FROM LMAXInstruments");
            return result.ToList();
        }

        public async Task<long> RemoveMapping(int id)
        {
            long retval;


            retval = await _db.QuerySingleAsync<long>(
                    @"DELETE FROM tbl_InstrumentMapping where Id=@param_id
                 ;", new
                    {
                        @param_id = id
                    }).ConfigureAwait(false);

            return retval;
        }

        public async Task<Tuple<List<InstrumentMaster>, long>> GetAllInstrument(PageParam pageParam, string search)
        {
            List<InstrumentMaster> instrumentMasterModel = new();
            var param = new DynamicParameters();
            param.Add("@Offset", pageParam.Offset, dbType: DbType.Int64, direction: ParameterDirection.Input);
            param.Add("@Limit", pageParam.Limit, dbType: DbType.Int64, direction: ParameterDirection.Input);
            param.Add("@Search", search, dbType: DbType.String, direction: ParameterDirection.Input);


            var res = await _db.QueryMultipleAsync("[dbo].[InstrumentMaster_All]", param, commandType: CommandType.StoredProcedure).ConfigureAwait(false);

            instrumentMasterModel.AddRange(res.Read<InstrumentMaster>());
            long no = res.Read<long>().SingleOrDefault();
            var tuple = new Tuple<List<InstrumentMaster>, long>(instrumentMasterModel, no);
            return tuple;
        }
        public async Task<Tuple<List<InstrumentMaster>, long>> GetAllInstrumentByUserId(PageParam pageParam, string search, int UserId)
        {
            List<InstrumentMaster> instrumentMasterModel = new();
            var param = new DynamicParameters();
            param.Add("@Offset", pageParam.Offset, dbType: DbType.Int64, direction: ParameterDirection.Input);
            param.Add("@Limit", pageParam.Limit, dbType: DbType.Int64, direction: ParameterDirection.Input);
            param.Add("@Search", search, dbType: DbType.String, direction: ParameterDirection.Input);
            param.Add("@UserId", UserId, dbType: DbType.Int64, direction: ParameterDirection.Input);

            var res = await _db.QueryMultipleAsync("[dbo].[InstrumentMaster_All]", param, commandType: CommandType.StoredProcedure).ConfigureAwait(false);

            instrumentMasterModel.AddRange(res.Read<InstrumentMaster>());
            long no = res.Read<long>().SingleOrDefault();
            var tuple = new Tuple<List<InstrumentMaster>, long>(instrumentMasterModel, no);
            return tuple;
        }
        public async Task<List<InstrumentMaster>> GetAllInstrumentPopular()
        {
            List<InstrumentMaster> instrumentMasterModel = new();

            var res = await _db.QueryMultipleAsync("[dbo].[InstrumentMasterPopular_All]", null, commandType: CommandType.StoredProcedure).ConfigureAwait(false);

            instrumentMasterModel.AddRange(res.Read<InstrumentMaster>());
            var tuple = new List<InstrumentMaster>(instrumentMasterModel);
            return tuple;
        }
        public async Task<List<CommissionRuleInstrument>> GetInstrumentsWithCommission()
        {
            try
            {
                List<CommissionRuleInstrument> ComminsioninstrumentMasterModel = new();

                var res = await _db.QueryMultipleAsync("[dbo].[GetInstrumentsWithCommission_All]", null, commandType: CommandType.StoredProcedure).ConfigureAwait(false);

                ComminsioninstrumentMasterModel.AddRange(res.Read<CommissionRuleInstrument>());
                var tuple = new List<CommissionRuleInstrument>(ComminsioninstrumentMasterModel);
                return tuple;
            }
            catch (Exception ex)
            {

                throw;
            }

        }
        public async Task<List<InstrumentWithIBCommision>> GetInstrumentsWithIBCommission(int IBID)
        {
            try
            {
                List<InstrumentWithIBCommision> ComminsioninstrumentMasterModel = new();
                DynamicParameters dynamicParameters = new();
                dynamicParameters.Add("@IBID", IBID, DbType.Int32);
                var res = await _db.QueryMultipleAsync("[dbo].[GetInstrumentCommision_ByIBID]", dynamicParameters, commandType: CommandType.StoredProcedure).ConfigureAwait(false);

                ComminsioninstrumentMasterModel.AddRange(res.Read<InstrumentWithIBCommision>());
                var tuple = new List<InstrumentWithIBCommision>(ComminsioninstrumentMasterModel);
                return tuple;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<Tuple<List<EditSpecificationInstrumentViewModel>, long>> GetAllImportMasterInstrument(int RuleId, PageParam pageParam, string search)
        {
            try
            {
                List<EditSpecificationInstrumentViewModel> instrumentMasterModel = new();
                var param = new DynamicParameters();
                param.Add("@RuleId", RuleId, dbType: DbType.Int64, direction: ParameterDirection.Input);
                param.Add("@Offset", pageParam.Offset, dbType: DbType.Int64, direction: ParameterDirection.Input);
                param.Add("@Limit", pageParam.Limit, dbType: DbType.Int64, direction: ParameterDirection.Input);
                param.Add("@search", search, dbType: DbType.String, direction: ParameterDirection.Input);

                var res = await _db.QueryMultipleAsync("[dbo].[Import_InstrumentMaster_All]", param, commandType: CommandType.StoredProcedure, commandTimeout: 60).ConfigureAwait(false);

                instrumentMasterModel.AddRange(res.Read<EditSpecificationInstrumentViewModel>());
                long no = res.Read<long>().SingleOrDefault();
                var tuple = new Tuple<List<EditSpecificationInstrumentViewModel>, long>(instrumentMasterModel, no);
                return tuple;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Tuple<List<ImportGateIOInstrumentMappingViewModel>, long>> GetAllImportGATEIO(PageParam pageParam, string search)
        {
            List<ImportGateIOInstrumentMappingViewModel> instrumentMasterModel = new();
            var param = new DynamicParameters();
            param.Add("@Offset", pageParam.Offset, dbType: DbType.Int64, direction: ParameterDirection.Input);
            param.Add("@Limit", pageParam.Limit, dbType: DbType.Int64, direction: ParameterDirection.Input);
            param.Add("@Search", search, dbType: DbType.String, direction: ParameterDirection.Input);


            var res = await _db.QueryMultipleAsync("[dbo].[Import_GATEIO_All]", param, commandType: CommandType.StoredProcedure).ConfigureAwait(false);

            instrumentMasterModel.AddRange(res.Read<ImportGateIOInstrumentMappingViewModel>());
            long no = res.Read<long>().SingleOrDefault();
            var tuple = new Tuple<List<ImportGateIOInstrumentMappingViewModel>, long>(instrumentMasterModel, no);
            return tuple;
        }

        public async Task<Tuple<List<ImportLMaxInstrumentsViewModel>, long>> GetAllImportLMax(PageParam pageParam, string search)
        {
            List<ImportLMaxInstrumentsViewModel> instrumentMasterModel = new();
            var param = new DynamicParameters();
            param.Add("@Offset", pageParam.Offset, dbType: DbType.Int64, direction: ParameterDirection.Input);
            param.Add("@Limit", pageParam.Limit, dbType: DbType.Int64, direction: ParameterDirection.Input);
            param.Add("@Search", search, dbType: DbType.String, direction: ParameterDirection.Input);


            var res = await _db.QueryMultipleAsync("[dbo].[Import_LMAX_All]", param, commandType: CommandType.StoredProcedure).ConfigureAwait(false);

            instrumentMasterModel.AddRange(res.Read<ImportLMaxInstrumentsViewModel>());
            long no = res.Read<long>().SingleOrDefault();
            var tuple = new Tuple<List<ImportLMaxInstrumentsViewModel>, long>(instrumentMasterModel, no);
            return tuple;
        }

        public async Task<List<InstrumentMasterDDViewModel>> GetInstrument_DD_Data(int MasterInstrumentId, string LPName)
        {
            List<InstrumentMasterDDViewModel> instrumentMasterDDViewModels = new();
            var param = new DynamicParameters();
            param.Add("@MasterInstrumentId", MasterInstrumentId, dbType: DbType.Int64, direction: ParameterDirection.Input);
            param.Add("@LPName", LPName, dbType: DbType.String, direction: ParameterDirection.Input);


            var res = await _db.QueryMultipleAsync("[dbo].[GET_InstrumentMapping_DD]", param, commandType: CommandType.StoredProcedure).ConfigureAwait(false);

            instrumentMasterDDViewModels.AddRange(res.Read<InstrumentMasterDDViewModel>());

            return instrumentMasterDDViewModels;
        }

        public async Task<List<TradeStatusViewModel>> GetAllTradeStatus()
        {
            var result = await _db.QueryAsync<TradeStatusViewModel>(
               @"SELECT Id,Status FROM TradeStatus"
               );

            return result.ToList();
        }
        public async Task<List<MasterInstrumentViewModel>> GetByID(int MasterInstrumentId, int TradeStatusId)
        {
            List<MasterInstrumentViewModel> instrumentMasterDDViewModels = new();
            var param = new DynamicParameters();
            param.Add("@Id", MasterInstrumentId, dbType: DbType.Int32, direction: ParameterDirection.Input);
            param.Add("@TradeId", TradeStatusId, dbType: DbType.Int32, direction: ParameterDirection.Input);

            var res = await _db.QueryMultipleAsync("[dbo].[InstrumentMaster_GETById]", param, commandType: CommandType.StoredProcedure).ConfigureAwait(false);

            instrumentMasterDDViewModels.AddRange(res.Read<MasterInstrumentViewModel>());

            return instrumentMasterDDViewModels;
        }
        public async Task<List<SymbolGroupViewModel>> GetAllSymbolGroup()
        {
            var result = await _db.QueryAsync<SymbolGroupViewModel>(
               @"SELECT Id,[Group] FROM SymbolGroup"
               );

            return result.ToList();
        }
        public async Task<Tuple<List<InstrumentSpecificationRule>, long>> GetAllSpecificationRule(PageParam pageParam, string search)
        {
            List<InstrumentSpecificationRule> instrumentMasterModel = new();
            var param = new DynamicParameters();
            param.Add("@Offset", pageParam.Offset, dbType: DbType.Int64, direction: ParameterDirection.Input);
            param.Add("@Limit", pageParam.Limit, dbType: DbType.Int64, direction: ParameterDirection.Input);
            param.Add("@Search", search, dbType: DbType.String, direction: ParameterDirection.Input);


            var res = await _db.QueryMultipleAsync("[dbo].[Import_SpecificationRule_All]", param, commandType: CommandType.StoredProcedure).ConfigureAwait(false);

            instrumentMasterModel.AddRange(res.Read<InstrumentSpecificationRule>());
            long no = res.Read<long>().SingleOrDefault();
            var tuple = new Tuple<List<InstrumentSpecificationRule>, long>(instrumentMasterModel, no);
            return tuple;
        }
        public async Task<InstrumentSpecificationRule> GetSpecificationRuleByID(int Id)
        {
            InstrumentSpecificationRule response = new InstrumentSpecificationRule();
            var param = new DynamicParameters();
            param.Add("@Id", Id, dbType: DbType.Int64, direction: ParameterDirection.Input);
            var res = await _db.QueryMultipleAsync("[SpecificationRule_GetById]", param, commandType: CommandType.StoredProcedure);
            response = res.Read<InstrumentSpecificationRule>().FirstOrDefault();

            return response;
        }



        public async Task<List<InstrumentMaster>> GetInstrumentListAll()
        {
            var result = await _db.QueryAsync<InstrumentMaster>(
               @"SELECT Id,Name FROM InstrumentMaster"
               );

            return result.ToList();
        }

        public async Task<List<SymbolGroupViewModel>> GetSymbolListALL()
        {
            var result = await _db.QueryAsync<SymbolGroupViewModel>(
               @"SELECT Id,[Group] FROM SymbolGroup"
               );

            return result.ToList();
        }




    }
}
