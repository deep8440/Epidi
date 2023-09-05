using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Models.ViewModel.Steps
{
    public class StepsViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public int Priority { get; set; }
        public int PageSize { get; set; }
        public int EstimateTime { get; set; }
        public string FooterTitle { get; set; }

        public string FooterDescription { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime DeletedAt { get; set; }
        public int Code { get; set; }
        public string Message { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsTermsAndConditions { get; set; }

    }
}
