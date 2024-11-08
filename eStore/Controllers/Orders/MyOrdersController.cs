using BusinessObject.Models;
using eStore.Models;
using eStoreAPI.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;
using System.Text.Json;
using static System.Net.WebRequestMethods;

namespace eStore.Controllers.Orders
{
    public class MyOrdersController : Controller
    {
        private readonly PRN231_AS1Context _context;
        private readonly HttpClient client = null;
        private string OrderApiUrl = "";
        public MyOrdersController(PRN231_AS1Context context)
        {
            _context = context;
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            OrderApiUrl = "http://localhost:5008/api/OrderAPI";
        }
        public async Task<IActionResult> Cancel(int id)
        {
            HttpResponseMessage response = await client.PostAsync(OrderApiUrl + "/cancel/" + id, null);

            if (response.IsSuccessStatusCode)
            {
                // If success, redirect or return success
                return RedirectToAction(nameof(Index));
            }
            else
            {
                // Handle failure by passing the response to the error view
                var errorMessage = await response.Content.ReadAsStringAsync();
                return View("Error", new ErrorViewModel { StatusCode = response.StatusCode, Message = errorMessage });
            }
        }
        public async Task<IActionResult> Index()
        {
            var roleCookie = Request.Cookies["Email"];
            int id = _context.Members.FirstOrDefault(x => x.Email == roleCookie).MemberId;

            HttpResponseMessage response = await client.GetAsync(OrderApiUrl + "/getmyorders?id=" + id);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            List<BusinessObject.Models.Order>? listO = JsonSerializer.Deserialize<List<BusinessObject.Models.Order>>(strData, options);
            return View("Index", listO);
        }
        public async Task<IActionResult> Details(int id)
        {
            var roleCookie = Request.Cookies["Email"];
            string url = "http://localhost:5008/api/OrderDetailAPI/getfullorder/" + id.ToString();
            HttpResponseMessage response = await client.GetAsync(url);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            List<FullOrderDTO> o = JsonSerializer.Deserialize<List<FullOrderDTO>>(strData, options);
            ViewData["fullorder"] = o;
            return View(o);  //loi ngu vcl
        }
    }
}
