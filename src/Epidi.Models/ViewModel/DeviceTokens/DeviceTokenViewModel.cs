using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Models.ViewModel.DeviceTokens
{
    public class DeviceTokenViewModel
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
        /// Gets or sets the Token value.
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Gets or sets the IMEI value.
        /// </summary>
        public string IMEI { get; set; }

        /// <summary>
        /// Gets or sets the IsActive value.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the OS value.
        /// </summary>
        public string OS { get; set; }

        /// <summary>
        /// Gets or sets the CreatedAt value.
        /// </summary>
        public int CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the CreatedDate value.
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the IsDeleted value.
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Gets or sets the DeletedDate value.
        /// </summary>
        public DateTime DeletedDate { get; set; }
    }
}
