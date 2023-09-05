using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Models.ViewModel.Question
{
    public class QuestionViewModel
    {
        public QuestionViewModel()
        {
            Answers = new List<AnswerViewModel>();
        }
        public long QuestionId { get; set; }

        public int StepId { get; set; }

        public string? QuestionInfo { get; set; }

        public string? QuestionDesc { get; set; }

        public long CountryId { get; set; }
        public string Title { get; set; }
        public string CountryName { get; set; }
        public string StepsTitle { get; set; }
        public int AnsType { get; set; }
        public string AnsTypeName { get; set; }
        public int Priority { get; set; }
        public int PageNumber { get; set; }
        public string PageTitle { get; set; }
        public string AnswerTypeText { get; set; }

        public bool IsActive { get; set; }

        public int DefaultWeightage { get; set; }

        public List<AnswerViewModel> Answers { get; set; }

        public bool IsCountryDropdown { get; set; }
    }
}
