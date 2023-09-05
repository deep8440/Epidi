using Epidi.Models.Model;
using Epidi.Models.Paging;
using Epidi.Models.ViewModel.Common;
using Epidi.Repository.IBRepository;
using Epidi.Repository.OrdersRepository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Service.OrdersService
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        public Tuple<List<Order>, long> GetOrders(PageParam pageParam, int MasterInstrumentId, DateTime? OpenTiming, DateTime? CloseTiming, int UserId, int CountryId, decimal Price, decimal SpecificPrice, string search1)
        {
            return _orderRepository.GetOrders(pageParam,MasterInstrumentId,OpenTiming,CloseTiming,UserId,CountryId,Price,SpecificPrice, search1);
        }
        public async Task<List<Order>> GetAllOrders()
        {
            return await _orderRepository.GetAllOrders();
        }
        public ResponseViewModel removeOrders(int ID)
        {
            return _orderRepository.removeOrders(ID);
        }
        public ResponseViewModel updateFields(string fieldName, string fieldValue, int ID)
        {
            return _orderRepository.updateFields(fieldName,fieldValue, ID);
        }
        public async Task<long> ImportOrderData_UpsertByDt(DataTable dtOrderData)
        {
            return await _orderRepository.ImportOrderData_UpsertByDt(dtOrderData);
        }
        public ResponseViewModel updateGridFields(int OrderId, double Closeprice, double OpenUnits, double OpenPrice)
        {
            return _orderRepository.updateGridFields(OrderId, Closeprice, OpenUnits, OpenPrice);
        }
    }
}
