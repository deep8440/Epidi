using Epidi.Models.Paging;
using Epidi.Models.ViewModel.Common;
using Epidi.Models.ViewModel.Question;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Repository.QuestionsRepository
{
    public interface IQuestionRepository
    {
        Task<List<QuestionViewModel>> GetQuestionsByCountryIdStepId(long? CountryId, int? StepId, bool? IsActive,int PageNumber);

        Task<QuestionViewModel> GetQuestionByQuestionId(long QuestionId);
		Task<long> ManageQuestions(QuestionViewModel model);
		List<ResponseViewModel> SaveQuestions(QuestionViewModel model);
        List<QuestionViewModel> Steps_GetByCountryId(int CountryId);
        List<QuestionViewModel> OnlySteps_GetByCountryId(int CountryId);
        Tuple<List<QuestionViewModel>, long> GetAllQuestionForAdmin(PageParam pageParam, string search, int countryId, int PageNumber, string ColumnName, string SortType);
        List<ResponseViewModel> DeleteQuestion(int questionId);

        Task<long> GetQuestionCountByCountryId(long CountryId);
        Task<List<AnsTypesViewModel>> GetAllAnsTypes();

    }
}
