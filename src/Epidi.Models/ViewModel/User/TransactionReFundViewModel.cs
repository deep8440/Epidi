using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Models.ViewModel.User
{
	public class TransactionReFundViewModel
	{
		public ExtraData extra_data { get; set; }
	}
	public class ExtraData
	{
		public string reason { get; set; }
		public string cardid { get; set; }
	}
}
