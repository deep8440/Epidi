using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Models.Model
{
    public class Answer : CommonField
    {
        public long  AnswerId { get; set; }

        public long QuestionId { get; set; }

        public string AnswerDesc { get; set; }
    }
}
