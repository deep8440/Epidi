using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Models.ViewModel.User
{
	public class CreateCardViewModel
	{
		public long userId { get; set; }
		public string name { get; set; }
		public string number { get; set; }
		public string cvv { get; set; }
		public string expiry_month { get; set; }
		public string expiry_year { get; set; }
		public string origin_ipaddr { get; set; }
	}

	public class DisableCardViewModel
	{
		public string cardid { get; set; }
		public string initiated_by { get; set; }
	}
}
