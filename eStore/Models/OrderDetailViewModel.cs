using BusinessObject.Models;

namespace eStore.Models
{
    public class OrderDetailViewModel
    {
        Order Order { get; set; }
        OrderDetail OrderDetail { get; set; }
        Product Product { get; set; }
    }
}
