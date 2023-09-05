using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Models.ViewModel.User
{
    public class PaymentTransactionRespViewModel
    {
        public long Amount { get; set; }
        public string CreatedAt { get; set; }
        public string Currency { get; set; }
        public string CardName { get; set; }
        public string CustomerName { get; set; }
    }

	public class Capture
	{
		public string id { get; set; }
		public string status { get; set; }
		public int created_at { get; set; }
		public int amount { get; set; }
		public ExtraData extra_data { get; set; }
	}

	public class Card
	{
		public string id { get; set; }
	}

	public class Datum
	{
		public string id { get; set; }
		public Card card { get; set; }
		public List<object> captures { get; set; }
		public List<object> refunds { get; set; }
		public List<object> voids { get; set; }
		public List<object> attempts { get; set; }
		public int created_at { get; set; }
		public int amount { get; set; }
		public string currency { get; set; }
		public bool authorized { get; set; }
		public bool pre_auth { get; set; }
		public bool captured { get; set; }
		public bool refunded { get; set; }
		public object extra_data { get; set; }
		public object straal_custom_data { get; set; }
		public dynamic chargeback { get; set; }
		public object reference { get; set; }
		public object external_reference { get; set; }
		public object order_reference { get; set; }
		public object decline_reason { get; set; }
		public bool with_3ds_params { get; set; }
		public bool voided { get; set; }
		public string method { get; set; }
	}

	public class ExtraTransactionData
	{
	}

	public class Root
	{
		public int page { get; set; }
		public int per_page { get; set; }
		public int total_count { get; set; }
		public List<Datum> data { get; set; }
	}

	public class StraalCustomData
	{
	}

	public class Void
	{
		public string id { get; set; }
		public string status { get; set; }
		public int created_at { get; set; }
		public ExtraTransactionData extra_data { get; set; }
	}

}
