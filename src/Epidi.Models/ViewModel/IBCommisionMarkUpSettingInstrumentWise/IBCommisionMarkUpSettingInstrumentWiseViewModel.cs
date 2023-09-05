using Epidi.Models.ViewModel.Question;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Models.ViewModel.IBCommissionMarkUpSettingInstrumentWise
{
    public class IBCommisionMarkUpSettingInstrumentWiseViewModel
    {
        /// <summary>
        /// Gets or sets the Id value.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the MasterInstrumentalId value.
        /// </summary>
        public int MasterInstrumentalId { get; set; }

        /// <summary>
        /// Gets or sets the MarkUpPer1000 value.
        /// </summary>
        public double MarkUpPer1000{ get; set; }

        /// <summary>
        /// Gets or sets the IBID value.
        /// </summary>
        public int IBID { get; set; }

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
        public double MarkUpCommssion { get; set; }
        public double BuySwapCommission { get; set; }
        public double SellSwapCommission { get; set; }
        public double MarkUpTotal { get; set; }
        public double BuySwapTotal { get; set; }
        public double SellSwapTotal { get; set; }

        /// <summary>
        /// Gets or sets the CreatedAt value.
        /// </summary>
        public DateTime? CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the CreatedBy value.
        /// </summary>
        public string? CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the ModifiedAt value.
        /// </summary>
        public DateTime? ModifiedAt { get; set; }

        /// <summary>
        /// Gets or sets the ModifiedBy value.
        /// </summary>
        public string? ModifiedBy { get; set; }

        /// <summary>
        /// Gets or sets the IsActive value.
        /// </summary>
        public bool IsActive { get; set; }
        /// <summary>
        /// Gets or sets the BDMID value.
        /// </summary>
        public int? BDMID { get; set; }

        [NotMapped]
        public string ib_name { get; set; }
        [NotMapped]
        public string InstrumentalName { get; set; }

        public int SymbolGroupId { get; set; }

        public string SymbolGroupName { get; set; }
    }
    public class CommisionMarkUpSetting 
    {
        public int Id { get; set; }
        public int MasterInstrumentalId { get; set; }
        public int BDMID { get; set; }
        public int IbId { get; set; }        
        public double MarkUpPer1000 { get; set; }
        public double MarkUpPer { get; set; }
        public double BuySwapPer1000 { get; set; }
        public double BuySwapPer { get; set; }        
        public double SellSwapPer1000 { get; set; }
        public double SellSwapPer { get; set; }
        public string  objCommissionString { get; set; }
        public string BdmName { get; set; }
        public string IbNo { get; set; }
        public string IbName { get; set; }
    }

    public class CommisionMarkUpSettingExport
    {
        public int MasterInstrumentalName { get; set; }
        public double MarkUpPer1000 { get; set; }
        public double MarkUpPer { get; set; }
        public double BuySwapPer1000 { get; set; }
        public double BuySwapPer { get; set; }
        public double SellSwapPer1000 { get; set; }
        public double SellSwapPer { get; set; }
    }
}
