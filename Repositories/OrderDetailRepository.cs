using BusinessObject.Models;
using DataAccess;

namespace Repositories
{
    public class OrderDetailRepository : IOrderDetailRepository
    {
        public void DeleteOrderDetail(OrderDetail p) => OrderDetailDAO.DeleteOrderDetail(p);

        public OrderDetail GetOrderDetailById(int id) => OrderDetailDAO.FindOrderDetailById(id);

        public List<OrderDetail> GetOrderDetails() => OrderDetailDAO.GetOrderDetails();


        public void SaveOrderDetail(OrderDetail p) => OrderDetailDAO.SaveOrderDetail(p);


        public void UpdateOrderDetail(OrderDetail p) => OrderDetailDAO.UpdateOrderDetail(p);

    }
}
