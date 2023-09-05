using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Models.ViewModel.IB
{
    public class IBLimitViewModel
    {
    


        /// <summary>
        /// Gets or sets the Id value.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the BDMId value.
        /// </summary>
        /// 
        public int MarkUpPer1000 { get; set; }

        public int MarkUpPer { get; set; }

        /// <summary>
        /// Gets or sets the Name value.
        /// </summary>
        public int BuySwapPer1000 { get; set; }

        /// <summary>
        /// Gets or sets the ParentIBId value.
        /// </summary>
        public int BuySwapPer { get; set; }

        public int SellSwapPer1000 { get; set; }
        public int SellSwapPer { get; set; }

        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }

        public int UpdatedBy { get; set; }

        public DateTime UpdatedDate { get; set; }

        public int DeletedBy { get; set; }


        public DateTime DeletedDate { get; set;}

        public int MasterInstrumentId { get; set; }
        public string MasterInstrumentName { get; set; }
        public int SymbolGroupId { get; set; }
        public string SymbolName { get; set; }
        public bool IsActive { get; set; }  

    }

    public class IBLimit_ViewModel
    {

        public string MasterInstrumentName { get; set; }
        public string SymbolName { get; set; }

        /// <summary>
        /// Gets or sets the Id value.
        /// </summary>

        /// <summary>
        /// Gets or sets the BDMId value.
        /// </summary>
        /// 
        public int MarkUpPer1000 { get; set; }

        public int MarkUpPer { get; set; }

        /// <summary>
        /// Gets or sets the Name value.
        /// </summary>
        public int BuySwapPer1000 { get; set; }

        /// <summary>
        /// Gets or sets the ParentIBId value.
        /// </summary>
        public int BuySwapPer { get; set; }

        public int SellSwapPer1000 { get; set; }
        public int SellSwapPer { get; set; }

       
      
    }


}
