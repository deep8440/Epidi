using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Models.ViewModel.BDMCommisionMarkUpSettingInstrumentWise
{
    public class BDMCommisionMarkUpSettingInstrumentWiseViewModel
    {
        #region Properties
        /// <summary>
        /// Gets or sets the Id value.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the MasterInstrumentalId value.
        /// </summary>
        public int MasterInstrumentalId { get; set; }
        public string? MasterInstrumentalName { get; set; }

        /// <summary>
        /// Gets or sets the BDMID value.
        /// </summary>
        public int BDMID { get; set; }

        /// <summary>
        /// Gets or sets the MarkUpPer1000 value.
        /// </summary>
        public double MarkUpPer1000 { get; set; }

        /// <summary>
        /// Gets or sets the MarkUpPer value.
        /// </summary>
        public double MarkUpPer { get; set; }

        /// <summary>
        /// Gets or sets the BuySwapPer1000 value.
        /// </summary>
        public double BuySwapPer1000 { get; set; }

        /// <summary>
        /// Gets or sets the BuySwapPer value.
        /// </summary>
        public double BuySwapPer { get; set; }

        /// <summary>
        /// Gets or sets the SellSwapPer1000 value.
        /// </summary>
        public double SellSwapPer1000 { get; set; }

        /// <summary>
        /// Gets or sets the SellSwapPer value.
        /// </summary>
        public double SellSwapPer { get; set; }

        /// <summary>
        /// Gets or sets the CreatedAt value.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the CreatedBy value.
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the ModifiedAt value.
        /// </summary>
        public DateTime ModifiedAt { get; set; }

        /// <summary>
        /// Gets or sets the ModifiedBy value.
        /// </summary>
        public string ModifiedBy { get; set; }

        /// <summary>
        /// Gets or sets the IsActive value.
        /// </summary>
        public bool IsActive { get; set; }
        [NotMapped]
        public string BdmName { get; set; }

        #endregion
    }
}
