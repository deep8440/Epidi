using Dapper;
using Epidi.Models.DBConnection;
using Epidi.Models.Paging;
using Epidi.Models.ViewModel.Common;
using Epidi.Models.ViewModel.IBCommissionMarkUpSettingInstrumentWise;
using Epidi.Models.ViewModel.LegalEntity;
using Epidi.Repository.ConnectionProvider;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Repository.IBCommisionMarkUpSettingInstrumentWise
{
    public class IBCommisionMarkUpSettingInstrumentWise : RepositoryBase, IIBCommisionMarkUpSettingInstrumentWise
    {
        public IBCommisionMarkUpSettingInstrumentWise(IOptions<ConnectionSettings> connectionOptions,
            IConnectionProvider connectionProvider) : base(connectionOptions: connectionOptions,
                connectionProvider: connectionProvider)
        {
        }

        public Tuple<List<IBCommisionMarkUpSettingInstrumentWiseViewModel>, long> GetAll(PageParam pageParam, string search)
        {
            List<IBCommisionMarkUpSettingInstrumentWiseViewModel> response = new List<IBCommisionMarkUpSettingInstrumentWiseViewModel>();

            var vParams = new DynamicParameters();
            vParams.Add("@Offset", pageParam.Offset, dbType: DbType.Int64, direction: ParameterDirection.Input);
            vParams.Add("@Limit", pageParam.Limit, dbType: DbType.Int64, direction: ParameterDirection.Input);
            vParams.Add("@Search", search, dbType: DbType.String, direction: ParameterDirection.Input);

            var res = _db.QueryMultiple("[IBCommInstWise_GetAll]", vParams, commandType: CommandType.StoredProcedure);

            response.AddRange(res.Read<IBCommisionMarkUpSettingInstrumentWiseViewModel>());
            long no = res.Read<long>().SingleOrDefault();
            var tuple = new Tuple<List<IBCommisionMarkUpSettingInstrumentWiseViewModel>, long>(response, no);
            return tuple;
        }
        public async Task<ResponseViewModel> Upsert(IBCommisionMarkUpSettingInstrumentWiseViewModel model)
        {
            ResponseViewModel response = new();
            IBCommisionMarkUpSettingInstrumentWiseViewModel checkResponse = new();
            //List<LegalEntityViewModel> response = new List<LegalEntityViewModel>();

            try
            {
                var vCheckParams = new DynamicParameters();
                vCheckParams.Add("@IBID", model.IBID);
                vCheckParams.Add("@Name", model.InstrumentalName);
                var checkRes = await _db.QueryMultipleAsync("[Check_IBCommissionMarkUpSettingInstrumentWise_Upsert]", vCheckParams, commandType: CommandType.StoredProcedure);
                checkResponse = checkRes.Read<IBCommisionMarkUpSettingInstrumentWiseViewModel>().FirstOrDefault();
                if (checkResponse == null)
                {
                    var vParams = new DynamicParameters();
                    vParams.Add("@Id", model.Id);
                    vParams.Add("@MasterInstrumentalId", model.MasterInstrumentalId);
                    vParams.Add("@Name", model.InstrumentalName);
                    vParams.Add("@MarkUpPer1000", model.MarkUpPer1000);
                    vParams.Add("@IBID", model.IBID);
                    vParams.Add("@BDMId", model.BDMID);
                    vParams.Add("@MarkUpPer", model.MarkUpPer);
                    vParams.Add("@BuySwapPer1000", model.BuySwapPer1000);
                    vParams.Add("@BuySwapPer", model.BuySwapPer);
                    vParams.Add("@SellSwapPer1000", model.SellSwapPer1000);
                    vParams.Add("@SellSwapPer", model.SellSwapPer);

                    vParams.Add("@MarkUpCommssion", model.MarkUpCommssion);
                    vParams.Add("@BuySwapCommission", model.BuySwapCommission);
                    vParams.Add("@SellSwapCommission", model.SellSwapCommission);
                    vParams.Add("@MarkUpTotal", model.MarkUpTotal);
                    vParams.Add("@BuySwapTotal", model.BuySwapTotal);
                    vParams.Add("@SellSwapTotal", model.SellSwapTotal);
                    vParams.Add("@CreatedBy", model.CreatedBy);

                    vParams.Add("@IsActive", model.IsActive);

                    var res = await _db.QueryMultipleAsync("[IBCommissionMarkUpSettingInstrumentWise_Upsert]", vParams, commandType: CommandType.StoredProcedure);
                    //response.AddRange(res.Read<ResponseViewModel>());
                    //long no = res.Read<long>().SingleOrDefault();
                    response = res.Read<ResponseViewModel>().FirstOrDefault();
                }
                else
                {
                    response.Code = 404;
                    response.Message = "Already IB exists";
                }
                
                return response;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }

        public async Task<List<CommisionMarkUpSetting>> GetIBMCommision_IBID(int IBId, int instrument_filter)
        {
            List<CommisionMarkUpSetting> response1 = new();

            var vParams1 = new DynamicParameters();
            vParams1.Add("@IBID", IBId);
            vParams1.Add("@instrument_filter", instrument_filter);
            var res1 = await _db.QueryMultipleAsync("[IBCommisionInstrumentWise_GetByIBID]", vParams1, commandType: CommandType.StoredProcedure);

            response1 = res1.Read<CommisionMarkUpSetting>().ToList();

            return response1;

        }
        public Tuple<List<CommisionMarkUpSetting>, long> GetAll_Commission(PageParam pageParam, string search)
        {
            List<CommisionMarkUpSetting> response = new List<CommisionMarkUpSetting>();

            var vParams = new DynamicParameters();
            vParams.Add("@Offset", pageParam.Offset, dbType: DbType.Int64, direction: ParameterDirection.Input);
            vParams.Add("@Limit", pageParam.Limit, dbType: DbType.Int64, direction: ParameterDirection.Input);
            vParams.Add("@Search", search, dbType: DbType.String, direction: ParameterDirection.Input);

            var res = _db.QueryMultiple("[CommisionMarkUpSetting_GetAll]", vParams, commandType: CommandType.StoredProcedure);

            response.AddRange(res.Read<CommisionMarkUpSetting>());
            long no = res.Read<long>().SingleOrDefault();
            var tuple = new Tuple<List<CommisionMarkUpSetting>, long>(response, no);
            return tuple;
        }
        public async Task<long> CommisionMarkUpSetting_UpsertByDt(DataTable dtRuleInstrument)
        {
            long roleId = 0;
            try
            {
                SqlConnection con = new SqlConnection(_connectionOptions.ConnectionString);
                await con.OpenAsync();

                SqlCommand sqlCommand = new SqlCommand("CommisionMarkUpSetting_UpsertByDt", con);
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
    }
}
