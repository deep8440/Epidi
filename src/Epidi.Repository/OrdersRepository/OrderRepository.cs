using Dapper;
using Epidi.Models.DBConnection;
using Epidi.Models.Model;
using Epidi.Models.Paging;
using Epidi.Models.ViewModel.Common;
using Epidi.Models.ViewModel.Country;
using Epidi.Models.ViewModel.MasterLP;
using Epidi.Models.ViewModel.PayInoutRequest;
using Epidi.Repository.ConnectionProvider;
using Epidi.Repository.MasterLP;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Epidi.Repository.OrdersRepository
{
    public class OrderRepository : RepositoryBase, IOrderRepository
    {
        public OrderRepository(IOptions<ConnectionSettings> connectionOptions,
            IConnectionProvider connectionProvider) : base(connectionOptions: connectionOptions,
                connectionProvider: connectionProvider)
        {
        }

        public Tuple<List<Order>,long> GetOrders(PageParam pageParam,int MasterInstrumentId,DateTime? OpenTiming,DateTime? CloseTiming,int UserId,int CountryId,decimal Price,decimal SpecificPrice, string search1)
        {
            List<Order> response = new();
            var vParams = new DynamicParameters();
            vParams.Add("@Offset", pageParam.Offset, dbType: DbType.Int64, direction: ParameterDirection.Input);
            vParams.Add("@Limit", pageParam.Limit, dbType: DbType.Int64, direction: ParameterDirection.Input);
            vParams.Add("@MasterInstrumentId", MasterInstrumentId, dbType: DbType.Int32, direction: ParameterDirection.Input);
            vParams.Add("@UserId", UserId, dbType: DbType.Int32, direction: ParameterDirection.Input);
            vParams.Add("@CountryId", CountryId, dbType: DbType.Int32, direction: ParameterDirection.Input);
            vParams.Add("@Price", Price, dbType: DbType.Decimal, direction: ParameterDirection.Input);
            vParams.Add("@SpecificPrice", SpecificPrice, dbType: DbType.Decimal, direction: ParameterDirection.Input);
            vParams.Add("@OpenTiming", OpenTiming, dbType: DbType.DateTime, direction: ParameterDirection.Input);
            vParams.Add("@closeTiming", CloseTiming, dbType: DbType.DateTime, direction: ParameterDirection.Input);
            vParams.Add("@Search", search1, dbType: DbType.String, direction: ParameterDirection.Input);

            var res = _db.QueryMultiple("[Get_Orders]", vParams, commandType: CommandType.StoredProcedure);

            response.AddRange(res.Read<Order>());
            long no = res.Read<long>().SingleOrDefault();
            var tuple = new Tuple<List<Order>, long>(response, no);
            return tuple;
            
        }
        public async Task<List<Order>> GetAllOrders()
        {
            List<Order> response = new();
            var vParams = new DynamicParameters();
            vParams.Add("@Offset", 1, dbType: DbType.Int64, direction: ParameterDirection.Input);
            vParams.Add("@Limit", 100000, dbType: DbType.Int64, direction: ParameterDirection.Input);

            var res = _db.QueryMultiple("[dbo].[Get_Orders]", vParams, commandType: CommandType.StoredProcedure);


            response = res.Read<Order>().ToList();

            return response;

        }
        public ResponseViewModel updateFields(string fieldName, string fieldValue,int ID)
        {
            var response = new ResponseViewModel();

            var vParams = new DynamicParameters();
            vParams.Add("@fieldName", fieldName, dbType: DbType.String, direction: ParameterDirection.Input);
            vParams.Add("@value", fieldValue, dbType: DbType.String, direction: ParameterDirection.Input);
            vParams.Add("@ID", ID, dbType: DbType.Int32, direction: ParameterDirection.Input);

            var res = _db.QueryMultiple("[UpdateOrder_Fields]", vParams, commandType: CommandType.StoredProcedure);

            response = res.Read<ResponseViewModel>().FirstOrDefault() ?? new ResponseViewModel();

            return response;
        }
        public ResponseViewModel updateGridFields(int OrderId, double Closeprice, double OpenUnits, double OpenPrice)
        {
            var response = new ResponseViewModel();

            var vParams = new DynamicParameters();
            vParams.Add("@OrderId", OrderId, dbType: DbType.Int32, direction: ParameterDirection.Input);
            vParams.Add("@Closeprice", Closeprice, dbType: DbType.Decimal, direction: ParameterDirection.Input);
            vParams.Add("@OpenUnits", OpenUnits, dbType: DbType.Decimal, direction: ParameterDirection.Input);
            vParams.Add("@OpenPrice", OpenPrice, dbType: DbType.Decimal, direction: ParameterDirection.Input);
            

            var res = _db.QueryMultiple("[UpdateOrder_GridFields]", vParams, commandType: CommandType.StoredProcedure);

            response = res.Read<ResponseViewModel>().FirstOrDefault() ?? new ResponseViewModel();

            return response;
        }
        public ResponseViewModel removeOrders(int ID)
        {
            var response = new ResponseViewModel();
            var vParams = new DynamicParameters();
            vParams.Add("@ID", ID);
            var res = _db.QueryMultiple("[Orders_Remove]", vParams, commandType: CommandType.StoredProcedure);

            response = res.Read<ResponseViewModel>().FirstOrDefault()??new ResponseViewModel();

            return response;
        }
        public async Task<long> ImportOrderData_UpsertByDt(DataTable dtOrderData)
        {
            long roleId = 0;
            try
            {
                SqlConnection con = new SqlConnection(_connectionOptions.ConnectionString);
                await con.OpenAsync();

                SqlCommand sqlCommand = new SqlCommand("ImportOrderData_UpsertByDt", con);
                sqlCommand.CommandType = CommandType.StoredProcedure;

                SqlParameter sqlParam = sqlCommand.Parameters.AddWithValue("@paramTable", dtOrderData);
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
    }
}
