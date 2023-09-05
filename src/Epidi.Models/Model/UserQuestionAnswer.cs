using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Models.Model
{
    public class UserQuestionAnswer : CommonField
    {
        public long Id { get; set; }

        public long UserId { get; set; }

        public long QuestionId { get; set; }

        public long AnswerId { get; set; }
    }
}
