using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Models.ViewModel.UsersTransactions
{
    public class UsersTransactionViewModel
    {
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the UserId value.
        /// </summary>
        public int UserId { get; set; }

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

        /// <summary>
        /// Gets or sets the Comment value.
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Gets or sets the Amount value.
        /// </summary>
        public decimal Amount { get; set; }

        public string UserName { get; set; }
        public decimal CurrentBalance { get; set; }

    }
}
