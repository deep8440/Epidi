using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Models.ViewModel.PartnerSetting
{
    public class PartnerSettingViewModel
    {
		#region Properties
		/// <summary>
		/// Gets or sets the Id value.
		/// </summary>
		public int Id
		{ get; set; }

		/// <summary>
		/// Gets or sets the InstrumentId value.
		/// </summary>
		public int InstrumentId
		{ get; set; }

		/// <summary>
		/// Gets or sets the UserId value.
		/// </summary>
		public int UserId
		{ get; set; }

		/// <summary>
		/// Gets or sets the Markup_Per value.
		/// </summary>
		public double Markup_Per
		{ get; set; }

		/// <summary>
		/// Gets or sets the Markup_Amt value.
		/// </summary>
		public double Markup_Amt
		{ get; set; }

		/// <summary>
		/// Gets or sets the Buy_Per value.
		/// </summary>
		public double Buy_Per
		{ get; set; }

		/// <summary>
		/// Gets or sets the Buy_Amt value.
		/// </summary>
		public double Buy_Amt
		{ get; set; }

		/// <summary>
		/// Gets or sets the Sell_Per value.
		/// </summary>
		public double Sell_Per
		{ get; set; }

		/// <summary>
		/// Gets or sets the Sell_Amt value.
		/// </summary>
		public double Sell_Amt
		{ get; set; }

		/// <summary>
		/// Gets or sets the CreatedBy value.
		/// </summary>
		public int CreatedBy
		{ get; set; }

		/// <summary>
		/// Gets or sets the UpdatedBy value.
		/// </summary>
		public int UpdatedBy
		{ get; set; }

		/// <summary>
		/// Gets or sets the IsActive value.
		/// </summary>
		public bool IsActive
		{ get; set; }

		#endregion
	}
}
