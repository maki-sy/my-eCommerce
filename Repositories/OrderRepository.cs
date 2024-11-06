using BusinessObject.Models;
using DataAccess;

namespace Repositories
{
    public class OrderRepository : IOrderRepository
    {
        public void DeleteOrder(Order p) => OrderDAO.DeleteOrder(p);

        public Order GetOrderById(int id) => OrderDAO.FindOrderById(id);

        public List<Order> GetOrders() => OrderDAO.GetOrders();


        public void SaveOrder(Order p) => OrderDAO.SaveOrder(p);


        public void UpdateOrder(Order p) => OrderDAO.UpdateOrder(p);

    }
}
