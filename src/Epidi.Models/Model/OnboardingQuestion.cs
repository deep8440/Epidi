using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Models.Model
{
    public class OnboardingQuestion : CommonField
    {
        public long QuestionId { get; set; }

        public string QuesDesc { get; set; }

        public string QuestionInfo { get; set; }

        public long CountryId { get; set; } = 0;

        public int StepId { get; set; }
    }
}
