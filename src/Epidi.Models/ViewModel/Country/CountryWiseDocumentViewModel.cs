using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Models.ViewModel.Country
{
    public class CountryWiseDocumentViewModel
    {
        public int Id { get; set; }
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public string DocumentName { get; set; }
    }

    public class CopyCountryDataViewModel
    {
        public int Id { get; set; }
        public int FromCountry { get; set; }
        public int ToCountry { get; set; }
    }
}
