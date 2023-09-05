using Epidi.Models.ViewModel.CorporateActions;
using Epidi.Models.ViewModel.InstrumentMaster;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Repository.CorporateActionsRepository
{
    public interface ICorporateActionsRepository
    {
        Task<List<InstrumentMasterViewModel>> Get_MasterInstrument();
        Task<List<StockDetailsViewModel>> GetStockDetails(long InstrumentId);
        Task<List<StockDetailsViewModel>> SaveNewEquityStockDetails(long OldInstrumentId, long NewInstrumentId, string OrderIds);
        Task<List<StockDetailsViewModel>> SaveDelstingComment(string OrderIds);
        Task<List<SplitCalculatedOrderViewModel>> GetCalculatedSplitOrderDetails(SplitViewModel splitViewModel);
        Task<long> AddUpdateSplitOrder(DataTable dataTable);
        Task<List<SplitCalculatedOrderViewModel>> GetCalculatedReverseSplitOrderDetails(SplitViewModel splitViewModel);
        Task<long> AddUpdateReverseSplitOrder(DataTable dataTable);
        Task<List<DelistingOrderDetailsViewModel>> GetCalculatedDelistingOrderDetails(DelistingViewModel delistingViewModel);
        Tuple<List<SpinOffOldOrderViewModel>, List<SpinOffNewOrderViewModel>> GetCalculatedSpinOffOrderDetails(SpinoffViewModel spinoffViewModel);
        Task<long> AddUpdateSpinOffOrder(DataTable dataTable);
        List<DividendDetailsViewModel> GetDivedendDetails(string InstrumentId, string FromDate);
        Task<long> AddUpdateDividenMaster(DataTable dataTable);
        List<DividendDetailsViewModel> ExportDivedendDetails();
        List<DividendOrdersViewModel> GetDivedendOrderDetails(int InstrumentId, string FromDate, string ToDate);
    }
}
