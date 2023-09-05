using Epidi.Models.Model;
using Epidi.Models.ViewModel.Common;
using Epidi.Models.ViewModel.Question;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Repository.UserQuestionAnswerRepository
{
    public interface IUserQuestionAnswerRepository
    {
        Task<long> Create(DataTable dt, int StepId, int CountryId);
       // Task<UPloadFiles> UploadFile(string DocumentName, int UserId,string files, string DocumentType);
       //Task<UPloadFiles> UploadFile(string DocumentName, int UserId,string files);
        Task<Response> Response(string DocumentName, int UserId,string files);
    }
}
