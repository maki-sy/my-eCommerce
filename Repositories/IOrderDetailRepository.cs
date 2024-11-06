using BusinessObject.Models;

namespace Repositories
{
    public interface IOrderDetailRepository
    {
        void SaveOrderDetail(OrderDetail p);
        OrderDetail GetOrderDetailById(int id);
        void DeleteOrderDetail(OrderDetail p);
        void UpdateOrderDetail(OrderDetail p);
        List<OrderDetail> GetOrderDetails();
    }
}
