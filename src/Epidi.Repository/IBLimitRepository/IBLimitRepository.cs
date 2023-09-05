using Dapper;
using Epidi.Models.DBConnection;
using Epidi.Models.Paging;

using Epidi.Models.ViewModel.CommissionRule;
using Epidi.Models.ViewModel.Common;
using Epidi.Repository.ConnectionProvider;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Epidi.Repository.IBRepository;
using Epidi.Models.ViewModel.IB;
using Epidi.Models.ViewModel.MarginRuleViewModel;
using Epidi.Models.ViewModel.TermsCondition;
using Epidi.Models.ViewModel.Question;
using Epidi.Models.ViewModel.Instrument;
using Epidi.Models.ViewModel.SkipRule;
using Epidi.Models.ViewModel.Country;
using Microsoft.VisualBasic;
using System.Data.SqlClient;
using Epidi.Models.ViewModel.SymbolGroup;

namespace Epidi.Repository.IBLimitRepository
{
    public class IBLimitRepository : RepositoryBase, IIBLimitRepository
    {

        public IBLimitRepository(IOptions<ConnectionSettings> connectionOptions,
           IConnectionProvider connectionProvider) : base(connectionOptions: connectionOptions,
               connectionProvider: connectionProvider)
        {
        }

        public async Task<IBLimitViewModel> Limit_Upsert(IBLimitViewModel model)
        {
            IBLimitViewModel response = new();

            try
            {
                var vParams = new DynamicParameters();
                vParams.Add("@Id", model.Id);
                vParams.Add("@MasterInstrumentId", model.MasterInstrumentId);
                vParams.Add("@SymbolGroupId", model.SymbolGroupId);
                vParams.Add("@MarkUpPer1000", model.MarkUpPer1000);
                vParams.Add("@MarkUpPer", model.MarkUpPer);
                vParams.Add("@BuySwapPer1000", model.BuySwapPer1000);
                vParams.Add("@BuySwapPer", model.BuySwapPer);
                vParams.Add("@SellSwapPer1000", model.SellSwapPer1000);
                vParams.Add("@SellSwapPer", model.SellSwapPer);
                // vParams.Add("@CreatedBy", model.CreatedBy);
                // vParams.Add("@CreatedDate", model.CreatedDate);
                // vParams.Add("@UpdatedBy", model.UpdatedBy);
                // vParams.Add("@UpdatedDate", model.UpdatedDate);
                // vParams.Add("@@DeletedBy", model.DeletedBy);
                // vParams.Add("@DeletedDate", model.DeletedDate);
                // vParams.Add("@IsActive", model.IsActive);
                var res = await _db.QueryMultipleAsync("[IBLimit_Upsert]", vParams, commandType: CommandType.StoredProcedure);

                response = res.Read<IBLimitViewModel>().FirstOrDefault();
            }
            catch (Exception ex)
            {

                throw;
            }


            return response;

        }

        public async Task<IBLimitViewModel> GetBy_Id(int MasterInstrumentId)
        {
            IBLimitViewModel response = new();

            var vParams = new DynamicParameters();
            vParams.Add("@MasterInstrumentId", MasterInstrumentId);
            var res = await _db.QueryMultipleAsync("[IBLimit_GetById]", vParams, commandType: CommandType.StoredProcedure);

            response = res.Read<IBLimitViewModel>().FirstOrDefault();

            return response;
        }

        public long GetSymbolGroupId(int MasterInstrumentId)
        {
            var result = _db.Query<long>(
               @"select SymbolGroupId from InstrumentMaster i
inner join SymbolGroup g on i.SymbolGroupId = g.Id
where i.Id = @MasterInstrumentId",
               new
               {
                   @MasterInstrumentId = MasterInstrumentId
               }
               );

            return result.FirstOrDefault();
        }



        public Tuple<List<IBLimitViewModel>, long> Get_All(PageParam pageParam, string search)
        {
            List<IBLimitViewModel> response = new();

            var vParams = new DynamicParameters();
            vParams.Add("@Offset", pageParam.Offset, dbType: DbType.Int64, direction: ParameterDirection.Input);
            vParams.Add("@Limit", pageParam.Limit, dbType: DbType.Int64, direction: ParameterDirection.Input);
            vParams.Add("@Search", search, dbType: DbType.String, direction: ParameterDirection.Input);

            var res = _db.QueryMultiple("[IBLimit_GetAll]", vParams, commandType: CommandType.StoredProcedure);

            response.AddRange(res.Read<IBLimitViewModel>());
            long no = res.Read<long>().SingleOrDefault();
            var tuple = new Tuple<List<IBLimitViewModel>, long>(response, no);
            return tuple;

        }



        public List<ResponseViewModel> Delete(int Id)
        {

            List<ResponseViewModel> response = new List<ResponseViewModel>();

            var param = new DynamicParameters();
            param.Add("@Id", Id, dbType: DbType.Int32, direction: ParameterDirection.Input);

            var res = _db.QueryMultiple("[dbo].[IBLimit_Delete]", param, commandType: CommandType.StoredProcedure);


            response = res.Read<ResponseViewModel>().ToList();

            return response;
        }




        public List<IBLimitViewModel> GetIBLimit_Data()
        {

            List<IBLimitViewModel> response = new List<IBLimitViewModel>();

            var param = new DynamicParameters();

            var res = _db.QueryMultiple("[dbo].[IBLimit_GetIBLimit_Data]", param, commandType: CommandType.StoredProcedure);


            response = res.Read<IBLimitViewModel>().ToList();

            return response;
        }




        public async Task<long> ImportIBLimitData_UpsertByDt(DataTable dt_IBLimitData)
        {
            try
            {

                //long roleId = 0;
                SqlConnection con = new SqlConnection(_connectionOptions.ConnectionString);
                await con.OpenAsync();

                SqlCommand sqlCommand = new SqlCommand("ImportIBLimitData_UpsertByDt", con);
                sqlCommand.CommandType = CommandType.StoredProcedure;

                SqlParameter sqlParam = sqlCommand.Parameters.AddWithValue("@paramTable", dt_IBLimitData);
                sqlParam.SqlDbType = SqlDbType.Structured;


                //roleId = Convert.ToInt32(await sqlCommand.ExecuteScalarAsync());

                await sqlCommand.ExecuteNonQueryAsync();

                await con.CloseAsync();
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }

        }

        public List<IBLimitViewModel> GetIBLimit_All_Data()
        {

            List<IBLimitViewModel> response = new List<IBLimitViewModel>();

            var param = new DynamicParameters();

            var res = _db.QueryMultiple("[dbo].[IBLimit_All_Data]", param, commandType: CommandType.StoredProcedure);


            response = res.Read<IBLimitViewModel>().ToList();

            return response;
        }
    }

}
