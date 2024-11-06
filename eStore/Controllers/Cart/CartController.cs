using AutoMapper;
using BusinessObject.Models;
using eStore.Models;
using eStore.Services;
using eStoreAPI.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace eStore.Controllers.Cart
{
    public class CartController : Controller
    {
        private readonly PRN231_AS1Context _context;
        private readonly HttpClient client = null;
        private readonly CookieService _cookieService;
        private readonly IMapper _mapper;
        public CartController(PRN231_AS1Context context, CookieService cookieService, IMapper mapper)
        {
            _context = context;
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            _cookieService = cookieService;
            _mapper = mapper;
        }
        public IActionResult Index()
        {
            List<KeyValuePair<int, int>> objectList = _cookieService.GetObjectListFromCookie<KeyValuePair<int, int>>("MyObjectList");
            if (objectList == null || objectList.Count == 0)
                return NotFound("No objects found in the cart(cookie).");
            List<CartItem> allItems = new List<CartItem>();
            foreach (KeyValuePair<int, int> item in objectList)
            {
                Product p = _context.Products.FirstOrDefault(x => x.ProductId == item.Key);
                CartItem cartItem = new CartItem
                {
                    ProductName = p.ProductName,
                    Quantity = item.Value,
                    UnitPrice = p.UnitPrice,
                    Discount = 0
                };
                allItems.Add(cartItem);
            }

            ViewData["objectList"] = allItems;
            return View();
        }
        [HttpPost]
        public IActionResult ClearMyObjectListCookie()
        {
            _cookieService.ClearCookie("MyObjectList");
            return RedirectToAction("Index", "Products"); 
        }
        [HttpPost]
        public async Task<IActionResult> CreateOrder(DateTime orderdate, DateTime requireddate, double total)
        {
            string? email = HttpContext.Request.Cookies["Email"];

            Member? m = _context.Members.FirstOrDefault(x => x.Email == email);
            if (m == null)
            {
                return View();
            }
            OrderDTO orderDTO = new OrderDTO
            {
                OrderDate = orderdate,
                RequireDate = requireddate,
                Total = total,
                MemberId = m.MemberId
            };
            BusinessObject.Models.Order o = _mapper.Map<BusinessObject.Models.Order>(orderDTO);

            if (o == null)
            {
                return BadRequest("Order data is null.");
            }
            //var jsonContent = JsonSerializer.Serialize(o);
            //var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            //HttpResponseMessage response = await client.PostAsync("http://localhost:5008/api/OrderAPI", content);
            _context.Orders.Add(o);   //** CAN'T BE DONE WITH 2 API CALL
            _context.SaveChanges();

            List<KeyValuePair<int, int>> objectList = _cookieService.GetObjectListFromCookie<KeyValuePair<int, int>>("MyObjectList");
            if (objectList == null || objectList.Count == 0)
                return NotFound("No objects found in the cookie.");
            List<OrderDetail> ods = new List<OrderDetail>();
            foreach (KeyValuePair<int, int> item in objectList)
            {
                Product? p = _context.Products.FirstOrDefault(x => x.ProductId == item.Key);
                OrderDetail od = new OrderDetail
                {
                    OrderId = o.OrderId,
                    ProductId = item.Key,
                    Quantity = item.Value,
                };
                ods.Add(od);
            }
            var jsonContent2 = JsonSerializer.Serialize(ods);
            var content2 = new StringContent(jsonContent2, Encoding.UTF8, "application/json");
            HttpResponseMessage response2 = await client.PostAsync("http://localhost:5008/api/OrderDetailAPI", content2);

            if (response2.IsSuccessStatusCode)
            {
                _cookieService.ClearCookie("MyObjectList"); 
                return RedirectToAction("Index", "Products"); 
            }
            else
            {
                var errorContent = await response2.Content.ReadAsStringAsync();  

                var errorModel = new ErrorViewModel
                {
                    //RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    //ErrorMessage = errorContent  
                };

                return View("Error", errorModel);
            }

        }
    }
}
