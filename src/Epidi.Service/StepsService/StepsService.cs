using Epidi.Models.Paging;
using Epidi.Models.ViewModel.Common;
using Epidi.Models.ViewModel.Country;
using Epidi.Models.ViewModel.Question;
using Epidi.Models.ViewModel.Steps;
using Epidi.Repository.CountryRepository;
using Epidi.Repository.StepsRepository;
using Epidi.Service.CountryService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Service.StepsService
{
    public class StepsService : IStepsService
    {
        private readonly IStepsRepository _repository;

        public StepsService(IStepsRepository repository)
        {
            _repository = repository;
        }

        public Task<Tuple<List<StepsViewModel>, long>> GetAllSteps(PageParam pageParam, string search, int CountryId = 0)
        {
            return _repository.GetAllSteps(pageParam, search, CountryId);
        }    

        public List<ResponseViewModel> DeleteSteps(int Id)
        {
            return _repository.DeleteSteps(Id);
        }
        public Task<StepsViewModel> GetStepsById(int Id)
        {
            return _repository.GetStepsById(Id);
        }

        public Task<StepsViewModel> SaveSteps(StepsViewModel model)
        {
            return _repository.SaveSteps(model);    
        }

        public Task<List<StepAndPageNumberStatusViewModel>> GetStepAndPageNumberStatusInfo(int CountryId = 0)
        {
            return _repository.GetStepAndPageNumberStatusInfo(CountryId);
        }

        public async Task<List<StepsViewModel>> Steps_GetByCountryId(int countryId, int userId)
        {
            return await _repository.Steps_GetByCountryId(countryId, userId);
        }
    }
}
