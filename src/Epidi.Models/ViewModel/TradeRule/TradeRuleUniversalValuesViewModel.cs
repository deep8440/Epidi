using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Models.ViewModel.TradeRule
{
	public class TradeRuleUniversalValuesViewModel
	{
		public int Id { get; set; }
		public int UniversalParametersValuesId { get; set; }
		public int ActionsDefintiionsId { get; set; }
		public string ActionDefinationName { get; set; }
		public int Priority { get; set; }
		public string Values { get; set; }
		public string Formulas { get; set; }
		public string Function { get; set; }
		public int FunctionsId { get; set; }

		public string Table { get; set; }
		public int TablesId { get; set; }
		public string WhenToCheck { get; set; }
		public int WhenToCheckId { get; set; }
		public string Comment { get; set; }
		public string Action { get; set; }

		public TradeRuleUniversalValuesViewModel()
		{
			itemlists = new List<Itemlist>() {
		new Itemlist { Text = "Floating", Value = "Floating" },
		new Itemlist { Text = "Avaialble Balance", Value = "Avaialble Balance" },
			new Itemlist { Text = "LPClose", Value = "LPClose" },
	new Itemlist { Text = "LPOpen", Value = "LPOpen" },
	new Itemlist { Text = "LPCurrentPriceAsk", Value = "LPCurrentPriceAsk" },
	new Itemlist { Text = "LPCurrentPriceBid", Value = "LPCurrentPriceBid" },
	new Itemlist { Text = "LPOpenPrice", Value = "LPOpenPrice" },
	new Itemlist { Text = "Open Price", Value = "Open Price" },

	new Itemlist { Text = "OpenReqPrice", Value = "OpenReqPrice" },

	new Itemlist { Text = "Close Price", Value = "Close Price" },
	new Itemlist { Text = "Current Bid", Value = "Current Bid" },
	new Itemlist { Text = "Current Ask", Value = "Current Ask" },

	new Itemlist { Text = "SLTriggeredPrice", Value = "SLTriggeredPrice" },
	new Itemlist { Text = "SLReqPRice", Value = "SLReqPRice" },
	new Itemlist { Text = "SLPRice", Value = "SLPRice" },
	new Itemlist { Text = "TPTriggeredPrice", Value = "TPTriggeredPrice" },

	new Itemlist { Text = "TPReqPrice", Value = "TPReqPrice" },

		new Itemlist { Text = "TPPrice", Value = "TPPrice" },
	new Itemlist { Text = "MarginLevel", Value = "MarginLevel" },
	new Itemlist { Text = "Decimals", Value = "Decimals" },


		};
		}

		public List<Itemlist> itemlists { get; set; }

	}
	
	public class Itemlist
	{
		public string Text { get; set; }
		public string Value { get; set; }
	}
}
