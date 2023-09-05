using Dapper;
using Epidi.Models.DBConnection;
using Epidi.Models.Paging;
using Epidi.Models.ViewModel.Common;
using Epidi.Models.ViewModel.Country;
using Epidi.Models.ViewModel.IBCommissionMarkUpSettingInstrumentWise;
using Epidi.Models.ViewModel.Instrument;
using Epidi.Models.ViewModel.Margin;
using Epidi.Repository.ConnectionProvider;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Repository.MarginRepository
{
    public class MarginRepository : RepositoryBase, IMarginRepository
    {
        public MarginRepository(IOptions<ConnectionSettings> connectionOptions,
           IConnectionProvider connectionProvider) : base(connectionOptions: connectionOptions,
               connectionProvider: connectionProvider)
        {
        }
        public Tuple<List<MarginViewModel>, long> Get_All(PageParam pageParam, string search)
        {
            List<MarginViewModel> marginViewModels = new List<MarginViewModel>();
            var param = new DynamicParameters();
            param.Add("@Offset", pageParam.Offset, dbType: DbType.Int64, direction: ParameterDirection.Input);
            param.Add("@Limit", pageParam.Limit, dbType: DbType.Int64, direction: ParameterDirection.Input);
            param.Add("@Search", search, dbType: DbType.String, direction: ParameterDirection.Input);

            var res = _db.QueryMultiple("[dbo].[GetAll_Margin]", param, commandType: CommandType.StoredProcedure);

            marginViewModels.AddRange(res.Read<MarginViewModel>());
            long no = res.Read<long>().SingleOrDefault();
            var tuple = new Tuple<List<MarginViewModel>, long>(marginViewModels, no);
            return tuple;
        }
        public List <ResponseViewModel> DeleteMargin(int id)
        {



            List<ResponseViewModel> response = new List<ResponseViewModel>();

            var vParams = new DynamicParameters();
                vParams.Add("@Id", id);
                var res = _db.QueryMultiple("[Margin_Delete]", vParams, commandType: CommandType.StoredProcedure);

                response = res.Read<ResponseViewModel>().ToList();

                return response;
          
        }

        public async Task<List<DropDownItems>> GetMarginTypes()
        {
            List<DropDownItems> response = new List<DropDownItems>();

            var res = await _db.QueryMultipleAsync("[Get_MarginTypes]", commandType: CommandType.StoredProcedure);

            response = res.Read<DropDownItems>().ToList();

            return response;
            //var result = await _db.QueryAsync<DropDownItems>(@"SELECT margintypeid as Id,Type as Name FROM margintype");
            //return result.ToList();
        }
        public async Task<List<DropDownItems>> GetInstrumentToConvertTypes()
        {
            List<DropDownItems> response = new List<DropDownItems>();

            var res = await _db.QueryMultipleAsync("[Get_InstrumentToConvertTypes]", commandType: CommandType.StoredProcedure);

            response = res.Read<DropDownItems>().ToList();

            return response;
            //var result = await _db.QueryAsync<DropDownItems>(@"SELECT Id,Name FROM CurrencyMaster WHERE IsActive = 1");
            //return result.ToList();
        }
        public async Task<List<DropDownItems>> GetInstruments()
        {
            List<DropDownItems> response = new List<DropDownItems>();

            var res = await _db.QueryMultipleAsync("[Get_Instruments]", commandType: CommandType.StoredProcedure);

            response = res.Read<DropDownItems>().ToList();

            return response;
            //var result = await _db.QueryAsync<DropDownItems>(@"SELECT id as Id,Name FROM InstrumentMaster");
            //return result.ToList();
        }
        public async Task<List<DropDownItems>> GetSymbolGroups()
        {
            List<DropDownItems> response = new List<DropDownItems>();

            var res = await _db.QueryMultipleAsync("[Get_SymbolGroups]", commandType: CommandType.StoredProcedure);

            response = res.Read<DropDownItems>().ToList();

            return response;
            //var result = await _db.QueryAsync<DropDownItems>(@"SELECT Id,[Group] as Name FROM SymbolGroup");
            //return result.ToList();
        }
        public async Task<ResponseViewModel> Margin_Upsert(MarginViewModel marginViewModel)
        {
            try
            {
                ResponseViewModel response = new ResponseViewModel();
                var vParams = new DynamicParameters();
                vParams.Add("@MarginId", marginViewModel.MarginId);
                vParams.Add("@MarginTypeId", marginViewModel.MarginTypeId);
                vParams.Add("@InstrumentEnd", marginViewModel.InstrumentEnd);
                vParams.Add("@MasterInstrumentId", marginViewModel.MasterInstrumentId);
                vParams.Add("@SymbolGroupId", marginViewModel.SymbolGroupId);
                vParams.Add("@SymbolCurrencyFrom", marginViewModel.SymbolCurrencyFrom);
                vParams.Add("@SymbolCurrencyTo", marginViewModel.SymbolCurrencyTo);
                vParams.Add("@InstrumentConversiontoUSD", marginViewModel.InstrumentConversiontoUSD);
                vParams.Add("@InstrumentConversiontoEUR", marginViewModel.InstrumentConversiontoEUR);
                vParams.Add("@InstrumentConversiontoINR", marginViewModel.InstrumentConversiontoINR);
                vParams.Add("@ManualInstrumentConversiontoUSD", marginViewModel.ManualInstrumentConversiontoUSD);
                vParams.Add("@ManualInstrumentConversiontoEUR", marginViewModel.ManualInstrumentConversiontoEUR);
                vParams.Add("@ManualInstrumentConversiontoINR", marginViewModel.ManualInstrumentConversiontoINR);

                var res = await _db.QueryMultipleAsync("[Margin_Upsert]", vParams, commandType: CommandType.StoredProcedure);
                response = res.Read<ResponseViewModel>().FirstOrDefault();
                
                return response;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<ResponseViewModel> MarginDetails_Upsert(List<MarginDetails> model,int MarginId)
        {
            try
            {
                ResponseViewModel response = new ResponseViewModel();
                foreach (var item in model)
                {
                    var vParams1 = new DynamicParameters();
                    vParams1.Add("@Id", item.Id);
                    vParams1.Add("@MarginId", MarginId);
                    vParams1.Add("@InstrumentId", item.InstrumentId);
                    vParams1.Add("@ManualConversion", item.ManualConversion);
                    vParams1.Add("@InstrumentToConvertId", item.InstrumentToConvertId);
                    var res1 = await _db.QueryMultipleAsync("[MarginDetails_Upsert]", vParams1, commandType: CommandType.StoredProcedure);
                    response = res1.Read<ResponseViewModel>().FirstOrDefault();
                }
                return response;
            }
            catch (Exception ex)
            {
                throw;
            }            
        }
        public async Task<List<MarginDetails>> GetMarginDetails_All(int MarginId)
        {
            List<MarginDetails> marginViewModels = new List<MarginDetails>();
            var param = new DynamicParameters();           
            param.Add("@MarginId", MarginId, dbType: DbType.Int32, direction: ParameterDirection.Input);
            
            var res =await _db.QueryMultipleAsync("[GetAll_MarginDetails]", param, commandType: CommandType.StoredProcedure);

            marginViewModels = res.Read<MarginDetails>().ToList();
            return marginViewModels;
        }

        public async Task<long> ImportMargin(DataTable dataTable)
        {
            SqlConnection con = new SqlConnection(_connectionOptions.ConnectionString);
            await con.OpenAsync();

            SqlCommand sqlCommand = new SqlCommand("ImportExcelMargin_Add", con);
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
