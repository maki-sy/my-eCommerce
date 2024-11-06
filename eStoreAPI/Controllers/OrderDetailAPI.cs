using AutoMapper;
using BusinessObject.Models;
using eStoreAPI.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NuGet.Packaging.Signing;

namespace eStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailAPI : ControllerBase
    {
        private readonly PRN231_AS1Context _context;
        private readonly IMapper _mapper;

        public OrderDetailAPI(PRN231_AS1Context context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/OrderAPI
        [HttpGet]
        public ActionResult<IEnumerable<OrderDetail>> GetOrderDetails()
        {
            if (_context.Orders == null)
            {
                return NotFound();
            }
            return _context.OrderDetails.ToList();
        }
        [HttpPost]
        public IActionResult PostOrderDetail([FromBody] List<OrderDetailDTO> odDTOs)
        {
            if (odDTOs == null || !odDTOs.Any())
            {
                return BadRequest("No order details to send.");
            }
            foreach (var item in odDTOs) 
            {
                OrderDetail od = _mapper.Map<OrderDetail>(item);
                _context.OrderDetails.Add(od);
                Product p = _context.Products.FirstOrDefault(p => p.ProductId == item.ProductId);
                p.UnitsInStock = p.UnitsInStock - item.Quantity;
                _context.Products.Update(p);
            }
            _context.SaveChanges();
            return NoContent();

        }
    }
}
