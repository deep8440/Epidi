using Dapper;
using Epidi.Models.DBConnection;
using Epidi.Models.Utility;
using Epidi.Models.ViewModel.QuoteTiming;
using Epidi.Models.ViewModel.SkipRule;
using Epidi.Repository.ConnectionProvider;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Epidi.Models.Paging;
using Epidi.Models.ViewModel.QuoteMarkUpRule;
using Epidi.Models.ViewModel.TermsCondition;
using Epidi.Models.ViewModel.Common;

namespace Epidi.Repository.MathRepository
{
    public class MathRepository : RepositoryBase,IMathRepository
    {
        public MathRepository(IOptions<ConnectionSettings> connectionOptions,
           IConnectionProvider connectionProvider
          ) : base(connectionOptions: connectionOptions,
               connectionProvider: connectionProvider)
        {
        }
        public async Task<long> AddMath(SkipRuleModelView Math)
        {
            try
            {
				long response = new();

				var vParams = new DynamicParameters();
				vParams.Add("@Id", Math.id);
				vParams.Add("@Name", Math.Name);
				vParams.Add("@Equation", Math.Equation);
				vParams.Add("@IsSkip", Math.IsSkip);
				vParams.Add("@Value", Math.Value);
				
				vParams.Add("@MasterInstrumentId", Math.MasterInstrumentIds);

				var res = await _db.QueryMultipleAsync("[Math_Upsert]", vParams, commandType: CommandType.StoredProcedure);

				response = res.Read<long>().FirstOrDefault();

				return response;
			}
            catch (Exception ex)
            {

                throw;
            }
        }
        
        public async Task<long> EditMath(SkipRuleModelView Math)
        {
            try
            {
            long retval = -1;
            retval = await _db.QuerySingleAsync<long>(
                    @"UPDATE [dbo].[Math]
                   SET [MasterInstrumentId] = @MasterInstrumentId
                      ,[StartTime] = @StartTime
                      ,[EndTime] = @EndTime
                      ,[Day] = @Day
                OUTPUT inserted.Id
                    WHERE Id=@id
                 ;", new
                    {
                            @MasterInstrumentId = Math.MasterInstrumentId,
                            //@StartTime = Math.StartTime,
                            //@EndTime = Math.EndTime,
                            //@Day = Math.Day,
                        @id = Math.id
                    }).ConfigureAwait(false);
            return retval;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        
       public Tuple<List<SkipRuleModelView>, long> GetMath(PageParam pageParam, string search)
        {
            List<SkipRuleModelView> response = new ();

            var vParams = new DynamicParameters();
            vParams.Add("@Offset", pageParam.Offset, dbType: DbType.Int64, direction: ParameterDirection.Input);
            vParams.Add("@Limit", pageParam.Limit, dbType: DbType.Int64, direction: ParameterDirection.Input);
            vParams.Add("@Search", search, dbType: DbType.String, direction: ParameterDirection.Input);
           
            var res = _db.QueryMultiple("[SkipRule_GetAll]", vParams, commandType: CommandType.StoredProcedure);

            response.AddRange(res.Read<SkipRuleModelView>());
            long no = res.Read<long>().SingleOrDefault();
            var tuple = new Tuple<List<SkipRuleModelView>, long>(response, no);
            return tuple;
   
        }

        public List<ResponseViewModel> RemoveMath(int Id)
        {

            List<ResponseViewModel> response = new List<ResponseViewModel>();

            var param = new DynamicParameters();
            param.Add("@Id", Id, dbType: DbType.Int32, direction: ParameterDirection.Input);

            var res = _db.QueryMultiple("[dbo].[Remove_Math]", param, commandType: CommandType.StoredProcedure);


            response = res.Read<ResponseViewModel>().ToList();

            return response;
        }



        //public async Task<long> RemoveMath(int id)
        //{
        //    try
        //    {
        //        long retval = -1;

        //        var vParams = new DynamicParameters();
        //        vParams.Add("@Id", id);

        //        var res = await _db.QueryMultipleAsync("[Remove_Math]", vParams, commandType: CommandType.StoredProcedure);

        //        retval = res.Read<long>().FirstOrDefault();

        //        return retval;
        //        //          retval = await _db.QuerySingleAsync<long>(
        //        //@"UPDATE SkipRule 
        //        //                      SET IsDelete = 1 ,
        //        //                      DeletedAt = GETDATE() 
        //        //                      OUTPUT inserted.Id 
        //        //                      WHERE Id = @id

        //        //                      UPDATE SkipRuleInstrument 
        //        //                      SET DeletedBy = 1 ,
        //        //                      DeletedDate = GETDATE() 
        //        //                      WHERE SkipRuleId = @id;", 
        //        //                  new
        //        //                  {
        //        //                      @id = id
        //        //                  }).ConfigureAwait(false);
        //        //          return retval;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}
        public async Task<SkipRuleModelView> GetMathById(int id)
        {
            SkipRuleModelView response = new();

            var vParams = new DynamicParameters();
            vParams.Add("@Id", id);
            var res = await _db.QueryMultipleAsync("[Get_MathById]", vParams, commandType: CommandType.StoredProcedure);

            response = res.Read<SkipRuleModelView>().FirstOrDefault();
            return response;
            //var result = await _db.QueryAsync<SkipRuleModelView>(@"SELECT  
            //    SR.*,SI.MasterInstrumentId as MasterInstrumentIds 
            //    FROM SkipRule as SR
            //    LEFT JOIN SkipRuleInstrument as SI ON SI.SkipRuleId = SR.Id
            //    WHERE SR.Id=" + id);
            //return result.FirstOrDefault();
        }
        public async Task<long> SkipRule_InsertByImport(DataTable dataTable)
        {
            SqlConnection con = new SqlConnection(_connectionOptions.ConnectionString);
            await con.OpenAsync();

            SqlCommand sqlCommand = new SqlCommand("SkipRule_InsertByImport", con);
            sqlCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter sqlParam = sqlCommand.Parameters.AddWithValue("@paramTable", dataTable);
            sqlParam.SqlDbType = SqlDbType.Structured;

            await sqlCommand.ExecuteNonQueryAsync();

            await con.CloseAsync();
            return 1;
        }

        public async Task<ResponseViewModel> CopyMathRule(int Id)
        {
            ResponseViewModel response = new ResponseViewModel();
            var vParams = new DynamicParameters();
            vParams.Add("@Id", Id);
            var res = _db.QueryMultiple("[Math_Copy]", vParams, commandType: CommandType.StoredProcedure);

            response = res.Read<ResponseViewModel>().FirstOrDefault();

            return response;
        }
    }
}
