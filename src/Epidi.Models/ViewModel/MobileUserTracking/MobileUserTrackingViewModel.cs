using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Models.ViewModel.MobileUserTracking
{
    public class MobileUserTrackingViewModel
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
        /// Gets or sets the ModuleName value.
        /// </summary>
        public string ModuleName { get; set; }

        /// <summary>
        /// Gets or sets the ScreenName value.
        /// </summary>
        public string ScreenName { get; set; }

        /// <summary>
        /// Gets or sets the TraceNumber value.
        /// </summary>
        public string TraceNumber { get; set; }

        /// <summary>
        /// Gets or sets the CreatedDate value.
        /// </summary>
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
    }
}
