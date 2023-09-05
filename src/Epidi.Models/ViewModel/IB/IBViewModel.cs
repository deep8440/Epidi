using Epidi.Models.ViewModel.IBCommissionMarkUpSettingInstrumentWise;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Models.ViewModel.IB
{
    public class IBViewModel
    {
        /// <summary>
        /// Gets or sets the Id value.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the BDMId value.
        /// </summary>
        /// 
        public string Name { get; set; }

        public int BDMId { get; set; }

        /// <summary>
        /// Gets or sets the Name value.
        /// </summary>
        public string bdmname { get; set; }

        /// <summary>
        /// Gets or sets the ParentIBId value.
        /// </summary>
        public int LegalEntityId { get; set; }

        public string LegalEntityName { get; set; }
        public int ParentIBId { get; set; }
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        [DefaultValue(3)]
        public double ParentCommissionPercentageRebate { get; set; }
        [DefaultValue(1)]
        public double BDMCommissionPercentage { get; set; }
        public IFormFile? File { get; set; }
        public string? objIBCommissionMarkUpSettingInstrumentWise { get; set; }
        public List<IBCommisionMarkUpSettingInstrumentWiseViewModel> objiBCommisionMarkUpSettingInstrument { get; set; }

    }
}
