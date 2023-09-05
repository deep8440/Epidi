using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Models.ViewModel.User
{
	public class CreateTransactionViewModel
	{
		public string cardid { get; set; }
		public long amount { get; set; }
		public string currency { get; set; }
		public string reference { get; set; }
		public string type { get; set; }
	}
}
