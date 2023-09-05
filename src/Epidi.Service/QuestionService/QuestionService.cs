using Epidi.Models.ViewModel.Question;
using Epidi.Repository.AnswerRepository;
﻿using Epidi.Models.ViewModel.Common;
using Epidi.Repository.QuestionsRepository;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Epidi.Models.Paging;

namespace Epidi.Service.QuestionService
{
    public class QuestionService : IQuestionService
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly IAnswerRepository _answerRepository;
        public QuestionService(IQuestionRepository questionRepository, IAnswerRepository answerRepository)
        {
            _questionRepository = questionRepository;
            _answerRepository = answerRepository;
        }

        public List<QuestionViewModel> Steps_GetByCountryId(int countryId)
        {
            return _questionRepository.Steps_GetByCountryId(countryId);

		}
        
        public List<QuestionViewModel> OnlySteps_GetByCountryId(int countryId)
        {
            return _questionRepository.OnlySteps_GetByCountryId(countryId);

		}

        public async Task<QuestionViewModel> GetQuestionByQuestionId(long QuestionId)
        {
            return await _questionRepository.GetQuestionByQuestionId(QuestionId);

        }

        public async Task<List<QuestionViewModel>> GetQuestionsByCountryIdStepId(long? CountryId, int? StepId, bool? IsActive,int PageNumber = 0)
        {
            List<QuestionViewModel> questions = await _questionRepository.GetQuestionsByCountryIdStepId(CountryId, StepId, IsActive, PageNumber);

            foreach (var question in questions)
            {
                question.Answers = await _answerRepository.GetAnswerByQuestionId(question.QuestionId);
            }

            return questions;
        }


        public async Task<long> ManageQuestions(QuestionViewModel model)
        {
            return await _questionRepository.ManageQuestions(model);
        }

		public List<ResponseViewModel> SaveQuestions(QuestionViewModel model)
		{
			return _questionRepository.SaveQuestions(model);
		}
		public Tuple<List<QuestionViewModel>, long> GetAllQuestionForAdmin(PageParam pageParam, string search, int countryId, int PageNumber, string ColumnName, string SortType)
		{
			return _questionRepository.GetAllQuestionForAdmin(pageParam, search, countryId, PageNumber,ColumnName,SortType);
		}

		public List<ResponseViewModel> DeleteQuestion(int questionId)
		{
			return _questionRepository.DeleteQuestion(questionId);
		}
        public Task<long> GetQuestionCountByCountryId(long CountryId)
        {
            return _questionRepository.GetQuestionCountByCountryId(CountryId);
        }
        public async Task<List<AnsTypesViewModel>> GetAllAnsTypes()
        {
            return await _questionRepository.GetAllAnsTypes();
        }
    }
}
