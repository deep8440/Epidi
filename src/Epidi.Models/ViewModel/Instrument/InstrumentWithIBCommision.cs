namespace Epidi.Models.ViewModel.Instrument
{
    public class InstrumentWithIBCommision
    {
        public int Id { get; set; }
        public int MasterInstrumentalId { get; set; }
        public string Name { get; set; } = string.Empty;
        public float MarkUpPer1000 { get; set; }
        public int IBID { get; set; }
        public float MarkUpPer { get; set; }
        public float BuySwapPer1000 { get; set; }
        public float BuySwapPer { get; set; }
        public float SellSwapPer1000 { get; set; }
        public float SellSwapPer { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime ModifiedAt { get; set; }
        public string ModifiedBy { get; set; } = string.Empty;
    }
}
