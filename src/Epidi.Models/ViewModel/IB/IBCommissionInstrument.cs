namespace Epidi.Models.ViewModel.IB
{
    public class IBCommissionInstrument
    {
        /// <summary>
        /// Gets or sets the Id value.
        /// </summary>
        public int Id { get; set; }


        public int MasterInstrumentId { get; set; }
        public string MasterInstrumentName { get; set; }

        /// <summary>
        /// Gets or sets the BDMId value.
        /// </summary>
        /// 
        public double MarkUpPer1000 { get; set; }

        public double MarkUpPer { get; set; }

        /// <summary>
        /// Gets or sets the Name value.
        /// </summary>
        public double BuySwapPer1000 { get; set; }

        /// <summary>
        /// Gets or sets the ParentIBId value.
        /// </summary>
        public double BuySwapPer { get; set; }

        public double SellSwapPer1000 { get; set; }
        public double SellSwapPer { get; set; }


        public bool IsActive { get; set; }

        public int IBID { get; set; }

        public int SymbolGroupId { get; set; }
        public string SymbolGroupName { get; set; }
    }
}
