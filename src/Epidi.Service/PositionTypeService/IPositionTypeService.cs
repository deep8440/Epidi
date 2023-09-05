using Epidi.Models.ViewModel.PositionType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Service.PositionTypeService
{
    public interface IPositionTypeService
    {
        Task<List<PositionTypeViewModel>> Get_All();

    }
}
