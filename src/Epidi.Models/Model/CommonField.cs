using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Models.Model
{
    public class CommonField
    {
        public bool IsActive { get; set; }

        public bool IsDelete { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? DeletedAt { get; set; }
    }
}
