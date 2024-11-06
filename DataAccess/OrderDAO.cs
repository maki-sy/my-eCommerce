using BusinessObject.Models;

namespace DataAccess
{
    public class OrderDAO
    {
        public static List<Order> GetOrders()
        {
            var listP = new List<Order>();
            try
            {
                using (var context = new PRN231_AS1Context())
                {
                    listP = context.Orders.ToList();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return listP;
        }
        public static Order FindOrderById(int id)
        {
            Order p = new Order();
            try
            {
                using (var context = new PRN231_AS1Context())
                {
                    p = context.Orders.FirstOrDefault(ord => ord.OrderId == id);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return p;
        }
        public static void SaveOrder(Order p)
        {
            try
            {
                using (var context = new PRN231_AS1Context())
                {
                    context.Orders.Add(p);
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public static void UpdateOrder(Order p)
        {
            try
            {
                using (var context = new PRN231_AS1Context())
                {
                    context.Entry<Order>(p).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public static void DeleteOrder(Order p)
        {
            try
            {
                using (var context = new PRN231_AS1Context())
                {
                    context.Orders.SingleOrDefault(ord => ord.OrderId == p.OrderId);
                    context.Orders.Remove(p);
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
