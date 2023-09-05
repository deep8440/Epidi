using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Models.ViewModel.MasterLP
{
    public class MasterLPViewModel
    {
        /// <summary>
        /// Gets or sets the Id value.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the Name value.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the HealthCheckURL value.
        /// </summary>
        public string HealthCheckURL { get; set; }

        /// <summary>
        /// Gets or sets the IsDown value.
        /// </summary>
        public bool IsDown { get; set; }

        /// <summary>
        /// Gets or sets the LastHealthCheckTime value.
        /// </summary>
        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime LastHealthCheckTime { get; set; }

        public string Username { get; set; }
        public string Password { get; set; }
        public string APIKey { get; set; }
        public string DNS { get; set; }

        /// <summary>
        /// Gets or sets the IsActive value.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the CreatedBy value.
        /// </summary>
        public int CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the CreatedDate value.
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the UpdatedBy value.
        /// </summary>
        public int UpdatedBy { get; set; }

        /// <summary>
        /// Gets or sets the UpdatedDate value.
        /// </summary>
        public DateTime UpdatedDate { get; set; }

        /// <summary>
        /// Gets or sets the DeletedBy value.
        /// </summary>
        public int DeletedBy { get; set; }

        /// <summary>
        /// Gets or sets the DeletedDate value.
        /// </summary>
        public DateTime DeletedDate { get; set; }
        [NotMapped]
        public string LastHealthCheckTimeStr => LastHealthCheckTime != null ? LastHealthCheckTime.ToString("dd-MMM-yyyy HH:mm") : "-";
    }
}
