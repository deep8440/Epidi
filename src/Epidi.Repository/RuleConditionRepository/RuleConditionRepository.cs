using Dapper;
using Epidi.Models.DBConnection;
using Epidi.Models.Paging;
using Epidi.Models.ViewModel.Common;
using Epidi.Models.ViewModel.LeverageRules;
using Epidi.Models.ViewModel.RuleCondition;
using Epidi.Repository.ConnectionProvider;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Repository.RuleConditionRepository
{
    public class RuleConditionRepository : RepositoryBase, IRuleConditionRepository
    {
        public RuleConditionRepository(IOptions<ConnectionSettings> connectionOptions,
           IConnectionProvider connectionProvider) : base(connectionOptions: connectionOptions,
               connectionProvider: connectionProvider)
        {
        }
        public List<ResponseViewModel> Delete(int Id)
        {
            List<ResponseViewModel> response = new List<ResponseViewModel>();

            var vParams = new DynamicParameters();
            vParams.Add("@Id", Id);
            var res = _db.QueryMultiple("[RuleConditions_Delete]", vParams, commandType: CommandType.StoredProcedure);

            response = res.Read<ResponseViewModel>().ToList();

            return response;
        }

        public Tuple<List<RuleConditionViewModel>, long> GetAll(PageParam pageParam, string search)
        {
            List<RuleConditionViewModel> response = new();

            var vParams = new DynamicParameters();
            vParams.Add("@Offset", pageParam.Offset, dbType: DbType.Int64, direction: ParameterDirection.Input);
            vParams.Add("@Limit", pageParam.Limit, dbType: DbType.Int64, direction: ParameterDirection.Input);
            vParams.Add("@Search", search, dbType: DbType.String, direction: ParameterDirection.Input);
            var res = _db.QueryMultiple("[RuleConditions_GetAll]", vParams, commandType: CommandType.StoredProcedure);

            response.AddRange(res.Read<RuleConditionViewModel>());
            long no = res.Read<long>().SingleOrDefault();
            var tuple = new Tuple<List<RuleConditionViewModel>, long>(response, no);
            return tuple;
        }

        public async Task<RuleConditionViewModel> GetById(int Id)
        {
            RuleConditionViewModel response = new RuleConditionViewModel();
            response.rulesDtlViewModel = new List<LeverageRulesDtlViewModel>();

            var vParams = new DynamicParameters();
            vParams.Add("@Id", Id);
            var res = await _db.QueryMultipleAsync("[RuleConditions_GetById]", vParams, commandType: CommandType.StoredProcedure);

            response = res.Read<RuleConditionViewModel>().FirstOrDefault();

            List<LeverageRulesDtlViewModel> responseDtl = new();
            responseDtl = await GetRuleConditionsDTL_ByuleConditionsId(Id);
            response.rulesDtlViewModel = responseDtl;

            return response;
        }
        public async Task<RuleConditionViewModel> GetByRuleId(int Id)
        {
            RuleConditionViewModel response = new();

            var vParams = new DynamicParameters();
            vParams.Add("@Id", Id);
            var res = await _db.QueryMultipleAsync("[RuleConditions_GetByRuleId]", vParams, commandType: CommandType.StoredProcedure);

            response = res.Read<RuleConditionViewModel>().FirstOrDefault();
            if (response!=null)
            {
                List<LeverageRulesDtlViewModel> responseDtl = new();
                responseDtl = await GetRuleConditionsDTL_ByuleConditionsId(response.Id);
                response.rulesDtlViewModel = responseDtl;
            }
            return response;
        }

        public async Task<RuleConditionViewModel> GetRuleConditionByTradeRuleId(int Id)
        {
            RuleConditionViewModel response = new();

            var vParams = new DynamicParameters();
            vParams.Add("@Id", Id);
            var res = await _db.QueryMultipleAsync("[RuleConditions_GetByTradeRuleId]", vParams, commandType: CommandType.StoredProcedure);

            response = res.Read<RuleConditionViewModel>().FirstOrDefault();
            if (response != null)
            {
                List<LeverageRulesDtlViewModel> responseDtl = new();
                responseDtl = await GetRuleConditionsDTL_ByuleConditionsId(response.Id);
                response.rulesDtlViewModel = responseDtl;
            }
            return response;
        }

        public async Task<RuleConditionViewModel> GetRuleConditionBySwapRuleId(int Id)
        {
            RuleConditionViewModel response = new();

            var vParams = new DynamicParameters();
            vParams.Add("@Id", Id);
            var res = await _db.QueryMultipleAsync("[RuleConditions_GetBySwapRuleId]", vParams, commandType: CommandType.StoredProcedure);

            response = res.Read<RuleConditionViewModel>().FirstOrDefault();
            if (response != null)
            {
                List<LeverageRulesDtlViewModel> responseDtl = new();
                responseDtl = await GetRuleConditionsDTL_ByuleConditionsId(response.Id);
                response.rulesDtlViewModel = responseDtl;
            }
            return response;
        }
        public async Task<RuleConditionViewModel> GetRuleConditionBySpecificationRuleId(int Id)
        {
            RuleConditionViewModel response = new();

            var vParams = new DynamicParameters();
            vParams.Add("@Id", Id);
            var res = await _db.QueryMultipleAsync("[RuleConditions_GetBySpecificationRuleId]", vParams, commandType: CommandType.StoredProcedure);

            response = res.Read<RuleConditionViewModel>().FirstOrDefault();
            if (response != null)
            {
                List<LeverageRulesDtlViewModel> responseDtl = new();
                responseDtl = await GetRuleConditionsDTL_ByuleConditionsId(response.Id);
                response.rulesDtlViewModel = responseDtl;
            }
            return response;
        }
        public async Task<RuleConditionViewModel> GetRuleConditionByCommissionRuleId(int Id)
        {
            RuleConditionViewModel response = new();

            var vParams = new DynamicParameters();
            vParams.Add("@Id", Id);
            var res = await _db.QueryMultipleAsync("[RuleConditions_GetByCommissionRuleId]", vParams, commandType: CommandType.StoredProcedure);

            response = res.Read<RuleConditionViewModel>().FirstOrDefault();
            if (response != null)
            {
                List<LeverageRulesDtlViewModel> responseDtl = new();
                responseDtl = await GetRuleConditionsDTL_ByuleConditionsId(response.Id);
                response.rulesDtlViewModel = responseDtl;
            }
            return response;
        }
        public async Task<RuleConditionViewModel> GetByMarginRuleId(int Id)
        {
            RuleConditionViewModel response = new();

            var vParams = new DynamicParameters();
            vParams.Add("@MarginRuleId", Id);
            var res = await _db.QueryMultipleAsync("[RuleConditions_GetByMarginRuleId]", vParams, commandType: CommandType.StoredProcedure);

            response = res.Read<RuleConditionViewModel>().FirstOrDefault();
            if (response != null)
            {
                List<LeverageRulesDtlViewModel> responseDtl = new();
                responseDtl = await GetRuleConditionsDTL_ByuleConditionsId(response.Id);
                response.rulesDtlViewModel = responseDtl;
            }
            return response;
        }

        public async Task<RuleConditionViewModel> Upsert(RuleConditionViewModel aRuleCondition, bool Skip = false)
        {
            try
            {
                RuleConditionViewModel response = new();
                string _sqlQuery = CreateSqlQuery(aRuleCondition.rulesDtlViewModel);

                var vParams = new DynamicParameters();
                vParams.Add("@Id", aRuleCondition.Id);
                vParams.Add("@RuleId", aRuleCondition.RuleId);
                vParams.Add("@SwapRuleId", aRuleCondition.SwapRuleId);
                vParams.Add("@CommissionRuleId", aRuleCondition.CommissionRuleId);
                vParams.Add("@SpecificationRuleId", aRuleCondition.SpecificationRuleId);
                vParams.Add("@TradeRuleId", aRuleCondition.TradeRuleId);
                vParams.Add("@MarginRuleId", aRuleCondition.MarginRuleId);
                vParams.Add("@FieldName", aRuleCondition.FieldName);
                vParams.Add("@MatchingValues", aRuleCondition.MatchingValues);
                vParams.Add("@SqlQuery", _sqlQuery);
                vParams.Add("@Opration", aRuleCondition.Opration);
                vParams.Add("@CreatedBy", aRuleCondition.CreatedBy);
                vParams.Add("@UpdatedBy", aRuleCondition.UpdatedBy);
                vParams.Add("@LeverageRuleId", aRuleCondition.LeverageRuleId);
                vParams.Add("@PromotionRulesId", aRuleCondition.PromotionRuleId);
                var res = await _db.QueryMultipleAsync("[RuleConditions_Upsert]", vParams, commandType: CommandType.StoredProcedure);

                response = res.Read<RuleConditionViewModel>().FirstOrDefault();
                if (!Skip)
                {
                    if (aRuleCondition.Id > 0)
                    {
                        await DeleteConditionRulesDtl(aRuleCondition.Id);
                    }
                    UpsertConditionRulesDtl(aRuleCondition.rulesDtlViewModel, response.Id==null?0:response.Id);
                }
                return response;
            }
            catch (Exception EX)
            {

                throw;
            }
        }
        private List<ResponseViewModel> UpsertConditionRulesDtl(List<LeverageRulesDtlViewModel> model, int RuleId)
        {
            try
            {
                List<ResponseViewModel> response = new List<ResponseViewModel>();
                foreach (var item in model)
                {
                    string[] FiledNameArr = item.FieldName.Split("|");
                    string FieldName = FiledNameArr[0];
                    string Datatype = FiledNameArr[1];
                    string ForeignKey = FiledNameArr[2];

                    var vParams = new DynamicParameters();
                    vParams.Add("@Id", 0);
                    vParams.Add("@RuleConditionsId", RuleId);
                    vParams.Add("@FieldName", FieldName);
                    vParams.Add("@Opration", item.Opration);
                    vParams.Add("@OprationValue", item.OprationValue);
                    vParams.Add("@Datatype", Datatype);
                    vParams.Add("@ForeignKey", ForeignKey);
                    var res = _db.QueryMultiple("[RuleConditionsDtl_Upsert]", vParams, commandType: CommandType.StoredProcedure);
                    response = res.Read<ResponseViewModel>().ToList();
                }
                return response;
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public async Task<bool> DeleteConditionRulesDtl(int RuleId)
        {
            var vParams = new DynamicParameters();
            vParams.Add("@RuleConditionsId", RuleId);
            var res = _db.QueryMultiple("[RuleConditionsDTL_DeleteByuleConditionsId]", vParams, commandType: CommandType.StoredProcedure);
            res.Read<ResponseViewModel>().ToList();
            return true;
        }
        public async Task<List<LeverageRulesDtlViewModel>> GetRuleConditionsDTL_ByuleConditionsId(int Id)
        {
            List<LeverageRulesDtlViewModel> response = new();

            var vParams = new DynamicParameters();
            vParams.Add("@RuleConditionsId", Id);
            var res = await _db.QueryMultipleAsync("[RuleConditionsDTL_GetByuleConditionsId]", vParams, commandType: CommandType.StoredProcedure);

            response = res.Read<LeverageRulesDtlViewModel>().ToList();

            return response;
        }
        public string CreateSqlQuery(List<LeverageRulesDtlViewModel> model)
        {
            string strSql = "";
            foreach (var item in model)
            {
                string[] FiledNameArr = item.FieldName.Split("|");
                string FieldName = FiledNameArr[0];
                string Datatype = FiledNameArr[1];
                string ForeignKey = FiledNameArr[2];

                string Opration = item.Opration;
                string OprationValue = item.OprationValue;

                strSql += " AND " + FieldName + "";
                //if (FieldName == "CountryId")
                //{
                //    strSql += " = AND (SELECT CountryId FROM Country WHERE Upper(CountryName) = '" + OprationValue.ToUpper() + "')";
                //}
                if (Opration.ToUpper() == "CONTAIN" && Datatype.ToUpper() == "STRING")
                {
                    strSql += " LIKE '%" + OprationValue + "%'";
                }
                else if (Opration.ToUpper() == "NOTCONTAIN" && Datatype.ToUpper() == "STRING")
                {
                    strSql += " NOT LIKE '%" + OprationValue + "%'";
                }
                else if (Opration.ToUpper() == "EQUAL" && Datatype.ToUpper() == "STRING")
                {
                    strSql += " = '" + OprationValue + "'";
                }
                else if (Opration.ToUpper() == "EQUAL" && Datatype.ToUpper() == "NUMBER")
                {
                    strSql += " = " + OprationValue + "";
                }
                else if (Opration.ToUpper() == "LESSTHAN" && Datatype.ToUpper() == "NUMBER")
                {
                    strSql += " < " + OprationValue + "";
                }
                else if (Opration.ToUpper() == "LESSTHANEQUAL" && Datatype.ToUpper() == "NUMBER")
                {
                    strSql += " <= " + OprationValue + "";
                }
                else if (Opration.ToUpper() == "GREATERTHAN" && Datatype.ToUpper() == "NUMBER")
                {
                    strSql += " > " + OprationValue + "";
                }
                else if (Opration.ToUpper() == "GREATERTHANEQUAL" && Datatype.ToUpper() == "NUMBER")
                {
                    strSql += " >= " + OprationValue + "";
                }
            }
            return strSql;
        }
        public async Task<bool> DeleteConditionRulesDtlById(int Id, int RuleConditionId)
        {
            var vParams = new DynamicParameters();
            vParams.Add("@Id", Id);
            var res = _db.QueryMultiple("[RuleConditionsDTL_DeleteById]", vParams, commandType: CommandType.StoredProcedure);
            res.Read<ResponseViewModel>().ToList();

            var res1 = await GetById(RuleConditionId);
            RuleConditionViewModel model = res1;
            List<LeverageRulesDtlViewModel> responseDtl = new();
            responseDtl = await GetRuleConditionsDTL_ByuleConditionsId(Id);
            model.rulesDtlViewModel = responseDtl;


            await Upsert(model, true);

            return true;
        }
        public async Task<bool> DeleteConditionRulesDtlByTraedRuleId(int Id, int RuleConditionId)
        {
            var vParams = new DynamicParameters();
            vParams.Add("@Id", Id);
            var res = _db.QueryMultiple("[RuleConditionsDTL_DeleteById]", vParams, commandType: CommandType.StoredProcedure);
            res.Read<ResponseViewModel>().ToList();

            return true;
        }
        public async Task<bool> DeleteSwapRuleConditionDtlById(int RuleConditionId, int RuleConditionDtlId)
        {
            var vParams = new DynamicParameters();
            vParams.Add("@RuleConditionId", RuleConditionId);
            vParams.Add("@RuleConditionDtlId", RuleConditionDtlId);
            var res = _db.QueryMultiple("[SwapRuleConditionsDTL_DeleteById]", vParams, commandType: CommandType.StoredProcedure);
            
            return true;
        }
        public async Task<bool> DeleteMarginRuleDtlById(int RuleConditionId, int RuleConditionDtlId)
        {
            var vParams = new DynamicParameters();
            vParams.Add("@RuleConditionId", RuleConditionId);
            vParams.Add("@RuleConditionDtlId", RuleConditionDtlId);
            var res = _db.QueryMultiple("[MarginRuleConditionsDTL_DeleteById]", vParams, commandType: CommandType.StoredProcedure);

            return true;
        }
        public async Task<int> DeleteQuotemarkUpRuleInstrument(int Id, int RuleId, int tLP_ID)
        {
            var vParams = new DynamicParameters();
            vParams.Add("@Id", Id);
            vParams.Add("@RuleId", RuleId);
            vParams.Add("@tLP_ID", tLP_ID);
            var res = await _db.QueryMultipleAsync("[QupteMarkUpRuleInstrument_DeleteById]", vParams, commandType: CommandType.StoredProcedure);

            return 1;
        }
    }

}
