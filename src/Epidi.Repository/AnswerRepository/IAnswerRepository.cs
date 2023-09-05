using Epidi.Models.ViewModel.Common;
using Epidi.Models.ViewModel.Question;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Repository.AnswerRepository
{
    public interface IAnswerRepository
    {
        Task<List<AnswerViewModel>> GetAnswerByQuestionId(long QuestionId);
        public List<ResponseViewModel> SaveAnswer(AnswerViewModel model);
        Task<AnswerViewModel> GetAnswerByAnswerId(long answerId);
        public List<ResponseViewModel> DeleteAnswer(int answerId);

	}
}
