using Dapper;
using Epidi.Models.DBConnection;
using Epidi.Models.Model;
using Epidi.Models.Paging;
using Epidi.Models.Utility;
using Epidi.Models.ViewModel.Common;
using Epidi.Models.ViewModel.Country;
using Epidi.Models.ViewModel.Question;
using Epidi.Repository.ConnectionProvider;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Repository.QuestionsRepository
{
    public class QuestionRepository : RepositoryBase, IQuestionRepository
    {

        public QuestionRepository(IOptions<ConnectionSettings> connectionOptions,
            IConnectionProvider connectionProvider
           ) : base(connectionOptions: connectionOptions,
                connectionProvider: connectionProvider)
        {
        }


        public async Task<List<QuestionViewModel>> GetQuestionsByCountryIdStepId(long? CountryId, int? StepId, bool? IsActive, int PageNumber)
        {
            List<QuestionViewModel> response = new();

            var vParams = new DynamicParameters();
            vParams.Add("@CountryId", CountryId);
            vParams.Add("@PageNumber", PageNumber);
            vParams.Add("@StepId", StepId);
            vParams.Add("@IsActive", IsActive);

            var res = _db.QueryMultiple("[Get_QuestionsBy_CountryId_StepId]", vParams, commandType: CommandType.StoredProcedure);

            response = res.Read<QuestionViewModel>().ToList();
            return response;

            //var result = await _db.QueryAsync<QuestionViewModel>(
            //     @"SELECT QuestionId, 
            //                  QuesDesc AS QuestionDesc,
            //                  QuestionInfo, 
            //                  CountryId, 
            //                  StepId, 
            //                  Priority,
            //                  PageNumber,
            //                  AnsType,
            //                  PageTitle AS Title,
            //                  CASE AnsType WHEN 1 THEN 'DropDown' WHEN 2 THEN 'CheckBox' WHEN 3 THEN 'Radio' WHEN 4 THEN 'Text' ELSE 'Datepicker' END AS AnswerTypeText
            //                  FROM OnboardingQuestion
            //   WHERE (@CountryId = 0 OR CountryId = @CountryId) 
            //   AND (@PageNumber = 0 OR PageNumber = @PageNumber)
            //   AND (@StepId = 0 OR StepId = @StepId)
            //   AND (@IsActive = 0 OR IsActive = @IsActive)",
            //     new
            //     {
            //         @CountryId = CountryId,
            //         @StepId = StepId,
            //         @IsActive = IsActive,
            //         @PageNumber = PageNumber
            //     });

            //return result.ToList();

        }

        public async Task<long> GetQuestionCountByCountryId(long CountryId)
        {
            var vParams = new DynamicParameters();
            vParams.Add("@CountryId", CountryId);

            var res = await _db.ExecuteScalarAsync<long>("[Get_QuestionCountBy_CountryId]", vParams, commandType: CommandType.StoredProcedure);

            //retval = res.Read<long>().FirstOrDefault();

            return res;

            //var questionCount = await _db.ExecuteScalarAsync<long>(
            //      @"SELECT 
            //    COUNT(1) FROM OnboardingQuestion
            //   WHERE (@CountryId = 0 OR CountryId = @CountryId) 
            //   AND (IsActive = 1)",
            //     new
            //     {
            //         @CountryId = CountryId
            //     });

            //return questionCount;

        }

        public async Task<QuestionViewModel> GetQuestionByQuestionId(long QuestionId)
        {

            QuestionViewModel response = new();

            var vParams = new DynamicParameters();
            vParams.Add("@QuestionId", QuestionId);
            var res = await _db.QueryMultipleAsync("[Get_QuestionBy_QuestionId]", vParams, commandType: CommandType.StoredProcedure);

            response = res.Read<QuestionViewModel>().FirstOrDefault();
            return response;

            //var result = await _db.QueryFirstOrDefaultAsync<QuestionViewModel>(
            //@"SELECT QuestionId, QuesDesc AS QuestionDesc, QuestionInfo, CountryId, StepId,AnsType,Priority,PageNumber,PageTitle, AnswerTypeText FROM OnboardingQuestion
            //   WHERE (@QuestionId IS NULL OR QuestionId = @QuestionId)",
            //new
            //{
            //    QuestionId
            //});

            //return result;
        }
        public async Task<long> ManageQuestions(QuestionViewModel model)
        {
            long retval = -1;
            var vParams = new DynamicParameters();
            vParams.Add("@QuesDesc", model.QuestionDesc);
            vParams.Add("@QuesInfo", model.QuestionInfo);
            vParams.Add("@CountryId", model.CountryId);
            vParams.Add("@StepId", model.StepId);
            vParams.Add("@IsActive", model.IsActive);
            vParams.Add("@QuestionId", model.QuestionId);

            var res = await _db.QueryMultipleAsync("[Manage_Questions]", vParams, commandType: CommandType.StoredProcedure);

            retval = res.Read<long>().FirstOrDefault();

            return retval;
            //if (model.QuestionId != 0)
            //{
            //    await _db.QuerySingleAsync<long>(
            //             @"UPDATE OnboardingQuestion
            //               SET QuesDesc =  @QuesDesc,
            //                   QuesInfo = @QuesInfo,
            //                   CountryId = @CountryId,
            //                   StepId = @StepId 
            //                   IsActive = @IsActive
            //                 WHERE QuestionId = @QuestionId;", new
            //             {
            //                 model.QuestionDesc,
            //                 model.QuestionInfo,
            //                 model.CountryId,
            //                 model.StepId,
            //                 model.IsActive,
            //                 @QuestionId = model.QuestionId,

            //             }).ConfigureAwait(false);

            //    return model.QuestionId;
            //}
            //else
            //{

            //    retval = await _db.QuerySingleAsync<long>(@"INSERT INTO OnboardingQuestion (QuesDesc,StepId, CountryId, QuestionInfo, IsActive)
            //            OUTPUT inserted.QuestionId                    
            //            VALUES(@QuesDesc, @StepId, @CountryId, @QuestionInfo, @IsActive)",
            //        new { model.QuestionDesc, model.StepId, model.CountryId, model.QuestionInfo, model.IsActive });
            //    return retval;
            //}


        }

        public List<ResponseViewModel> SaveQuestions(QuestionViewModel model)
        {

            List<ResponseViewModel> response = new List<ResponseViewModel>();

            var param = new DynamicParameters();
            param.Add("@QuestionId", model.QuestionId, dbType: DbType.Int32, direction: ParameterDirection.Input);
            param.Add("@QuestionDesc", model.QuestionDesc, dbType: DbType.String, direction: ParameterDirection.Input);
            param.Add("@QuestionInfo", model.QuestionInfo, dbType: DbType.String, direction: ParameterDirection.Input);
            param.Add("@CountryId", model.CountryId, dbType: DbType.Int32, direction: ParameterDirection.Input);
            param.Add("@StepId", model.StepId, dbType: DbType.Int32, direction: ParameterDirection.Input);
            param.Add("@AnsType", model.AnsType, dbType: DbType.Int32, direction: ParameterDirection.Input);
            param.Add("@AnswerTypeText", model.AnswerTypeText, dbType: DbType.String, direction: ParameterDirection.Input);
            param.Add("@DefaultWeightage", model.DefaultWeightage, dbType: DbType.Int32, direction: ParameterDirection.Input);
            param.Add("@PageNumber", model.PageNumber, dbType: DbType.Int32, direction: ParameterDirection.Input);
            param.Add("@PageTitle", model.PageTitle, dbType: DbType.String, direction: ParameterDirection.Input);
            param.Add("@Priority", model.Priority, dbType: DbType.Int32, direction: ParameterDirection.Input);
            param.Add("@IsCountryDropdown", model.IsCountryDropdown);

            var res = _db.QueryMultiple("[dbo].[Question_Upsert]", param, commandType: CommandType.StoredProcedure);


            response = res.Read<ResponseViewModel>().ToList();

            return response;

        }

        //public async Task<List<CountryDDl>> GetAllCountries(int CountryId)
        //{
        //    var result = await _db.QueryAsync<CountryDDl>(
        //       @"SELECT st.Id,
        //             st.Title,
        //             st.CountryId,
        //             ct.CountryName,
        //             st.IsActive,
        //             st.IsDelete,
        //             st.CreatedAt,
        //             st.DeletedAt
        //        FROM Steps st
        //        INNER JOIN Country ct ON st.CountryId=ct.CountryId
        //        WHERE st.CountryId = @CountryId"
        //       );

        //    return result.ToList();
        //}

        public List<QuestionViewModel> Steps_GetByCountryId(int countryId)
        {
            List<QuestionViewModel> questionViewModels = new List<QuestionViewModel>();
            var param = new DynamicParameters();
            param.Add("@CountryId", countryId, dbType: DbType.Int64, direction: ParameterDirection.Input);


            var res = _db.QueryMultiple("[dbo].[Steps_GetByCountryId]", param, commandType: CommandType.StoredProcedure);

            questionViewModels.AddRange(res.Read<QuestionViewModel>());
            var tuple = new List<QuestionViewModel>(questionViewModels);
            return questionViewModels;
        }
        public List<QuestionViewModel> OnlySteps_GetByCountryId(int countryId)
        {
            List<QuestionViewModel> questionViewModels = new List<QuestionViewModel>();
            var param = new DynamicParameters();
            param.Add("@CountryId", countryId, dbType: DbType.Int64, direction: ParameterDirection.Input);


            var res = _db.QueryMultiple("[dbo].[OnlySteps_GetByCountryId]", param, commandType: CommandType.StoredProcedure);

            questionViewModels.AddRange(res.Read<QuestionViewModel>());
            var tuple = new List<QuestionViewModel>(questionViewModels);
            return questionViewModels;
        }

        public Tuple<List<QuestionViewModel>, long> GetAllQuestionForAdmin(PageParam pageParam, string search, int countryId, int PageNumber, string ColumnName, string SortType)
        {
            List<QuestionViewModel> questionViewModels = new List<QuestionViewModel>();
            var param = new DynamicParameters();
            param.Add("@Offset", pageParam.Offset, dbType: DbType.Int64, direction: ParameterDirection.Input);
            param.Add("@Limit", pageParam.Limit, dbType: DbType.Int64, direction: ParameterDirection.Input);
            param.Add("@Search", search, dbType: DbType.String, direction: ParameterDirection.Input);
            param.Add("@CountryId", countryId, dbType: DbType.Int32, direction: ParameterDirection.Input);
            param.Add("@PageNumber", PageNumber, dbType: DbType.Int32, direction: ParameterDirection.Input);
            param.Add("@ColumnName", ColumnName, dbType: DbType.String, direction: ParameterDirection.Input);
            param.Add("@SortType", SortType, dbType: DbType.String, direction: ParameterDirection.Input);

            var res = _db.QueryMultiple("[dbo].[OnBoardingQuestion_All]", param, commandType: CommandType.StoredProcedure);

            questionViewModels.AddRange(res.Read<QuestionViewModel>());
            long no = res.Read<long>().SingleOrDefault();
            var tuple = new Tuple<List<QuestionViewModel>, long>(questionViewModels, no);
            return tuple;
        }

        public List<ResponseViewModel> DeleteQuestion(int questionId)
        {

            List<ResponseViewModel> response = new List<ResponseViewModel>();

            var param = new DynamicParameters();
            param.Add("@QuestionId", questionId, dbType: DbType.Int32, direction: ParameterDirection.Input);

            var res = _db.QueryMultiple("[dbo].[OnboardingQuestion_Delete]", param, commandType: CommandType.StoredProcedure);


            response = res.Read<ResponseViewModel>().ToList();

            return response;

        }

        public async Task<List<AnsTypesViewModel>> GetAllAnsTypes()
        {
            List<AnsTypesViewModel> response = new();


            var res = _db.QueryMultiple("[Get_All_AnsTypes]", commandType: CommandType.StoredProcedure);

            response = res.Read<AnsTypesViewModel>().ToList();
            return response;
            //var result = await _db.QueryAsync<AnsTypesViewModel>(
            //   @"SELECT Id,Name FROM AnsTypes WHERE IsActive = 1"
            //   );

            //return result.ToList();
        }
    }
}
