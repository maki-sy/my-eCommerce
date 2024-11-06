using BusinessObject.Models;

namespace DataAccess
{
    public class ProductDAO
    {
        public static List<Product> GetProducts()
        {
            var listP = new List<Product>();
            try
            {
                using (var context = new PRN231_AS1Context())
                {
                    listP = context.Products.ToList();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return listP;
        }
        public static Product FindProductById(int id)
        {
            Product p = new Product();
            try
            {
                using (var context = new PRN231_AS1Context())
                {
                    p = context.Products.FirstOrDefault(pro => pro.ProductId == id);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return p;
        }
        public static void SaveProduct(Product p)
        {
            try
            {
                using (var context = new PRN231_AS1Context())
                {
                    context.Products.Add(p);
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public static void UpdateProduct(Product p)
        {
            try
            {
                using (var context = new PRN231_AS1Context())
                {
                    context.Entry<Product>(p).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public static void DeleteProduct(Product p)
        {
            try
            {
                using (var context = new PRN231_AS1Context())
                {
                    var p1 = context.Products.SingleOrDefault(pro => pro.ProductId == p.ProductId);
                    context.Products.Remove(p1);
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
