using Epidi.Models.ViewModel.PositionType;
using Epidi.Repository.PositionTypeRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Service.PositionTypeService
{
    public class PositionTypeService : IPositionTypeService
    {
        private readonly IPositionTypeRepository _PositionTypeRepository;
        public PositionTypeService(IPositionTypeRepository PositionTypeRepository) 
        { 
            _PositionTypeRepository= PositionTypeRepository;
        }
        public async Task<List<PositionTypeViewModel>> Get_All()
        {
            return await _PositionTypeRepository.Get_All();
        }
    }
}
