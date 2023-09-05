using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Models.ViewModel.Question
{
    public class AnswerViewModel
    {
        public long AnswerId { get; set; }

        public long QuestionId { get; set; }

        public string AnswerDesc { get; set; }

        public int Weightage { get; set; }


    }

    public class AnsTypesViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
