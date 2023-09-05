using Epidi.Models.ViewModel.Common;
using Epidi.Models.ViewModel.Question;
using Epidi.Repository.UserQuestionAnswerRepository;
using System.Data;

namespace Epidi.Service.UserQuestionAnswerService
{
    public class UserQuestionAnswerService : IUserQuestionAnswerService
    {
        private readonly IUserQuestionAnswerRepository _userQuestionAnswerRepository;
        public UserQuestionAnswerService(IUserQuestionAnswerRepository userQuestionAnswerRepository)
        {
            _userQuestionAnswerRepository = userQuestionAnswerRepository;
        }


        public async Task<long> Create(DataTable dt, int StepId, int CountryId)
        {
            var retval = await _userQuestionAnswerRepository.Create(dt, StepId, CountryId);
            return retval;
        }

        //public async Task<UPloadFiles> UploadFile(string DocumentName, int UserId, string files, string DocumentType)
        //{
        //    var retval = await _userQuestionAnswerRepository.UploadFile(DocumentName, UserId, files, DocumentType);
        //    return retval;
        //}

        //public async Task<UPloadFiles> UploadFile(string DocumentName, int UserId, string files)
        //{
        //    var retval = await _userQuestionAnswerRepository.UploadFile(DocumentName, UserId, files);
        //    return retval;
        //}
		public async Task<Response> Response(string DocumentName, int UserId, string files)
		{
			var retval = await _userQuestionAnswerRepository.Response(DocumentName, UserId, files);
			return retval;
		}
	}
}
