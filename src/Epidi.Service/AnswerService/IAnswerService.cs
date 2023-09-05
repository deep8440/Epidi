using Epidi.Models.ViewModel.Common;
using Epidi.Models.ViewModel.Question;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Service.AnswerService
{
    public interface IAnswerService
    {
        Task<List<AnswerViewModel>> GetAnswerByQuestionId(long QuestionId);
        List<ResponseViewModel> SaveAnswer(AnswerViewModel model);
        Task<AnswerViewModel> GetAnswerByAnswerId(long answerId);
        List<ResponseViewModel> DeleteAnswer(int answerId);

	}
}
