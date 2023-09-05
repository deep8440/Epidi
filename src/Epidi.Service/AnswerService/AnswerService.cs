using Dapper;
using Epidi.Models.DBConnection;
using Epidi.Models.Model;
using Epidi.Models.ViewModel.Common;
using Epidi.Models.ViewModel.Question;
using Epidi.Repository.AnswerRepository;
using Epidi.Repository.ConnectionProvider;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Service.AnswerService
{
    public class AnswerService : IAnswerService
    {
        private readonly IAnswerRepository  _answerRepository;
        public AnswerService(IAnswerRepository answerRepository)
        {
            _answerRepository = answerRepository;
        }
        public async Task<List<AnswerViewModel>> GetAnswerByQuestionId(long QuestionId)
        {
            return await _answerRepository.GetAnswerByQuestionId(QuestionId);
        }
		public List<ResponseViewModel> SaveAnswer(AnswerViewModel model)
        {
            return _answerRepository.SaveAnswer(model);
        }
		public async Task<AnswerViewModel> GetAnswerByAnswerId(long answerId)
        {
            return await _answerRepository.GetAnswerByAnswerId(answerId);
        }
		public List<ResponseViewModel> DeleteAnswer(int answerId)
        {
            return _answerRepository.DeleteAnswer(answerId);
        }

	}
}
