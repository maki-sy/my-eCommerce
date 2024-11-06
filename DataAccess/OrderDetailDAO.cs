using BusinessObject.Models;

namespace DataAccess
{
    public class OrderDetailDAO
    {
        public static List<OrderDetail> GetOrderDetails()
        {
            var listP = new List<OrderDetail>();
            try
            {
                using (var context = new PRN231_AS1Context())
                {
                    listP = context.OrderDetails.ToList();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return listP;
        }
        public static OrderDetail FindOrderDetailById(int id)
        {
            OrderDetail p = new OrderDetail();
            try
            {
                using (var context = new PRN231_AS1Context())
                {
                    p = context.OrderDetails.FirstOrDefault(pro => pro.OrderId == id);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return p;
        }
        public static void SaveOrderDetail(OrderDetail p)
        {
            try
            {
                using (var context = new PRN231_AS1Context())
                {
                    context.OrderDetails.Add(p);
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public static void UpdateOrderDetail(OrderDetail p)
        {
            try
            {
                using (var context = new PRN231_AS1Context())
                {
                    context.Entry<OrderDetail>(p).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public static void DeleteOrderDetail(OrderDetail p)
        {
            try
            {
                using (var context = new PRN231_AS1Context())
                {
                    context.OrderDetails.SingleOrDefault(pro => pro.OrderId == p.OrderId);
                    context.OrderDetails.Remove(p);
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }


    }
}
