using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusinessObject.Models;
using eStoreAPI.DTO;
using AutoMapper;

namespace eStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderAPI : ControllerBase
    {
        private readonly PRN231_AS1Context _context;
        private readonly IMapper _mapper;
        public OrderAPI(PRN231_AS1Context context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/OrderAPI
        [HttpGet]
        public ActionResult<IEnumerable<Order>> GetOrders()
        {
          if (_context.Orders == null)
          {
              return NotFound();
          }
            return _context.Orders.ToList();
        }
        [HttpGet("shipped")]
        public ActionResult<IEnumerable<Order>> GetShippedOrders()
        {
            if (_context.Orders == null)
            {
                return NotFound();
            }
            return _context.Orders.Where(s=>s.Status=="Shipped").ToList();
        }
        [HttpGet("cancelled")]
        public ActionResult<IEnumerable<Order>> GetCancelledOrders()
        {
            if (_context.Orders == null)
            {
                return NotFound();
            }
            return _context.Orders.Where(s => s.Status == "Cancelled").ToList();
        }
        [HttpGet("shipping")]
        public ActionResult<IEnumerable<Order>> GetShippingOrders()
        {
            if (_context.Orders == null)
            {
                return NotFound();
            }
            return _context.Orders.Where(s => s.Status == "Shipping").ToList();
        }

        [HttpGet("getmyorders")]
        public ActionResult<IEnumerable<Order>> GetMyOrders(int id)
        {
            if (_context.Orders == null)
            {
                return NotFound();
            }
            return _context.Orders.Where(x=>x.MemberId==id).ToList();
        }
        [HttpGet("getordersbydate")]
        public ActionResult<IEnumerable<Order>> GetOrdersByDate(DateTime startdate, DateTime? enddate)
        {
            if (_context.Orders == null)
            {
                return NotFound();
            }
            if (enddate.Equals(null))
                enddate = DateTime.Now;
            return _context.Orders.Where(x => x.OrderDate>=startdate&&x.OrderDate<=enddate).ToList();
        }
        [HttpGet("sortdescending")]
        public ActionResult<IEnumerable<Order>> GetSortedOrders()
        {
            if (_context.Orders == null)
            {
                return NotFound();
            }
            return _context.Orders.OrderByDescending(x=>x.Total).ToList();
        }
        [HttpGet("{id}")]
        public ActionResult<Order> GetOrderById(int id)
        {
            // Retrieve the order by its ID from the database
            Order order = _context.Orders.Find(id);

            if (order == null)
            {
                return NotFound(); // Return 404 if the order is not found
            }

            return Ok(order); // Return the found order
        }
        [HttpPost]
        public IActionResult PostOrder(OrderDTO oDTO)
        {
            using (var context = new PRN231_AS1Context())
            {
                Order o = _mapper.Map<Order>(oDTO);
                o.Status = "Shipping";
               _context.Orders.Add(o);
                _context.SaveChanges();
                return CreatedAtAction(nameof(GetOrderById), new { id = o.OrderId }, o);
            }
        }
        [HttpPost("cancel/{id}")]
        public IActionResult CancelOrder(int id)
        {
            using (var context = new PRN231_AS1Context())
            {
                var order = context.Orders.Find(id);
                if (order == null)
                {
                    return NotFound($"Order with ID {id} not found.");
                }

                order.Status = "Cancelled";
                context.Orders.Update(order);
                context.SaveChanges();

                return Ok(new { Message = "Order cancelled successfully", OrderId = order.OrderId });
            }
        }
        [HttpPost("done/{id}")]
        public IActionResult DoneOrder(int id)
        {
            using (var context = new PRN231_AS1Context())
            {
                var order = context.Orders.Find(id);
                if (order == null)
                {
                    return NotFound($"Order with ID {id} not found.");
                }

                order.Status = "Shipped";
                context.Orders.Update(order);
                context.SaveChanges();

                return Ok(new { Message = "Order cancelled successfully", OrderId = order.OrderId });
            }
        }
        // PUT: api/OrderAPI/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public IActionResult PutOrder(int id, OrderDTO oDTO)
        {
            if (oDTO == null)
            {
                return BadRequest("order data is null.");
            }
            Order o = _mapper.Map<Order>(oDTO);
            o.OrderId = id;
            _context.Orders.Update(o);
            _context.SaveChanges();
            return NoContent();
        }

        // DELETE: api/OrderAPI/5
        [HttpDelete]
        public IActionResult DeleteOrder(int id)
        {
            Order? o = _context.Orders.Find(id);
            if (o == null)
            {
                return BadRequest("No order was found");
            }
            List<OrderDetail> orderDetails = _context.OrderDetails.Where(o => o.OrderId == id).ToList();
            _context.OrderDetails.RemoveRange(orderDetails);
            _context.Orders.Remove(o);
            _context.SaveChanges();
            return NoContent();
        }

        private bool OrderExists(int id)
        {
            return (_context.Orders?.Any(e => e.OrderId == id)).GetValueOrDefault();
        }
    }
}
