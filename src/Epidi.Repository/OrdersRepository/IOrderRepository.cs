using Epidi.Models.Model;
using Epidi.Models.Paging;
using Epidi.Models.ViewModel.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Repository.OrdersRepository
{
    public interface IOrderRepository
    {
        Tuple<List<Order>, long> GetOrders(PageParam pageParam, int MasterInstrumentId, DateTime? OpenTiming, DateTime? CloseTiming, int UserId, int CountryId, decimal Price, decimal SpecificPrice, string search1);
        ResponseViewModel removeOrders(int ID);
        ResponseViewModel updateFields(string fieldName, string fieldValue, int ID);
        Task<List<Order>> GetAllOrders();
        Task<long> ImportOrderData_UpsertByDt(DataTable dtOrderData);
        ResponseViewModel updateGridFields(int OrderId, double Closeprice, double OpenUnits, double OpenPrice);
    }
}
