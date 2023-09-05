using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Models.ViewModel.TradeTiming
{
    public class TradeTiming
    {
        public int id { get; set; }
        public int MasterInstrumentId { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string Day { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime DeletedDate { get; set; }
        public bool IsDelete { get; set; }
        [NotMapped]
        public string MasterInstrumentName { get; set; }
    }
    
}
