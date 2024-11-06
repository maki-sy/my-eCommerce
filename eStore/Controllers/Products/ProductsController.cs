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
using System.Text;
using eStore.Models;
using eStoreAPI.DTO;
using eStore.Services;
using System.Net.Http;

namespace eStore.Controllers.Products
{
    public class ProductsController : Controller
    {
        private readonly PRN231_AS1Context _context;
        private readonly HttpClient client = null;
        private string ProductApiUrl = "";
        private readonly CookieService _cookieService;

        public ProductsController(PRN231_AS1Context context, CookieService cookieService)
        {
            _context = context;
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            ProductApiUrl = "http://localhost:5008/api/ProductAPI";
            _cookieService = cookieService;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            HttpResponseMessage response = await client.GetAsync(ProductApiUrl);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            List<Product>? listP = JsonSerializer.Deserialize<List<Product>>(strData, options);
            ViewData["list"] = listP;
            return View();
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName"); return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] ProductDTO pDTO)
        {
            if (pDTO == null)
            {
                return BadRequest("Product data is null.");
            }

            // Handle file separately, as it is part of multipart data
            if (pDTO.imageFile != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await pDTO.imageFile.CopyToAsync(memoryStream);
                    // Assuming you want to save image as byte array in database or process it further
                    // Here you can assign it to the Product object or process it
                }
            }

            // Create a multipart form content
            var formData = new MultipartFormDataContent();

            // Add other fields (except image) to the form data
            formData.Add(new StringContent(pDTO.ProductName), "ProductName");
            formData.Add(new StringContent(pDTO.UnitPrice.ToString()), "UnitPrice");
            formData.Add(new StringContent(pDTO.UnitsInStock.ToString()), "UnitsInStock");
            formData.Add(new StringContent(pDTO.CategoryId.ToString()), "CategoryId");

            // Add image file to the form data (if exists)
            if (pDTO.imageFile != null)
            {
                var fileContent = new StreamContent(pDTO.imageFile.OpenReadStream());
                fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(pDTO.imageFile.ContentType);
                formData.Add(fileContent, "imageFile", pDTO.imageFile.FileName);
            }

            // Make the request to the API
            HttpResponseMessage response = await client.PostAsync(ProductApiUrl, formData);

            if (response.IsSuccessStatusCode)
            {
                // If success, redirect or return success
                return RedirectToAction("Index");
            }
            else
            {
                // Handle failure by passing the response to the error view
                var errorMessage = await response.Content.ReadAsStringAsync();
                return View("Error", new ErrorViewModel { StatusCode = response.StatusCode, Message = errorMessage });
            }
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId", product.CategoryId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product)
        {
            if (product == null)
            {
                return BadRequest("Product data is null.");
            }
            var jsonContent = JsonSerializer.Serialize(product);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PutAsync(ProductApiUrl + "/" + id, content);
            if (response.IsSuccessStatusCode)
            {
                // If success, redirect or return success
                return RedirectToAction("Index");
            }
            else
            {
                // Handle failure, for example by showing an error view
                ErrorViewModel e = new ErrorViewModel();
                return View("Error", e);
            }
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            string apiUrl = $"{ProductApiUrl}?id={id}";

            // Create an instance of HttpClient
            using (var httpClient = new HttpClient())
            {
                // Send the DELETE request to the API
                HttpResponseMessage response = await httpClient.DeleteAsync(apiUrl);

                // Check if the response was successful
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index"); // Return 204 No Content on success
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return NotFound(); // Return 404 if the Product was not found in the API
                }
                else
                {
                    return StatusCode((int)response.StatusCode, response.ReasonPhrase); // Return the status from the API
                }
            }
        }
        [HttpPost]
        public async Task<IActionResult> AddToCookie(int id, int quantity)
        {
            KeyValuePair<int, int> cartItems = new KeyValuePair<int, int>(id, quantity);

            _cookieService.AddObjectToListInCookie("MyObjectList", cartItems, 30); // store the list for 30 minutes
            return RedirectToAction("Index","Products");
        }

        // Retrieving all objects from the cookie
        [HttpGet]
        public IActionResult GetCookieList()
        {
            List<string> objectList = _cookieService.GetObjectListFromCookie<string>("MyObjectList");
            if (objectList == null || objectList.Count == 0)
                return NotFound("No objects found in the cookie.");
            return Ok(objectList);
        }
        [HttpGet("searchbyname")]
        public async Task<IActionResult> SearchByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest("Product name cannot be empty.");
            }

            var response = await client.GetAsync($"http://localhost:5008/api/ProductAPI/searchbyname?name={name.Trim()}");

            if (response.IsSuccessStatusCode)
            {
                var productList = await response.Content.ReadFromJsonAsync<List<Product>>();
                ViewData["list"] = productList;
                return View("Index");
            }
            else
            {
                // Handle error response
                var errorMessage = await response.Content.ReadAsStringAsync();
                return View("Error", new ErrorViewModel { StatusCode = response.StatusCode, Message = errorMessage });
            }
        }
        [HttpGet("searchbyprice")]
        public async Task<IActionResult> SearchByPrice(double price)
        {
            if (price < 0)
            {
                return BadRequest("Price cannot be negative.");
            }

            var response = await client.GetAsync($"http://localhost:5008/api/ProductAPI/searchbyprice?price={price}");

            if (response.IsSuccessStatusCode)
            {
                var productList = await response.Content.ReadFromJsonAsync<List<Product>>();
                ViewData["list"] = productList;
                return View("Index"); // Replace "ProductList" with your actual view name
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                return View("Error", new ErrorViewModel { StatusCode = response.StatusCode, Message = errorMessage });
            }
        }

    }
}
