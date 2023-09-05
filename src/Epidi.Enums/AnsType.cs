using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Enums
{
    public enum AnsType
    {
        [Display(Name = "Dropdown")]
        DROPDOWN,
        [Display(Name = "Checkbox")]
        CHECKBOX,
        [Display(Name = "Radio")]
        RADIO,
        [Display(Name = "TextBox")]
        TEXTBOX
    }
}
