using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Models.ViewModel.User
{
	public class CreatePaymentLinkViewModel
	{
		public long userId { get; set; }
		public string currency { get; set; }
		public long amount { get; set; }
		public string return_url { get; set; }
		public string failure_url { get; set; }
		public string description { get; set; }
		public string reference { get; set; }
		public string order_reference { get; set; }
		public string origin_ipaddr { get; set; }
		public string payment_method_ident { get; set; }
	}
}
