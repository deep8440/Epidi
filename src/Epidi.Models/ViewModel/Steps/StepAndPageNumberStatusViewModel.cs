using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Models.ViewModel.Steps
{
    public class StepAndPageNumberStatusViewModel
    {
        public int Id { get; set; }
        public int StepId { get; set; }
        public string Title { get; set; }
        public string Status { get; set; }
        public int? NextStep { get; set; }
        public int? NextPage { get; set; }
    }
}
