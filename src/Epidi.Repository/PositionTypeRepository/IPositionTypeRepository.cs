using Epidi.Models.ViewModel.Common;
using Epidi.Models.ViewModel.PositionType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Repository.PositionTypeRepository
{
    public interface IPositionTypeRepository
    {
        Task<List<PositionTypeViewModel>> Get_All();
    }
}
