using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Models.ViewModel.PayInoutRequest
{
    public class PayInoutRequestViewModel
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
		/// Gets or sets the Status value.
		/// </summary>
		public string Status { get; set; }

		/// <summary>
		/// Gets or sets the ActionDate value.
		/// </summary>
		public DateTime ActionDate { get; set; }

		/// <summary>
		/// Gets or sets the Amount value.
		/// </summary>
		public decimal Amount { get; set; }

		/// <summary>
		/// Gets or sets the RequestType value.
		/// </summary>
		public string RequestType { get; set; }

		/// <summary>
		/// Gets or sets the CustomerNote value.
		/// </summary>
		public string CustomerNote { get; set; }

		/// <summary>
		/// Gets or sets the AdminNote value.
		/// </summary>
		public string AdminNote { get; set; }

		/// <summary>
		/// Gets or sets the CreatedDate value.
		/// </summary>
		public DateTime CreatedDate { get; set; }

		/// <summary>
		/// Gets or sets the CreatedBy value.
		/// </summary>
		public int CreatedBy { get; set; }
		[NotMapped]
		public string UserName { get; set; }
        public decimal CurrentBalance { get; set; }
		public string Comment { get; set; }
		public string RequestName { get; set; }
        public DateTime DeletedDate { get; set; }
		public bool IsDeleted { get; set; }


    }
}
