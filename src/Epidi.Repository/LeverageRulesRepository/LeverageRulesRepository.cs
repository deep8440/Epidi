using Dapper;
using Epidi.Models.DBConnection;
using Epidi.Models.Model;
using Epidi.Models.Paging;
using Epidi.Models.ViewModel.Common;
using Epidi.Models.ViewModel.Instrument;
using Epidi.Models.ViewModel.LeverageRule;
using Epidi.Models.ViewModel.LeverageRules;
using Epidi.Models.ViewModel.Margin;
using Epidi.Models.ViewModel.RuleCondition;
using Epidi.Models.ViewModel.Users;
using Epidi.Repository.ConnectionProvider;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Repository.LeverageRulesRepository
{
    public class LeverageRulesRepository : RepositoryBase, ILeverageRulesRepository
    {
        public LeverageRulesRepository(IOptions<ConnectionSettings> connectionOptions,
            IConnectionProvider connectionProvider) : base(connectionOptions: connectionOptions,
                connectionProvider: connectionProvider)
        {
        }
        public Tuple<List<LeverageRulesViewModel>, long> GetAll(PageParam pageParam, string search)
        {
            List<LeverageRulesViewModel> response = new();

            var vParams = new DynamicParameters();
            vParams.Add("@Offset", pageParam.Offset, dbType: DbType.Int64, direction: ParameterDirection.Input);
            vParams.Add("@Limit", pageParam.Limit, dbType: DbType.Int64, direction: ParameterDirection.Input);
            vParams.Add("@Search", search, dbType: DbType.String, direction: ParameterDirection.Input);
            var res = _db.QueryMultiple("[LeverageRules_GetAll]", vParams, commandType: CommandType.StoredProcedure);

            response.AddRange(res.Read<LeverageRulesViewModel>());
            long no = res.Read<long>().SingleOrDefault();
            var tuple = new Tuple<List<LeverageRulesViewModel>, long>(response, no);
            return tuple;
        }
        public async Task<LeverageRulesViewModel> GetById(int Id)
        {
            LeverageRulesViewModel response = new();

            var vParams = new DynamicParameters();
            vParams.Add("@Id", Id);
            var res = await _db.QueryMultipleAsync("[LeverageRules_GetById]", vParams, commandType: CommandType.StoredProcedure);

            response = res.Read<LeverageRulesViewModel>().FirstOrDefault();

            List<LeverageRulesDtlViewModel> responseDtl = new();
            responseDtl = await GetLeverageRulesDTL_ByLeverageRuleId(Id);
            response.leverageRulesDtlViewModels = responseDtl;

            return response;
        }
        public Tuple<List<LeverageRuleImport>, long> GetLeverageRuleImports(PageParam pageParam, int id, string search, int name)
        {
            try 
            {
                List<LeverageRuleImport> response = new();
                var vParams = new DynamicParameters();

                vParams.Add("@LevarageId", id);
                vParams.Add("@name", name);
                vParams.Add("@Offset", pageParam.Offset, dbType: DbType.Int64, direction: ParameterDirection.Input);
                vParams.Add("@Limit", pageParam.Limit, dbType: DbType.Int64, direction: ParameterDirection.Input);
                vParams.Add("@Search", search, dbType: DbType.String, direction: ParameterDirection.Input);
                var res = _db.QueryMultiple("[Get_LeverageRuleImports]", vParams, commandType: CommandType.StoredProcedure, commandTimeout: 1800);
                
                response.AddRange(res.Read<LeverageRuleImport>());
                long no = res.Read<long>().SingleOrDefault();
                var tuple = new Tuple<List<LeverageRuleImport>, long>(response, no);
                return tuple;
            }
            catch (Exception ex) 
            {
                throw ex;
            }

            //            var res = await _db.QueryAsync<LeverageRuleImport>(@"
            //if(@LevarageId != 0 AND @name != 0)
            //BEGIN
            //select LR.Id as LeverageRuleId,LR.Name as LeverageName,LR.[Priority] as Priority1, LR.Comment AS LComment, IM.id as MasterInstrumentId,IM.Name as MasterInstrumentName,IM.SymbolGroupId,LBM.LeverageAmount,LB.BandFrom,LB.BandTo,
            //                                                                 L.LeverageId,L.MasterInstrumentId,L.[Day],L.[Priority] as Priority2,CAST(L.TimeFrom as varchar) TimeFrom,CAST(L.TimeTo as varchar) TimeTo,
            //                                                                 L.IsNewPosition,L.LegalEntityId,LBM.LeverageAmount,SG.[Group] as SymbolGroupName,LE.Name as [LegalEntity]
            //                                                                 from LeverageRule LR  
            //                                                                 left join Leverage L on LR.Id=L.LeverageRuleId
            //                                                                 left join InstrumentMaster IM ON L.MasterInstrumentId=IM.id
            //                                                                 left join SymbolGroup SG ON IM.SymbolGroupId = SG.Id
            //                                                                 left join LeverageBandMapping LBM ON LBM.LeverageId=L.LeverageId
            //                                                                 left join LeverageBand LB ON LB.LeverageBandId=LBM.LeverageBandId
            //                                                                 left join LegalEntity LE ON L.legalentityid=LE.Id  
            //                                                                 Where LR.Id=@LevarageId AND IM.id=@name
            //END
            // Else If(@LevarageId != 0)
            //BEGIN
            //select LR.Id as LeverageRuleId,LR.Name as LeverageName,LR.[Priority] as Priority1, LR.Comment AS LComment, IM.id as MasterInstrumentId,IM.Name as MasterInstrumentName,IM.SymbolGroupId,LBM.LeverageAmount,LB.BandFrom,LB.BandTo,
            //                                                                 L.LeverageId,L.MasterInstrumentId,L.[Day],L.[Priority] as Priority2,CAST(L.TimeFrom as varchar) TimeFrom,CAST(L.TimeTo as varchar) TimeTo,
            //                                                                 L.IsNewPosition,L.LegalEntityId,LBM.LeverageAmount,SG.[Group] as SymbolGroupName,LE.Name as [LegalEntity]
            //                                                                 from LeverageRule LR  
            //                                                                 left join Leverage L on LR.Id=L.LeverageRuleId
            //                                                                 left join InstrumentMaster IM ON L.MasterInstrumentId=IM.id
            //                                                                 left join SymbolGroup SG ON IM.SymbolGroupId = SG.Id
            //                                                                 left join LeverageBandMapping LBM ON LBM.LeverageId=L.LeverageId
            //                                                                 left join LeverageBand LB ON LB.LeverageBandId=LBM.LeverageBandId
            //                                                                 left join LegalEntity LE ON L.legalentityid=LE.Id  
            //                                                                 Where LR.Id=@LevarageId
            //END
            //", new
            //            {
            //                @LevarageId = id,
            //                @search = search, @name = name
            //            });
            //response = res.ToList();
            //return response;
        }

        public async Task<LeverageRuleImport> GetLeverageRuleData(int id)
        {
            try
            {
                LeverageRuleImport response = new();
                var vParams = new DynamicParameters();
                vParams.Add("@LevarageId", id);
                var res = await _db.QueryMultipleAsync("[Get_LeverageRuleData]", vParams, commandType: CommandType.StoredProcedure);

                response = res.Read<LeverageRuleImport>().FirstOrDefault();
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public async Task<List<RuleConditionViewModel_Dt>> GetRuleConditionsByRuleId(int id)
        {
            var vParams = new DynamicParameters();
            vParams.Add("@LId", id);            
            var res = _db.QueryMultiple("[GetRulesByLeverageId]", vParams, commandType: CommandType.StoredProcedure);

            List<RuleConditionViewModel_Dt> response = new List<RuleConditionViewModel_Dt>();
            response.AddRange(res.Read<RuleConditionViewModel_Dt>().ToList());
            return response;
        }
        
        public List<ResponseViewModel> Upsert(LeverageRulesViewModel model)
        {
            string _sqlQuery = CreateSqlQuery(model.leverageRulesDtlViewModels);

            var vParams = new DynamicParameters();
            vParams.Add("@Id", model.Id);
            vParams.Add("@RuleName", model.RuleName);
            vParams.Add("@LevregeValue", model.LevregeValue);
            vParams.Add("@Priority", model.Priority);
            vParams.Add("@IsActive", model.IsActive);
            vParams.Add("@CreatedBy", model.CreatedBy);
            vParams.Add("@UpdatedBy", model.UpdatedAt);
            vParams.Add("@SqlQuery", _sqlQuery);
            var res = _db.QueryMultiple("[LeverageRules_Upsert]", vParams, commandType: CommandType.StoredProcedure);

            List<ResponseViewModel> response =  res.Read<ResponseViewModel>().ToList();
            UpsertLeverageRulesDtl(model.leverageRulesDtlViewModels, response[0].Id);
            SaveUserVsRuleDate(response[0].Id);
            return response;
        }

        public ResponseViewModel UpdateLeverage(LeverageRuleImport leverageRule)
        {

            var vParams = new DynamicParameters();
            vParams.Add("@LeverageId", leverageRule.LeverageId);
            vParams.Add("@LeverageRuleId", leverageRule.LeverageRuleId);
            vParams.Add("@MasterInstrumentId", leverageRule.MasterInstrumentId);
            vParams.Add("@day", leverageRule.Day);
            vParams.Add("@priority", leverageRule.Priority);
            vParams.Add("@Timefrom", leverageRule.TimeFrom);
            vParams.Add("@TimeTo", leverageRule.TimeTo);
            vParams.Add("@Isnewposition", leverageRule.IsNewPosition == "Yes" ? true : false);
            vParams.Add("@legalEntityId", leverageRule.LegalEntityId);
            var res = _db.QueryMultiple("[UpdateLeverage]", vParams, commandType: CommandType.StoredProcedure);



            ResponseViewModel response = res.Read<ResponseViewModel>().SingleOrDefault();
            return response;
        }
        public ResponseViewModel UpdateBandMapping(LeverageRuleImport leverageRule)
        {
            var vParams = new DynamicParameters();
            vParams.Add("@BandFrom", leverageRule.BandFrom);
            vParams.Add("@BandTo", leverageRule.BandTo);
            vParams.Add("@LevAmount", leverageRule.LeverageAmount);
            vParams.Add("@LeverageId", leverageRule.LeverageId);
          
            var res = _db.QueryMultiple("[UpdateLeverageBandMapping]", vParams, commandType: CommandType.StoredProcedure);



            ResponseViewModel response = res.Read<ResponseViewModel>().SingleOrDefault();
            return response;
        }
        public ResponseViewModel InsertLeverageRule(LeverageRuleImport leverageRuleImport)
        {
            var vParams = new DynamicParameters();
            vParams.Add("@Name", leverageRuleImport.LeverageName);
            vParams.Add("@Priority", leverageRuleImport.Priority1);
            vParams.Add("@Comment", leverageRuleImport.LComment);
            vParams.Add("@Id", leverageRuleImport.LeverageRuleId);

            var res = _db.QueryMultiple("[LeverageRule_Upsert]", vParams, commandType: CommandType.StoredProcedure);
            ResponseViewModel response = res.Read<ResponseViewModel>().FirstOrDefault();
            return response;
        }
        private List<ResponseViewModel> UpsertLeverageRulesDtl(List<LeverageRulesDtlViewModel> model, int LeverageRuleId)
        {
            List<ResponseViewModel> response = new List<ResponseViewModel>();
            foreach (var item in model)
            {
                string[] FiledNameArr = item.FieldName.Split("|");
                string FieldName = FiledNameArr[0];
                string Datatype = FiledNameArr[1];
                string ForeignKey = FiledNameArr[2];

                var vParams = new DynamicParameters();
                vParams.Add("@Id", item.Id);
                vParams.Add("@LeverageRuleId", LeverageRuleId);
                vParams.Add("@FieldName", FieldName);
                vParams.Add("@Opration", item.Opration);
                vParams.Add("@OprationValue", item.OprationValue);
                vParams.Add("@Datatype", Datatype);
                vParams.Add("@ForeignKey", ForeignKey);
                vParams.Add("@TableName", item.TableName);
                var res = _db.QueryMultiple("[LeverageRulesDtl_Upsert]", vParams, commandType: CommandType.StoredProcedure);
                response = res.Read<ResponseViewModel>().ToList();
            }
            return response;
        }

        public List<LeverageRuleImport> GetImportLeverageRules(int LeverageRuleId)
        {
            List<LeverageRuleImport> levrageViewModels = new List<LeverageRuleImport>();
            var param = new DynamicParameters();
            param.Add("@LeverageRuleId", LeverageRuleId, dbType: DbType.Int64, direction: ParameterDirection.Input);

            var res = _db.QueryMultiple("[dbo].[GetAll_Leverage]", param, commandType: CommandType.StoredProcedure);

            levrageViewModels.AddRange(res.Read<LeverageRuleImport>());
            return levrageViewModels;
        }

        public List<ResponseViewModel> RemoveRule(int Id)
        {

            List<ResponseViewModel> response = new List<ResponseViewModel>();

            var param = new DynamicParameters();
            param.Add("@Id", Id, dbType: DbType.Int32, direction: ParameterDirection.Input);

            var res = _db.QueryMultiple("[dbo].[RemoveRule]", param, commandType: CommandType.StoredProcedure);


            response = res.Read<ResponseViewModel>().ToList();

            return response;
        }


        //public async Task<bool> RemoveRule(int id)
        //{
        //    try
        //    {

        //        bool response = new();

        //        var vParams = new DynamicParameters();
        //        vParams.Add("@Id", id);
        //        var res = await _db.QuerySingleAsync("[RemoveRule]", vParams, commandType: CommandType.StoredProcedure);

        //        response = res.Read<bool>().First();

        //        return response;

        //    }
        //    catch (Exception ex)
        //    {

        //        throw;
        //    }
        //}
        public async Task<LevarageRuleImportViewModel> Edit(int id)
        {
            try
            {
                LevarageRuleImportViewModel obj = new LevarageRuleImportViewModel();
                var Params = new DynamicParameters();
                Params.Add("@LeverageId", id);
                var result = await _db.QuerySingleAsync("[LeverageRule_Edit]", Params, commandType: CommandType.StoredProcedure);
                //var result = await _db.QueryAsync<LevarageRuleImportViewModel>(@"select LeverageId,MasterInstrumentId,[Day],CAST(TimeFrom as varchar) as TimeFrom,CAST(TimeTo as varchar) as TimeTo,IsNewPosition from Leverage where LeverageId=@LeverageId", new
                //{
                //    @LeverageId = id
                //}).ConfigureAwait(false);
                obj = result.FirstOrDefault();
                var res = await _db.QueryAsync<LevarageBandMapping>(@"select * from LeverageBandMapping where LeverageId=@LevarageId", new
                {
                    @LevarageId = id
                });
                obj.ObjLeverageBandMapping = res.ToList();
                var vParams = new DynamicParameters();
                vParams.Add("@Id", string.Join(',', res.Select(x => x.LeverageBandId)), dbType: DbType.String, direction: ParameterDirection.Input);
                var res1 = _db.QueryMultiple("[GetLeverageBands]", vParams, commandType: CommandType.StoredProcedure);
                obj.ObjLeverageBand = new List<Models.ViewModel.LeverageRules.LevarageBand>();
                obj.ObjLeverageBand.AddRange(res1.Read<Models.ViewModel.LeverageRules.LevarageBand>());


                return obj;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public ResponseViewModel ImportLevarageRule(LeverageRuleImport leverageRuleImport)
        {
            ResponseViewModel response = new ResponseViewModel();


            var vParams = new DynamicParameters();
            vParams.Add("@MasterInstrumentName", leverageRuleImport.MasterInstrumentName);
            vParams.Add("@LeverageRuleId", leverageRuleImport.LeverageRuleId);
            vParams.Add("@Day", leverageRuleImport.Day);
            vParams.Add("@TimeFrom", leverageRuleImport.TimeFrom);
            vParams.Add("@TimeTo", leverageRuleImport.TimeTo);
            vParams.Add("@IsNewPosition", leverageRuleImport.IsNewPosition);
            vParams.Add("@Priority", leverageRuleImport.Priority);
            vParams.Add("@LegalEntity", leverageRuleImport.LegalEntity);
            vParams.Add("@SymbolGroup", leverageRuleImport.SymbolGroupName);
            var res = _db.QueryMultiple("[LevarageRule_Import]", vParams, commandType: CommandType.StoredProcedure);
            response = res.Read<ResponseViewModel>().FirstOrDefault();

            return response;
        }

		public async Task<long> ImportLevarageRule(DataTable dataTable)
		{
			try
			{
				int ruleId;
				SqlConnection con = new SqlConnection(_connectionOptions.ConnectionString);
				await con.OpenAsync();

				SqlCommand sqlCommand = new SqlCommand("LevarageRule_Import", con);
				sqlCommand.CommandType = CommandType.StoredProcedure;
				sqlCommand.CommandTimeout = 1800;
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

		public ResponseViewModel ImportLevarageBand(List<Models.Model.LevarageBand> levarageBand)
        {
            ResponseViewModel response = new ResponseViewModel();

            foreach (var item in levarageBand)
            {
                var vParams = new DynamicParameters();
                vParams.Add("@BandFrom", item.BandFrom);
                vParams.Add("@BandTo", item.BandTo);
                vParams.Add("@LevarageAmt", item.LeverageAmount);
                vParams.Add("@LevarageId", item.LeverageId);
                var res = _db.QueryMultiple("[LevarageBand_Import]", vParams, commandType: CommandType.StoredProcedure);
                response = res.Read<ResponseViewModel>().FirstOrDefault();
            }

            return response;
        }

        private List<ResponseViewModel> SaveUserVsRuleDate(int LeverageRuleId)
        {
            List<UsersViewModel> response = new();

            var vParams = new DynamicParameters();
            vParams.Add("@Offset", 0, dbType: DbType.Int64, direction: ParameterDirection.Input);
            vParams.Add("@Limit", 0, dbType: DbType.Int64, direction: ParameterDirection.Input);
            vParams.Add("@LeverageId", LeverageRuleId);
            var res = _db.QueryMultiple("[GetUsers_ByLeverageRuleId]", vParams, commandType: CommandType.StoredProcedure);

            response.AddRange(res.Read<UsersViewModel>());
            res.Read<long>().SingleOrDefault();



            List<ResponseViewModel> response_UR = new List<ResponseViewModel>();
            foreach (var item in response)
            {
                var vParams_UR = new DynamicParameters();
                vParams_UR.Add("@Id", 0);
                vParams_UR.Add("@RuleId", LeverageRuleId);
                vParams_UR.Add("@UserId", item.Id);

                var res_UR = _db.QueryMultiple("[UsersVsRules_Upsert]", vParams_UR, commandType: CommandType.StoredProcedure);
                response_UR = res_UR.Read<ResponseViewModel>().ToList();
            }
            return response_UR;
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
        public async Task<List<LeverageRulesDtlViewModel>> GetLeverageRulesDTL_ByLeverageRuleId(int Id)
        {
            List<LeverageRulesDtlViewModel> response = new();

            var vParams = new DynamicParameters();
            vParams.Add("@LeverageRuleId", Id);
            var res = await _db.QueryMultipleAsync("[LeverageRulesDTL_GetByLeverageRuleId]", vParams, commandType: CommandType.StoredProcedure);

            response = res.Read<LeverageRulesDtlViewModel>().ToList();

            return response;
        }
        public Tuple<List<LeverageRuleImport>, long> GetAllLeverageRules(PageParam pageParam, string search)
        {
            List<LeverageRuleImport> levrageViewModels = new List<LeverageRuleImport>();
            var param = new DynamicParameters();
            param.Add("@Offset", pageParam.Offset, dbType: DbType.Int64, direction: ParameterDirection.Input);
            param.Add("@Limit", pageParam.Limit, dbType: DbType.Int64, direction: ParameterDirection.Input);
            param.Add("@Search", search, dbType: DbType.String, direction: ParameterDirection.Input);

            var res = _db.QueryMultiple("[dbo].[GetAll_LeverageRule]", param, commandType: CommandType.StoredProcedure);

            levrageViewModels.AddRange(res.Read<LeverageRuleImport>());
            long no = res.Read<long>().SingleOrDefault();
            var tuple = new Tuple<List<LeverageRuleImport>, long>(levrageViewModels, no);
            return tuple;
        }
        public async Task<bool> DeleteRuleConditionDtlById(int Id, int RuleConditionId)
        {
            var vParams = new DynamicParameters();
            vParams.Add("@Id", Id);
            var res = _db.QueryMultiple("[LeverageRuleConditionDTL_DeleteById]", vParams, commandType: CommandType.StoredProcedure);

            return true;
        }
    }
}
