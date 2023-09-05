using Epidi.Models.Paging;
using Epidi.Models.ViewModel.Common;
using Epidi.Models.ViewModel.Question;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Service.QuestionService
{
    public interface IQuestionService
    {
        Task<List<QuestionViewModel>> GetQuestionsByCountryIdStepId(long? CountryId, int? StepId, bool? IsActive,int PageNumber = 0);

        Task<QuestionViewModel> GetQuestionByQuestionId(long QuestionId);

        Task<long> ManageQuestions(QuestionViewModel model);
	    List<ResponseViewModel> SaveQuestions(QuestionViewModel model);
        Tuple<List<QuestionViewModel>, long> GetAllQuestionForAdmin(PageParam pageParam, string search, int countryId, int PageNumber, string ColumnName, string SortType);
        List<ResponseViewModel> DeleteQuestion(int questionId);
        List<QuestionViewModel> Steps_GetByCountryId(int countryId);
        List<QuestionViewModel> OnlySteps_GetByCountryId(int countryId);

        Task<long> GetQuestionCountByCountryId(long CountryId);
        Task<List<AnsTypesViewModel>> GetAllAnsTypes();
    }
}
