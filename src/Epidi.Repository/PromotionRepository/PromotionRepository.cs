using Dapper;
using Epidi.Models.DBConnection;
using Epidi.Models.Model;
using Epidi.Models.Paging;
using Epidi.Models.ViewModel.Common;
using Epidi.Models.ViewModel.LeverageRule;
using Epidi.Models.ViewModel.RuleCondition;
using Epidi.Models.ViewModel.RuleInstrument;
using Epidi.Repository.ConnectionProvider;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Repository.PromotionRepository
{
    public class PromotionRepository : RepositoryBase, IPromotionRepository
    {
        public PromotionRepository(IOptions<ConnectionSettings> connectionOptions,
            IConnectionProvider connectionProvider) : base(connectionOptions: connectionOptions,
                connectionProvider: connectionProvider)
        {
        }
        public Promotion GetPromotion(int id)
        {
            var promotion = new Promotion();
            var vParams_UR = new DynamicParameters();
            vParams_UR.Add("@PromotionId", id);
            var res_UR = _db.QueryMultiple("[GetById_Promotion]", vParams_UR, commandType: CommandType.StoredProcedure);
            var response = res_UR.Read<Promotion>().SingleOrDefault();
            return response;
        }
        public Tuple<List<CreditPromotion>, long> creditPromotions(PageParam pageParam, string search, int Id)
        {
            List<CreditPromotion> response = new();

            var vParams = new DynamicParameters();
            vParams.Add("@Offset", pageParam.Offset, dbType: DbType.Int64, direction: ParameterDirection.Input);
            vParams.Add("@Limit", pageParam.Limit, dbType: DbType.Int64, direction: ParameterDirection.Input);
            vParams.Add("@Id", Id, dbType: DbType.Int64, direction: ParameterDirection.Input);
            vParams.Add("@Search", search, dbType: DbType.String, direction: ParameterDirection.Input);
            var res = _db.QueryMultiple("[GetAll_CreditPromotion]", vParams, commandType: CommandType.StoredProcedure);

            response.AddRange(res.Read<CreditPromotion>());
            long no = res.Read<long>().SingleOrDefault();
            var tuple = new Tuple<List<CreditPromotion>, long>(response, no);
            return tuple;
        }
        public Tuple<List<RebatePromotion>, long> rebatePromotions(PageParam pageParam, string search, int Id)
        {
            List<RebatePromotion> response = new();

            var vParams = new DynamicParameters();
            vParams.Add("@Offset", pageParam.Offset, dbType: DbType.Int64, direction: ParameterDirection.Input);
            vParams.Add("@Limit", pageParam.Limit, dbType: DbType.Int64, direction: ParameterDirection.Input);
            vParams.Add("@Id", Id, dbType: DbType.Int64, direction: ParameterDirection.Input);
            vParams.Add("@Search", search, dbType: DbType.String, direction: ParameterDirection.Input);
            var res = _db.QueryMultiple("[GetAll_RebatePromotion]", vParams, commandType: CommandType.StoredProcedure);

            response.AddRange(res.Read<RebatePromotion>());
            long no = res.Read<long>().SingleOrDefault();
            var tuple = new Tuple<List<RebatePromotion>, long>(response, no);
            return tuple;
        }
        public Tuple<List<VoucherPromotion>, long> voucherPromotions(PageParam pageParam, string search, int Id)
        {
            List<VoucherPromotion> response = new();

            var vParams = new DynamicParameters();
            vParams.Add("@Offset", pageParam.Offset, dbType: DbType.Int64, direction: ParameterDirection.Input);
            vParams.Add("@Limit", pageParam.Limit, dbType: DbType.Int64, direction: ParameterDirection.Input);
            vParams.Add("@Id", Id, dbType: DbType.Int64, direction: ParameterDirection.Input);
            vParams.Add("@Search", search, dbType: DbType.String, direction: ParameterDirection.Input);
            var res = _db.QueryMultiple("[GetAll_VoucherPromotion]", vParams, commandType: CommandType.StoredProcedure);

            response.AddRange(res.Read<VoucherPromotion>());
            long no = res.Read<long>().SingleOrDefault();
            var tuple = new Tuple<List<VoucherPromotion>, long>(response, no);
            return tuple;
        }
        public Tuple<List<FreeAssetPromotion>, long> freeAssetPromotions(PageParam pageParam, string search, int Id)
        {
            List<FreeAssetPromotion> response = new();

            var vParams = new DynamicParameters();
            vParams.Add("@Offset", pageParam.Offset, dbType: DbType.Int64, direction: ParameterDirection.Input);
            vParams.Add("@Limit", pageParam.Limit, dbType: DbType.Int64, direction: ParameterDirection.Input);
            vParams.Add("@Id", Id, dbType: DbType.Int64, direction: ParameterDirection.Input);
            vParams.Add("@Search", search, dbType: DbType.String, direction: ParameterDirection.Input);
            var res = _db.QueryMultiple("[GetAll_FreeAssetPromotion]", vParams, commandType: CommandType.StoredProcedure);

            response.AddRange(res.Read<FreeAssetPromotion>());
            long no = res.Read<long>().SingleOrDefault();
            var tuple = new Tuple<List<FreeAssetPromotion>, long>(response, no);
            return tuple;
        }
        public Tuple<List<ReferPromotion>, long> refPromotions(PageParam pageParam, string search, int Id)
        {
            List<ReferPromotion> response = new();

            var vParams = new DynamicParameters();
            vParams.Add("@Offset", pageParam.Offset, dbType: DbType.Int64, direction: ParameterDirection.Input);
            vParams.Add("@Limit", pageParam.Limit, dbType: DbType.Int64, direction: ParameterDirection.Input);
            vParams.Add("@Id", Id, dbType: DbType.Int64, direction: ParameterDirection.Input);
            vParams.Add("@Search", search, dbType: DbType.String, direction: ParameterDirection.Input);
            var res = _db.QueryMultiple("[GetAll_ReferPromotion]", vParams, commandType: CommandType.StoredProcedure);

            response.AddRange(res.Read<ReferPromotion>());
            long no = res.Read<long>().SingleOrDefault();
            var tuple = new Tuple<List<ReferPromotion>, long>(response, no);
            return tuple;
        }
        public async Task<List<RuleConditionViewModel_Dt>> GetRuleConditions(int Id)
        {
            List<RuleConditionViewModel_Dt> ruleConditions = new List<RuleConditionViewModel_Dt>();
            var vParams = new DynamicParameters();
            vParams.Add("@Id", Id);

            var response=_db.QueryMultiple("[GetRuleConditions]", vParams,commandType: CommandType.StoredProcedure);
            ruleConditions.AddRange(response.Read<RuleConditionViewModel_Dt>());
            return ruleConditions;
        }
        public async Task<ResponseViewModel> UpsertPromotion(Promotion promotion)
        {
            var vParams_UR = new DynamicParameters();
            vParams_UR.Add("@PromotionId", promotion.Id);
            vParams_UR.Add("@Name", promotion.Name);
            vParams_UR.Add("@Priority", promotion.Priority);
            vParams_UR.Add("@CreditPromotionId", promotion.creditPromotion.CreditPromoId);
            vParams_UR.Add("@AcceptedDateTime", promotion.creditPromotion.AcceptedDateTime);
            vParams_UR.Add("@PostedDateTime", promotion.creditPromotion.PostedDateTime);
            vParams_UR.Add("@PromoExpDate", promotion.creditPromotion.PromoExpDate);
            vParams_UR.Add("@PromoName", promotion.creditPromotion.PromoName);
            vParams_UR.Add("@PromoAmount", promotion.creditPromotion.PromoAmount);
            vParams_UR.Add("@PerOfDeposit", promotion.creditPromotion.PerOfDeposit);
            vParams_UR.Add("@Countries", promotion.creditPromotion.Countries);
            vParams_UR.Add("@EnableTrigger", promotion.creditPromotion.EnableTrigger);

            var res_UR = _db.QueryMultiple("[Upsert_Promotion]", vParams_UR, commandType: CommandType.StoredProcedure);
            var response = res_UR.Read<ResponseViewModel>().SingleOrDefault();
            return response;
        }
        public async Task<ResponseViewModel> UpsertPromotionRuleWithRuleCondition(Promotion promotion)
        {
            var vParams_UR = new DynamicParameters();
            vParams_UR.Add("@PromotionId", promotion.Id);
            vParams_UR.Add("@RuleName", promotion.Name);
            vParams_UR.Add("@Priority", promotion.Priority);

            var res_UR = _db.QueryMultiple("[Upsert_PromotionRuleWithRuleCondition]", vParams_UR, commandType: CommandType.StoredProcedure);
            var response = res_UR.Read<ResponseViewModel>().SingleOrDefault();
            return response;
        }
        public async Task<ResponseViewModel> UpsertRuleConditions(Promotion promotion)
        {
            try
            {
                ResponseViewModel responseViewModel = new ResponseViewModel();
                List<RuleConditionViewModel_Dt> ruleConditionViewModels = new List<RuleConditionViewModel_Dt>();
                ruleConditionViewModels = JsonConvert.DeserializeObject<List<RuleConditionViewModel_Dt>>(promotion.ObjRuleConditions);
                foreach (var item in ruleConditionViewModels)
                {
                    var vParams_UR = new DynamicParameters();
                    vParams_UR.Add("@RuleConditionId", promotion.RuleConditionId);
                    vParams_UR.Add("@FieldName", item.FieldName.Split('|')[0]);
                    vParams_UR.Add("@Operation", item.Opration);
                    vParams_UR.Add("@Operationvalue", item.OprationValue);
                    vParams_UR.Add("@datatype", item.FieldName.Split('|')[1]);

                    var res_UR = _db.QueryMultiple("[Upsert_RuleConditions]", vParams_UR, commandType: CommandType.StoredProcedure);
                    responseViewModel = res_UR.Read<ResponseViewModel>().SingleOrDefault();
                    
                }
                return await Task.FromResult(responseViewModel);
            }
            catch (Exception ex)
            {
                return null;
            }
           
        }
        public async Task<ResponseViewModel> UpsertReferFriendPromotion(ReferPromotion promotion)
        {
            var vParams_UR = new DynamicParameters();
            vParams_UR.Add("@PromotionId", promotion.Id);
            vParams_UR.Add("@RefCode", promotion.PromoRefCode);
            vParams_UR.Add("@ReferFriendPromoId", promotion.ReferFriendPromoId);
            vParams_UR.Add("@StartDate", promotion.StartDate);
            vParams_UR.Add("@EndDate", promotion.EndDate);
            vParams_UR.Add("@SenderRefFee", promotion.SenderRefFee);
            vParams_UR.Add("@RecipientFee", promotion.RecipientFee);
            vParams_UR.Add("@EnableTrigger", promotion.EnableTrigger);
            vParams_UR.Add("@Countries", promotion.Countries);


            var res_UR = _db.QueryMultiple("[Upsert_ReferFriendPromotion]", vParams_UR, commandType: CommandType.StoredProcedure);
            var response = res_UR.Read<ResponseViewModel>().SingleOrDefault();
            return response;
        }
        public async Task<ResponseViewModel> UpsertFreeAssetPromotion(FreeAssetPromotion promotion)
        {
            try
            {
                var vParams_UR = new DynamicParameters();
                vParams_UR.Add("@PromotionId", promotion.Id);
                vParams_UR.Add("@RefCode", promotion.PromoRefCode);
                vParams_UR.Add("@FreeAssetPromoId", promotion.FreeAssetsPromoId);
                vParams_UR.Add("@StartDate", promotion.StartDate);
                vParams_UR.Add("@EndDate", promotion.EndDate);
                vParams_UR.Add("@MaxAmount", promotion.MaxAmount);
                vParams_UR.Add("@MinDeposit", promotion.MinDeposit);
                vParams_UR.Add("@Allowedshare", promotion.AllowedShare);
                vParams_UR.Add("@EnableTrigger", promotion.EnableTrigger);
                vParams_UR.Add("@Countries", promotion.Countries);


                var res_UR = _db.QueryMultiple("[Upsert_FreeAssetPromotion]", vParams_UR, commandType: CommandType.StoredProcedure);
                var response = res_UR.Read<ResponseViewModel>().SingleOrDefault();
                return response;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<ResponseViewModel> UpsertVoucherPromotion(VoucherPromotion promotion)
        {
            try
            {
                var vParams_UR = new DynamicParameters();
                vParams_UR.Add("@PromotionId", promotion.Id);
                vParams_UR.Add("@RefCode", promotion.PromoRefCode);
                vParams_UR.Add("@VoucherPromoId", promotion.VoucherPromoId);
                vParams_UR.Add("@Name", promotion.Name);
                vParams_UR.Add("@StartDate", promotion.StartTime);
                vParams_UR.Add("@EndDate", promotion.EndTime);
                vParams_UR.Add("@MaxAmount", promotion.VoucherAmt);
                vParams_UR.Add("@MinDeposit", promotion.MinDeposit);
                vParams_UR.Add("@Allowedshare", promotion.MaxAllowedShare);
                vParams_UR.Add("@EnableTrigger", promotion.EnableTrigger);
                vParams_UR.Add("@Countries", promotion.Countries);


                var res_UR = _db.QueryMultiple("[Upsert_VoucherPromotion]", vParams_UR, commandType: CommandType.StoredProcedure);
                var response = res_UR.Read<ResponseViewModel>().SingleOrDefault();
                return response;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<ResponseViewModel> UpsertRebatePromotion(RebatePromotion promotion)
        {
            try
            {
                var vParams_UR = new DynamicParameters();
                vParams_UR.Add("@PromotionId", promotion.Id);
                vParams_UR.Add("@RefCode", promotion.PromoRefCode);
                vParams_UR.Add("@RebatePromoId", promotion.RebatePromoId);
                vParams_UR.Add("@StartDate", promotion.StartDate);
                vParams_UR.Add("@EndDate", promotion.EndDate);
                vParams_UR.Add("@RebateAmount", promotion.RebateAmt);
                vParams_UR.Add("@RebateVolume", promotion.RebateVol);
                vParams_UR.Add("@EnableTrigger", promotion.EnableTrigger);
                vParams_UR.Add("@Countries", promotion.Countries);


                var res_UR = _db.QueryMultiple("[Upsert_RebatePromotion]", vParams_UR, commandType: CommandType.StoredProcedure);
                var response = res_UR.Read<ResponseViewModel>().SingleOrDefault();
                return response;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public Tuple<List<Promotion>, long> GetAllPromotion(PageParam pageParam, string search)
        {
            List<Promotion> response = new();

            var vParams = new DynamicParameters();
            vParams.Add("@Offset", pageParam.Offset, dbType: DbType.Int64, direction: ParameterDirection.Input);
            vParams.Add("@Limit", pageParam.Limit, dbType: DbType.Int64, direction: ParameterDirection.Input);
            vParams.Add("@Search", search, dbType: DbType.String, direction: ParameterDirection.Input);
            var res = _db.QueryMultiple("[GetAll_Promotion]", vParams, commandType: CommandType.StoredProcedure);

            response.AddRange(res.Read<Promotion>());
            long no = res.Read<long>().SingleOrDefault();
            var tuple = new Tuple<List<Promotion>, long>(response, no);
            return tuple;
        }

        public ResponseViewModel DeleteCreditPromotion(int Id)
        {
            ResponseViewModel response = new ResponseViewModel();

            var vParams = new DynamicParameters();
            vParams.Add("@CreditPromoId", Id);
            var res = _db.QueryMultiple("[CreditPromo_Delete]", vParams, commandType: CommandType.StoredProcedure);

            response = res.Read<ResponseViewModel>().SingleOrDefault();

            return response;
        }
        public ResponseViewModel DeleteRebatePromotion(int Id)
        {
            ResponseViewModel response = new ResponseViewModel();

            var vParams = new DynamicParameters();
            vParams.Add("@RebatePromoId", Id);
            var res = _db.QueryMultiple("[RebatePromo_Delete]", vParams, commandType: CommandType.StoredProcedure);

            response = res.Read<ResponseViewModel>().SingleOrDefault();

            return response;
        }
        public ResponseViewModel DeleteReferFriendPromotion(int Id)
        {
            ResponseViewModel response = new ResponseViewModel();

            var vParams = new DynamicParameters();
            vParams.Add("@ReferFriendPromoId", Id);
            var res = _db.QueryMultiple("[ReferFriendPromo_Delete]", vParams, commandType: CommandType.StoredProcedure);

            response = res.Read<ResponseViewModel>().SingleOrDefault();

            return response;
        }
        public ResponseViewModel DeleteFreeAssetPromotion(int Id)
        {
            ResponseViewModel response = new ResponseViewModel();

            var vParams = new DynamicParameters();
            vParams.Add("@FreeAssetPromoId", Id);
            var res = _db.QueryMultiple("[FreeAssetPromo_Delete]", vParams, commandType: CommandType.StoredProcedure);

            response = res.Read<ResponseViewModel>().SingleOrDefault();

            return response;
        }
        public ResponseViewModel DeleteVoucherPromotion(int Id)
        {
            ResponseViewModel response = new ResponseViewModel();

            var vParams = new DynamicParameters();
            vParams.Add("@VoucherPromoId", Id);
            var res = _db.QueryMultiple("[VoucherPromo_Delete]", vParams, commandType: CommandType.StoredProcedure);

            response = res.Read<ResponseViewModel>().SingleOrDefault();

            return response;
        }
    }
}
