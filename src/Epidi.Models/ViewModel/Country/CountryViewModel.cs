using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Models.ViewModel.Country
{
    public class CountryViewModel
    {
        public long CountryId { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public string NiceName { get; set; }
        public string Iso3 { get; set; }
        public int NumCode { get; set; }
        public int PhoneCode { get; set; }
        public int OnboardingSteps { get; set; }
        public string Nationality { get; set; }
        public string TaxResidence { get; set; }
    }

    public class CountryDDl
    {
        public long Id { get; set; }
        public string CountryName { get; set; }
        public int PhoneCode { get; set; }
        public int OnboardingSteps { get; set; }
        public string Nationality { get; set; }
        public string TaxResidence { get; set; }
    }
}
