using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Models.Model
{
    public class Order
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public string Orders { get; set; }
        public string TradingAccount { get; set; }
        public string Side { get; set; }
        public string EquityName { get; set; }
        public DateTime OpenTiming { get; set; }
        public string? OpenTimingstr { get; set; }
        public string LegalEntity { get; set; }
        public decimal OpenPrice { get; set; }
        public decimal OpenUnits { get; set; }
        public decimal SL { get; set; }
        public decimal TP { get; set; }
        public decimal ClosePrice { get; set; }
        public decimal Swap { get; set; }
        public decimal Commision { get; set; }
        public decimal Profit { get; set; }
        public decimal Leverage { get; set; }
        public string CommentOfPosition { get; set; }
    }
}
