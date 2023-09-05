using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Models.ViewModel.LookUpInstrument
{
    public class LookUpInstrumentViewModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
