using BusinessObject.Models;

namespace Repositories
{
    public interface IOrderRepository
    {
        void SaveOrder(Order o);
        Order GetOrderById(int id);
        void DeleteOrder(Order o);
        void UpdateOrder(Order o);
        List<Order> GetOrders();
    }
}