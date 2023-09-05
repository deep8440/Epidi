using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Models.ViewModel.Transaction
{
    public class TransactionViewModel
    {
		/// <summary>
		/// Gets or sets the Id value.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// Gets or sets the UserId value.
		/// </summary>
		public int UserId { get; set; }

		/// <summary>
		/// Gets or sets the PayInPOutId value.
		/// </summary>
		public int PayInPOutId { get; set; }

		/// <summary>
		/// Gets or sets the Amount value.
		/// </summary>
		public decimal Amount { get; set; }

		/// <summary>
		/// Gets or sets the Note value.
		/// </summary>
		public string Note { get; set; }

		/// <summary>
		/// Gets or sets the Type value.
		/// </summary>
		public string Type { get; set; }

		/// <summary>
		/// Gets or sets the CreatedDate value.
		/// </summary>
		public DateTime CreatedDate { get; set; }

		/// <summary>
		/// Gets or sets the CreatedBy value.
		/// </summary>
		public int CreatedBy { get; set; }
	}
}
