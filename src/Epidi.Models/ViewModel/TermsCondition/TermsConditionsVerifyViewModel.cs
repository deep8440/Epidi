using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Models.ViewModel.TermsCondition
{
    public class TermsConditionsVerifyViewModel
    {
   

        /// <summary>
        /// Gets or sets the Id value.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the UserId value.
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Gets or sets the Token value.
        /// </summary>
        public long TermConditionId { get; set; }


public bool IsAgree { get; set; }

        
    public DateTime CreatedAt { get; set; }
	public bool IsDeleted { get; set; }







    }
}
