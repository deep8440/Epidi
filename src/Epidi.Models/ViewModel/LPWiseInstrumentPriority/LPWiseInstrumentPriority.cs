using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Models.ViewModel.LPWiseInstrumentPriority
{
    public class LPWiseInstrumentPriority
    {
        public long id { get; set; }
        public int Priority { get; set; }
        public int LPId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime DeletedDate { get; set; }
        public int DeletedBy { get; set; }
        public bool IsActive { get; set; }
        [NotMapped] 
        public string LPName { get; set; }
        [NotMapped]
        public string StartDateStr => StartDate != null ? StartDate.ToString("dd-MM-yyyy") : "-";
        [NotMapped]
        public string EndDateStr => EndDate != null ? EndDate.ToString("dd-MM-yyyy") : "-";
        [NotMapped]
        public string CreatedDateStr => CreatedDate != null ? CreatedDate.ToString("dd-MM-yyyy") : "-";
        [NotMapped]
        public string UpdatedDateStr => UpdatedDate != null ? UpdatedDate.ToString("dd-MM-yyyy") : "-";
        [NotMapped]
        public string DeletedDateStr => DeletedDate != null ? DeletedDate.ToString("dd-MM-yyyy") : "-";

    }
    
}
