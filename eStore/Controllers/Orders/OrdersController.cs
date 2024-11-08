using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusinessObject.Models;
using System.Net.Http.Headers;
using System.Text.Json;
using eStore.Models;
using Microsoft.Data.SqlClient.Server;

namespace eStore.Controllers.Order
{
    public class OrdersController : Controller
    {
        private readonly PRN231_AS1Context _context;
        private readonly HttpClient client = null;
        private string OrderApiUrl = "";
        public OrdersController(PRN231_AS1Context context)
        {
            _context = context;
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            OrderApiUrl = "http://localhost:5008/api/OrderAPI";
        }


        // GET: Orders
        public async Task<IActionResult> Index(string? sort, DateTime? startdate, DateTime? enddate)
        {
            HttpResponseMessage response;
            if (sort == null)
                response = await client.GetAsync(OrderApiUrl);
            else
            {
                response = await client.GetAsync(OrderApiUrl + "/sortdescending");
            }
            if (startdate != null)
            {
                if (enddate == null)
                {
                    enddate = DateTime.Now;
                }
                string formattedStartDate = startdate.HasValue ? startdate.Value.ToString("yyyy-MM-dd") : "";
                string formattedEndDate = enddate.HasValue ? enddate.Value.ToString("yyyy-MM-dd") : ""; 
                response = await client.GetAsync("http://localhost:5008/api/OrderAPI/getordersbydate?startdate="+formattedStartDate+"&enddate="+formattedEndDate);
            }                                                   
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            List<BusinessObject.Models.Order>? listO = JsonSerializer.Deserialize<List<BusinessObject.Models.Order>>(strData, options);
            return View(listO);
        }
        
        public async Task<IActionResult> GetOrdersByDate(DateTime startDate, DateTime endDate)
        {
            HttpResponseMessage response = await client.GetAsync(OrderApiUrl);//ONGOING XXXXXXXXXXXX
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            List<BusinessObject.Models.Order>? listO = JsonSerializer.Deserialize<List<BusinessObject.Models.Order>>(strData, options);
            return View(listO);
        }


        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Member)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            ViewData["MemberId"] = new SelectList(_context.Members, "MemberId", "Email");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BusinessObject.Models.Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MemberId"] = new SelectList(_context.Members, "MemberId", "Email", order.MemberId);
            return View(order);
        }
        
    }
}
