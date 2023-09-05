using Epidi.Models.ViewModel.BDMCommisionMarkUpSettingInstrumentWise;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Models.ViewModel.BDM
{
    public class BDMViewModel
    {
        public int Id { get; set; }

		public string Name { get; set; }
        [NotMapped]
        public int MasterInstrumentalId { get; set; }
        [NotMapped]
        public string MasterInstrumentalName { get; set; }

        public BDMCommisionMarkUpSettingInstrumentWiseViewModel BDMCommision { get; set; }
    }
}
