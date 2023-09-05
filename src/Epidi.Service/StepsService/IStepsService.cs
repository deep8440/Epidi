using Epidi.Models.Paging;
using Epidi.Models.ViewModel.Common;
using Epidi.Models.ViewModel.Country;
using Epidi.Models.ViewModel.Question;
using Epidi.Models.ViewModel.Steps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Service.StepsService
{
    public interface IStepsService 
    {
        Task<Tuple<List<StepsViewModel>, long>> GetAllSteps(PageParam pageParam, string search,int CountryId = 0);
        List<ResponseViewModel> DeleteSteps(int Id);
        Task<StepsViewModel> GetStepsById(int Id);
        Task<StepsViewModel> SaveSteps(StepsViewModel model);
        Task<List<StepAndPageNumberStatusViewModel>> GetStepAndPageNumberStatusInfo(int CountryId = 0);
        Task<List<StepsViewModel>> Steps_GetByCountryId(int countryId, int userId);
    }
}

