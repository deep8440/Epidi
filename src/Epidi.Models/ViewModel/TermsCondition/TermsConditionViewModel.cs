using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Models.ViewModel.TermsCondition
{
    public class TermsConditionViewModel
    {
        public TermsConditionViewModel()
        {
            FileList = new List<TCMultipleFilViewModel>();
        }
        
        public int Id { get; set; }
        
        public string TermsConditionText { get; set; }
        
        public int CountryId { get; set; }
       
        public string CountryName { get; set; }
        
        public string FileUrl { get; set; }
        
        public bool AutoFill { get; set;  }
        
        public string Title { get; set; }

        public bool IsActive { get; set; }

        public List<TCMultipleFilViewModel> FileList { get; set; }

    

      
    }

    public class TCMultipleFilViewModel
    {
        public int Id { get; set; }
        public int TermsConditionId { get; set; }
        public int CountryId { get; set; }
        public string FileName { get; set; }
        public string FileUrl { get; set; }
        public string Title { get; set; }
       
    }
}
