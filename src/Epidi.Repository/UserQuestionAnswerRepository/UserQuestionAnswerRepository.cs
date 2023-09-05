using Epidi.Models.DBConnection;
using Epidi.Models.ViewModel.Question;
using Epidi.Repository.ConnectionProvider;
using Microsoft.Extensions.Options;
using Dapper;
using System.Data;
using System.Data.SqlClient;
using Epidi.Models.ViewModel.Common;
using Epidi.Models.Model;
using System;
using System.Xml.Linq;

namespace Epidi.Repository.UserQuestionAnswerRepository
{
    public class UserQuestionAnswerRepository : RepositoryBase, IUserQuestionAnswerRepository
    {

        public UserQuestionAnswerRepository(IOptions<ConnectionSettings> connectionOptions,
             IConnectionProvider connectionProvider
            ) : base(connectionOptions: connectionOptions,
                 connectionProvider: connectionProvider)
        {
        }


        public async Task<List<QuestionViewModel>> GetQuestionsByCountryIdStepId(long? CountryId, int? StepId, bool? IsActive)
        {
            var result = await _db.QueryAsync<QuestionViewModel>(
             @"SELECT QuestionId, QuestionDesc, QuestionInfo, CountryId, StepId, AnsType as AnswerType FROM OnboardingQuestion
               WHERE (@CountryId IS NULL OR CountryId = @CountryId) 
               AND (@StepId IS NULL OR StepId = @StepId)
               AND (@IsActive IS NULL OR IsActive = @IsActive)",
             new
             {
                 CountryId,
                 StepId,
                 IsActive,
             });

            return result.ToList();
        }

        //public ResponseViewModel UploadFile(UPloadFiles model)
        //{
        //    var param = new DynamicParameters();
        //    param.Add("@DocumentName", model.DocumentName, dbType: DbType.Int32, direction: ParameterDirection.Input);
        //    param.Add("@UserId", model.UserId, dbType: DbType.String, direction: ParameterDirection.Input);
        //    param.Add("@files", model.files, dbType: DbType.String, direction: ParameterDirection.Input);
        //    param.Add("@QuestionId", model.QuestionId, dbType: DbType.String, direction: ParameterDirection.Input);

        //    var res = _db.QueryMultiple("[dbo].[UserQuestionAnswer_Update]", param, commandType: CommandType.StoredProcedure);

        //    // Assuming that the stored procedure returns a single ResponseViewModel, use .FirstOrDefault() to get the first result.
        //    var response = res.Read<ResponseViewModel>().FirstOrDefault();

        //    return response;
        //}


        public async Task<long> Create(DataTable dt, int StepId, int CountryId)
        {
            UserQuestionAnswerViewModel response = new();
            try
            {

                var QrequstModelCount = dt.Rows.Count;

                //var vParam = new DynamicParameters();
                //vParam.Add("@StepId", StepId);
                //vParam.Add("@countryId", CountryId);
                //var resp = await _db.ExecuteScalarAsync<int>("[UserQuestionCount]", vParam, commandType: CommandType.StoredProcedure);

                //if (QrequstModelCount ==)
                if (QrequstModelCount > 0)
                {
                    SqlConnection con = new SqlConnection(_connectionOptions.ConnectionString);
                    await con.OpenAsync();

                    SqlCommand sqlCommand = new SqlCommand("UserQuestionAnswer_Upsert", con);
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.CommandTimeout = 120;
                    SqlParameter sqlParam = sqlCommand.Parameters.AddWithValue("@tblUserQuestionAnswerParam", dt);

                    sqlParam.SqlDbType = SqlDbType.Structured;

                    var res = await sqlCommand.ExecuteScalarAsync();
                    await con.CloseAsync();

                    return Convert.ToInt32(res);
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                throw;
                return -1;
            }

        }

        //public async Task<UPloadFiles> UploadFile(string DocumentName, int UserId, string files, string DocumentType)
        public async Task<Response> Response(string DocumentName, int UserId, string files)
        {
            var param = new DynamicParameters();
            param.Add("@DocumentName", DocumentName);
            param.Add("@UserId", UserId);
            param.Add("@files", files);
            //.Add("@DocumentType", DocumentType);
            // param.Add("@DocumentPath");

            using (var connection = new SqlConnection(_connectionOptions.ConnectionString))
            {
                await connection.OpenAsync();
                var uploadFiles = new List<Response>();
                var res = await connection.QueryMultipleAsync("[dbo].[UserDoc_Update]", param, commandType: CommandType.StoredProcedure);
                try
                {
                     uploadFiles = res.Read<Response>().ToList();
                }
                catch (Exception ex)
                {
                
                }



                return uploadFiles[0];
            }

        }
    }
}



