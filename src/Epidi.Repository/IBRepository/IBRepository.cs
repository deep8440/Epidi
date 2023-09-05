using Dapper;
using Epidi.Models.DBConnection;
using Epidi.Models.Paging;
using Epidi.Models.ViewModel.BDM;
using Epidi.Models.ViewModel.CommissionRule;
using Epidi.Models.ViewModel.Common;
using Epidi.Models.ViewModel.IB;
using Epidi.Models.ViewModel.IBCommissionMarkUpSettingInstrumentWise;
using Epidi.Models.ViewModel.IBLimit;
using Epidi.Models.ViewModel.LegalEntity;
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

namespace Epidi.Repository.IBRepository
{
    public class IBRepository : RepositoryBase, IIBRepository
    {
        public IBRepository(IOptions<ConnectionSettings> connectionOptions,
            IConnectionProvider connectionProvider) : base(connectionOptions: connectionOptions,
                connectionProvider: connectionProvider)
        {
        }
        public async Task<IBViewModel> Upsert(IBViewModel model)
        {
            IBViewModel response = new();

            var vParams = new DynamicParameters();
            vParams.Add("@Id", model.Id);
            vParams.Add("@Name", model.Name);
            vParams.Add("@BDMId", model.BDMId);
            vParams.Add("@LegalEntityId", model.LegalEntityId);
            vParams.Add("@ParentIBId", model.ParentIBId);
            vParams.Add("@CountryId", model.CountryId);
            vParams.Add("@ParentCommissionPercentageRebate", model.ParentCommissionPercentageRebate);
            vParams.Add("@BDMCommissionPercentage", model.BDMCommissionPercentage);
            var res = await _db.QueryMultipleAsync("[IB_Upsert]", vParams, commandType: CommandType.StoredProcedure);

            response = res.Read<IBViewModel>().FirstOrDefault();
            return response;
        }
        public async Task<ResponseViewModel> Import(Tuple<string, decimal, decimal, decimal, decimal, decimal, decimal, Tuple<int>> model, int BDMId)
        {
            try
            {
                ResponseViewModel response = new();
                IBCommisionMarkUpSettingInstrumentWiseViewModel checkResponse = new();

                var vCheckParams = new DynamicParameters();
                vCheckParams.Add("@IBID", model.Rest.Item1);
                vCheckParams.Add("@Name", model.Item1);
                var checkRes = await _db.QueryMultipleAsync("[Check_IBCommissionMarkUpSettingInstrumentWise_Upsert]", vCheckParams, commandType: CommandType.StoredProcedure);
                checkResponse = checkRes.Read<IBCommisionMarkUpSettingInstrumentWiseViewModel>().FirstOrDefault();

                if (checkResponse == null)
                {
                    var vParams = new DynamicParameters();
                    vParams.Add("@IBId", model.Rest.Item1);
                    vParams.Add("@Instrument", model.Item1);
                    vParams.Add("@BDMId", BDMId);
                    vParams.Add("@MarkUp1000", model.Item2);
                    vParams.Add("@MarkUpPer", model.Item3);
                    vParams.Add("@BuySwap1000", model.Item4);
                    vParams.Add("@BuySwapPer", model.Item5);
                    vParams.Add("@SELLSwapPer1000", model.Item6);
                    vParams.Add("@SellSwapPer", model.Item7);

                    var res = await _db.QueryMultipleAsync("[Upsert_IBCommissionMarkUpSettingInstrumentWise]", vParams, commandType: CommandType.StoredProcedure);

                    response = res.Read<ResponseViewModel>().FirstOrDefault();
                }


                return response;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public Tuple<List<IBViewModel>, long> GetAll(PageParam pageParam, string search)
        {
            List<IBViewModel> response = new List<IBViewModel>();

            var vParams = new DynamicParameters();
            vParams.Add("@Offset", pageParam.Offset, dbType: DbType.Int64, direction: ParameterDirection.Input);
            vParams.Add("@Limit", pageParam.Limit, dbType: DbType.Int64, direction: ParameterDirection.Input);
            vParams.Add("@Search", search, dbType: DbType.String, direction: ParameterDirection.Input);

            var res = _db.QueryMultiple("[IB_GetAll]", vParams, commandType: CommandType.StoredProcedure);

            response.AddRange(res.Read<IBViewModel>());
            long no = res.Read<long>().SingleOrDefault();
            var tuple = new Tuple<List<IBViewModel>, long>(response, no);
            return tuple;
        }

        //public Tuple<List<IBViewModel>, long> GetAllBy_BDMID(PageParam pageParam, string search, string BDMID)
        //{
        //    List<IBViewModel> response = new List<IBViewModel>();

        //    var vParams = new DynamicParameters();
        //    vParams.Add("@Offset", pageParam.Offset, dbType: DbType.Int64, direction: ParameterDirection.Input);
        //    vParams.Add("@Limit", pageParam.Limit, dbType: DbType.Int64, direction: ParameterDirection.Input);
        //    vParams.Add("@Search", search, dbType: DbType.String, direction: ParameterDirection.Input);
        //    vParams.Add("@BDMID", BDMID, dbType: DbType.Int32, direction: ParameterDirection.Input);

        //    var res = _db.QueryMultiple("[IB_GetAllByBDM_Id]", vParams, commandType: CommandType.StoredProcedure);

        //    response.AddRange(res.Read<IBViewModel>());
        //    long no = res.Read<long>().SingleOrDefault();
        //    var tuple = new Tuple<List<IBViewModel>, long>(response, no);
        //    return tuple;
        //}
        public async Task<IBViewModel> GetById(int Id)
        {
            IBViewModel response = new();

            var vParams = new DynamicParameters();
            vParams.Add("@Id", Id);
            var res = await _db.QueryMultipleAsync("[IB_GetById]", vParams, commandType: CommandType.StoredProcedure);

            response = res.Read<IBViewModel>().FirstOrDefault();

            return response;

        }

        public async Task<List<IBCommisionMarkUpSettingInstrumentWiseViewModel>> GetIBCommitionById(int Id)
        {
            List<IBCommisionMarkUpSettingInstrumentWiseViewModel> response = new();

            var vParams = new DynamicParameters();
            vParams.Add("@RuleId", Id);
            var res = await _db.QueryMultipleAsync("[GET_IB_Commission_ById]", vParams, commandType: CommandType.StoredProcedure);

            response.AddRange(res.Read<IBCommisionMarkUpSettingInstrumentWiseViewModel>());

            return response;
        }

        public List<ResponseViewModel> DeleteIB(int Id)
        {

            List<ResponseViewModel> response = new List<ResponseViewModel>();

            var param = new DynamicParameters();
            param.Add("@Id", Id, dbType: DbType.Int32, direction: ParameterDirection.Input);

            var res = _db.QueryMultiple("[dbo].[IB_Delete]", param, commandType: CommandType.StoredProcedure);


            response = res.Read<ResponseViewModel>().ToList();

            return response;
        }

        public async Task<int> DeleteIBRuleInstrumentById(int IBId, int MasterInstrumentalID)
        {
            var vParams = new DynamicParameters();
            vParams.Add("@IBId", IBId);
            vParams.Add("@MasterInstrumentalID", MasterInstrumentalID);
            var res = await _db.QueryMultipleAsync("[IB_RuleInstrument_Delete]", vParams, commandType: CommandType.StoredProcedure);

            return 1;
        }

        public async Task<IBCommissionInstrument> GetIBRuleInstrumentById(int IBId, int MasterInstrumentalID)
        {
            IBCommissionInstrument IBCommissionInstrument = new IBCommissionInstrument();
            var param = new DynamicParameters();
            param.Add("@IBId", IBId);
            param.Add("@MasterInstrumentalID", MasterInstrumentalID);

            var res = _db.QueryMultiple("[dbo].[IBCommissionInstrument_ById]", param, commandType: CommandType.StoredProcedure);


            IBCommissionInstrument = res.Read<IBCommissionInstrument>().FirstOrDefault();
            return IBCommissionInstrument;
        }

        public async Task<IBCommissionInstrument> SaveIBCommissionInstrument(IBCommissionInstrument model)
        {

            IBCommissionInstrument response = new IBCommissionInstrument();

            var vParams = new DynamicParameters();
            vParams.Add("@Id", model.Id);
            vParams.Add("@MasterInstrumentId", model.MasterInstrumentId);
            vParams.Add("@MasterInstrumentName", model.MasterInstrumentName);
            vParams.Add("@MarkUpPer1000", model.MarkUpPer1000);
            vParams.Add("@IBID", model.IBID);
            vParams.Add("@MarkUpPer", model.MarkUpPer);
            vParams.Add("@BuySwapPer1000", model.BuySwapPer1000);
            vParams.Add("@BuySwapPer", model.BuySwapPer);
            vParams.Add("@SellSwapPer1000", model.SellSwapPer1000);
            vParams.Add("@SellSwapPer", model.SellSwapPer);
            vParams.Add("@IsActive", model.IsActive);

            var res = _db.QueryMultiple("[dbo].[Update_IBCommissionMarkUpSettingInstrumentWise]", vParams, commandType: CommandType.StoredProcedure);
            response = res.Read<IBCommissionInstrument>().FirstOrDefault();

            return response;

        }

        public async Task<ResponseViewModel> SaveIBInstrumentRule(IBCommissionInstrument model)
        {
            ResponseViewModel response = new ResponseViewModel();

            
                var vParams = new DynamicParameters();
                vParams.Add("@Instrument", model.MasterInstrumentId);
                vParams.Add("@MarkUp1000", model.MarkUpPer1000);
                vParams.Add("@MarkUpPer", model.MarkUpPer);
                vParams.Add("@BuySwap1000", model.BuySwapPer1000);
                vParams.Add("@BuySwapPer", model.BuySwapPer);
                vParams.Add("@SELLSwapPer1000", model.SellSwapPer1000);
                vParams.Add("@SellSwapPer", model.SellSwapPer);
                vParams.Add("@IBId", model.IBID);
                var res = await _db.QueryMultipleAsync("[AddIBCommissionInstrument]", vParams, commandType: CommandType.StoredProcedure);

                response = res.Read<ResponseViewModel>().FirstOrDefault();
                return response;
        }

        public async Task<bool> CheckIBRule(int Id, int MasterInstrumentId)
        {
            bool isAvailable;
            try
            {
                SqlConnection con = new SqlConnection(_connectionOptions.ConnectionString);
                await con.OpenAsync();

                SqlCommand sqlCommand = new SqlCommand("Check_IBCommissionInstrument", con);
                sqlCommand.CommandType = CommandType.StoredProcedure;

                SqlParameter sqlParam = new SqlParameter();

                sqlParam = sqlCommand.Parameters.AddWithValue("@Id", Id);
                sqlParam.SqlDbType = SqlDbType.Int;
                sqlParam = sqlCommand.Parameters.AddWithValue("@MasterInstrumentId", MasterInstrumentId);
                sqlParam.SqlDbType = SqlDbType.Int;

                isAvailable = Convert.ToBoolean(await sqlCommand.ExecuteScalarAsync());

                await con.CloseAsync();
                return isAvailable;
            }
            catch (Exception EX)
            {
                throw;
            }
        }

        public async Task<IBLimitValidateRespViewModel> ValidateIBLimit(IBLimitValidateViewModel model)
        {
            IBLimitValidateRespViewModel iBLimitValidateViewModel = new IBLimitValidateRespViewModel();
            try
            {
                var vParams = new DynamicParameters();
                vParams.Add("@MasterInstrumentId", model.MasterInstrumentId);
                vParams.Add("@Type", model.Type);
                vParams.Add("@Price", model.Price);
                var res = await _db.QueryMultipleAsync("[ValidateIBLimit]", vParams, commandType: CommandType.StoredProcedure);

                iBLimitValidateViewModel = res.Read<IBLimitValidateRespViewModel>().FirstOrDefault();
                return iBLimitValidateViewModel;

            }
            catch (Exception EX)
            {
                throw;
            }
        }
    }
}
