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
using Repositories;
using Newtonsoft.Json;
using eStoreAPI.Services;

namespace eStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductAPI : ControllerBase
    {
        private IProductRepository repo = new ProductRepository();
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly CookieService _cookieService;
        public ProductAPI(IMapper mapper, IHttpContextAccessor httpContextAccessor, CookieService cookieService)
        {
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _cookieService = cookieService;
        }
        // GET: api/ProductAPI
        [HttpGet]
        public IActionResult GetProducts()
        {
            using (var _context = new PRN231_AS1Context())
            {
                if (_context.Products == null)
                {
                    return NotFound();
                }
                return Ok(_context.Products.ToList());
            }
        }
        [HttpGet("searchbyname")]
        public IActionResult SearchByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest("Product name cannot be empty.");
            }

            using (var context = new PRN231_AS1Context())
            {
                var plist = context.Products
                    .Where(x => x.ProductName.Contains(name.Trim()))
                    .ToList();

                if (plist == null || plist.Count == 0)
                {
                    return NotFound("No products found.");
                }

                return Ok(plist); 
            }
        }
        [HttpGet("searchbyprice")]
        public IActionResult SearchByPrice(double price)
        {
            if (price < 0)
            {
                return BadRequest("Price cannot be negative.");
            }

            using (var context = new PRN231_AS1Context())
            {
                var plist = context.Products
                    .Where(x => x.UnitPrice <= price)
                    .ToList();

                if (plist == null || plist.Count == 0)
                {
                    return NotFound("No products found.");
                }

                return Ok(plist);
            }
        }
        [HttpPost]
        public async Task<IActionResult> PostProduct([FromForm] ProductDTO pDTO)
        {
            using (var context = new PRN231_AS1Context())
            {
                if (pDTO == null || context.Products.Any(x => x.ProductName.Equals(pDTO.ProductName)))
                {
                    return BadRequest("Product data is null or already exists.");
                }
                Product p = _mapper.Map<Product>(pDTO);

                if (pDTO.imageFile != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await pDTO.imageFile.CopyToAsync(memoryStream);
                        p.ImageData = memoryStream.ToArray();
                    }
                }
                Console.WriteLine(p.ProductName+"  "+ p.UnitsInStock + "  " + p.UnitPrice + "  " + p.ImageData);
                context.Products.Add(p);
                context.SaveChanges();
                return Ok("Product has been created");
            }
        }
        [HttpPut("{id}")]
        public IActionResult UpdateProduct(int id, ProductDTO pDTO)
        {
            if (pDTO == null)
            {
                return BadRequest("Product data is null.");
            }
            Product p = _mapper.Map<Product>(pDTO);
            p.ProductId = id;
            repo.UpdateProduct(p);
            return NoContent();
        }

        [HttpDelete]
        public IActionResult DeleteProduct(int id)
        {
            Product p = new Product { ProductId = id };
            repo.DeleteProduct(p);
            return NoContent();
        }
        [HttpPost("add-to-cookie")]
        public IActionResult AddToCookie([FromBody] string item)
        {
            _cookieService.AddObjectToListInCookie("MyObjectList", item, 30); // store the list for 30 minutes
            return Ok("Object added to the cookie list.");
        }

        // Retrieving all objects from the cookie
        [HttpGet("get-cookie-list")]
        public IActionResult GetCookieList()
        {
            var objectList = _cookieService.GetObjectListFromCookie<string>("MyObjectList");
            if (objectList == null || objectList.Count == 0)
                return NotFound("No objects found in the cookie.");

            return Ok(objectList);
        }
    }
}
