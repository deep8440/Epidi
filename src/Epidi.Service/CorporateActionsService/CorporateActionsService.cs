using Epidi.Models.ViewModel.CorporateActions;
using Epidi.Models.ViewModel.InstrumentMaster;
using Epidi.Repository.CorporateActionsRepository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Service.CorporateActionsService
{
    public class CorporateActionsService : ICorporateActionsService
    {
        private readonly ICorporateActionsRepository _corporateActionsRepository;

        public CorporateActionsService(ICorporateActionsRepository corporateActionsRepository)
        {
            _corporateActionsRepository = corporateActionsRepository;
        }

        public async Task<List<InstrumentMasterViewModel>> Get_MasterInstrument()
        {
            return await _corporateActionsRepository.Get_MasterInstrument();
        }
        
        public async Task<List<StockDetailsViewModel>> GetStockDetails(long InstrumentId)
        {
            return await _corporateActionsRepository.GetStockDetails(InstrumentId);
        }
       
        public async Task<List<StockDetailsViewModel>> SaveNewEquityStockDetails(long OldInstrumentId, long NewInstrumentId, string OrderIds)
        {
            return await _corporateActionsRepository.SaveNewEquityStockDetails(OldInstrumentId,NewInstrumentId, OrderIds);
        }
        public async Task<List<StockDetailsViewModel>> SaveDelstingComment(string OrderIds)
        {
            return await _corporateActionsRepository.SaveDelstingComment(OrderIds);
        }
        public async Task<List<SplitCalculatedOrderViewModel>> GetCalculatedSplitOrderDetails(SplitViewModel splitViewModel)
        {
            return await _corporateActionsRepository.GetCalculatedSplitOrderDetails(splitViewModel);
        }
        public async Task<List<DelistingOrderDetailsViewModel>> GetCalculatedDelistingOrderDetails(DelistingViewModel delistingViewModel)
        {
            return await _corporateActionsRepository.GetCalculatedDelistingOrderDetails(delistingViewModel);
        }
        public async Task<List<SplitCalculatedOrderViewModel>> GetCalculatedReverseSplitOrderDetails(SplitViewModel splitViewModel)
        {
            return await _corporateActionsRepository.GetCalculatedReverseSplitOrderDetails(splitViewModel);
        }  
        public Tuple<List<SpinOffOldOrderViewModel>, List<SpinOffNewOrderViewModel>> GetCalculatedSpinOffOrderDetails(SpinoffViewModel spinoffViewModel)
        {
            return _corporateActionsRepository.GetCalculatedSpinOffOrderDetails(spinoffViewModel);
        }
        public List<DividendDetailsViewModel> GetDivedendDetails(string InstrumentId, string FromDate)
        {
            return _corporateActionsRepository.GetDivedendDetails(InstrumentId, FromDate);
        }
        public List<DividendDetailsViewModel> ExportDivedendDetails()
        {
            return _corporateActionsRepository.ExportDivedendDetails();
        }
        public List<DividendOrdersViewModel> GetDivedendOrderDetails(int InstrumentId, string FromDate, string ToDate)
        {
            return _corporateActionsRepository.GetDivedendOrderDetails(InstrumentId, FromDate, ToDate);
        }
        public async Task<long> AddUpdateSplitOrder(DataTable dataTable)
        {
            return await _corporateActionsRepository.AddUpdateSplitOrder(dataTable);
        }
        public async Task<long> AddUpdateDividenMaster(DataTable dataTable)
        {
            return await _corporateActionsRepository.AddUpdateDividenMaster(dataTable);
        }
        public async Task<long> AddUpdateReverseSplitOrder(DataTable dataTable)
        {
            return await _corporateActionsRepository.AddUpdateReverseSplitOrder(dataTable);
        }
        public async Task<long> AddUpdateSpinOffOrder(DataTable dataTable)
        {
            return await _corporateActionsRepository.AddUpdateSpinOffOrder(dataTable);
        }

    }
}
