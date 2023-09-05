using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Models.Model
{
    public  class Steps : CommonField
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int CountryId { get; set; }
    }
}
