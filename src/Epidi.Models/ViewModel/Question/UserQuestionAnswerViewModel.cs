using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Models.ViewModel.Question
{
    public class UserQuestionAnswerFileRequest
    {
        public UPloadFiles uPloadFiles { get; set; }


    }
    public class UPloadFiles
    {
       // public string DocumentName { get; set; }
        public int UserId { get; set; }
        //public int QuestionId { get; set; }
        public IFormFile? files { get; set; }

        //public string file_path { get; set; }
        //public int PageNumber { get; set; }
        //public int StepId { get; set; }
       // public string DocumentType { get; set; }
        //public string /*DocumentType*/ { get; set; }
    }

    public class UserQuestionAnswerViewModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int QuestionId { get; set; }
        //public long AnswerId { get; set; }
        public string AnswerId { get; set; }
        public string AnswerDesc { get; set; }
        public int StepId { get; set; }
        public int countryId { get; set; }
        public int PageNumber { get; set; }
        public int CreatedAt { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? UpdateAt { get; set; }
        public DateTime UpdateDate { get; set; }
        //public IFormFile FileDetails { get; set; }
        public string? DocumentPath { get; set; }
        public string? DocumentName { get; set; }
    }

	public class Response
    {
		public string file_path { get; set; }
		public int UserId { get; set; }
		public int Id { get; set; }
		public string DocumentName { get; set; }
	}
}
