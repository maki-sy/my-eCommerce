using AutoMapper;
using BusinessObject.Models;
using eStoreAPI.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        [HttpGet("{id}")]
        public ActionResult<Order> GetOrderDetailById(int id)
        {
            // Retrieve the order by its ID from the database
            List<OrderDetail> ods = _context.OrderDetails.Where(x=>x.OrderId == id).ToList(); 
            if (ods == null)
            {
                return NotFound(); // Return 404 if the order is not found
            }

            return Ok(ods); // Return the found order
        }
        [HttpGet("getfullorder/{id}")]
        public ActionResult<List<FullOrderDTO>> GetFullOrder(int id)
        {
            // Retrieve the order by its ID from the database
            List<OrderDetail> ods = _context.OrderDetails.Where(x => x.OrderId == id).ToList();

            if (ods == null)
            {
                return NotFound(); // Return 404 if the order is not found
            }
            List<FullOrderDTO> imgQuantityPairs = new List<FullOrderDTO>();
            foreach (var product in ods) 
            {
                FullOrderDTO imgQuantityPair = new FullOrderDTO();
                imgQuantityPair.ImageData = _context.Products.Find(product.ProductId).ImageData;
                imgQuantityPair.Quantity = product.Quantity;
                imgQuantityPair.ProductName = _context.Products.Find(product.ProductId).ProductName;
                imgQuantityPairs.Add(imgQuantityPair);
            }
            return Ok(imgQuantityPairs); // Return the found order
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
