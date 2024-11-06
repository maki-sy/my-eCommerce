using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusinessObject.Models;
using System.Text.Json;
using System.Net.Http.Headers;
using System.Text;
using eStore.Models;

namespace eStore.Controllers.Members
{
    public class MembersController : Controller
    {
        private readonly PRN231_AS1Context _context;
        private readonly HttpClient client = null;
        private string MemberApiUrl = "";
        public MembersController()
        {
            _context = new PRN231_AS1Context();
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            MemberApiUrl = "http://localhost:5008/api/MemberAPI";
        }

        // GET: Members
        public async Task<IActionResult> Index()
        {
            HttpResponseMessage response = await client.GetAsync(MemberApiUrl);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            List<Member>? listP = JsonSerializer.Deserialize<List<Member>>(strData, options);
            return View(listP);
        }

        // GET: Members/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            HttpResponseMessage response = await client.GetAsync(MemberApiUrl + "/getbyid?id=" + id);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            Member member = JsonSerializer.Deserialize<Member>(strData, options);
            return View(member);
        }

        // GET: Members/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Members/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Member p)
        {
            if (p == null)
            {
                return BadRequest("Member data is null.");
            }
            var jsonContent = JsonSerializer.Serialize(p);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(MemberApiUrl, content);
            if (response.IsSuccessStatusCode)
            {
                // If success, redirect or return success
                return RedirectToAction("Index");
            }
            else
            {
                // Handle failure, for example by showing an error view
                return View("Error", response);
            }
        }

        // GET: Members/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Members == null)
            {
                return NotFound();
            }

            var member =  await _context.Members.FindAsync(id);
            if (member == null)
            {
                return NotFound();
            }
            return View(member);
        }

        // POST: Members/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Member member)
        {
            if (member == null)
            {
                return BadRequest("Member data is null.");
            }
            var jsonContent = JsonSerializer.Serialize(member);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PutAsync(MemberApiUrl+"/"+id, content);
            if (response.IsSuccessStatusCode)
            {
                // If success, redirect or return success
                return RedirectToAction("Index");
            }
            else
            {
                // Handle failure, for example by showing an error view
                ErrorViewModel e = new ErrorViewModel();
                return View("Error", e );
            }
        }

        // GET: Members/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Members == null)
            {
                return NotFound();
            }

            var member = await _context.Members
                .FirstOrDefaultAsync(m => m.MemberId == id);
            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        // POST: Members/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            string apiUrl = $"{MemberApiUrl}?id={id}";

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
                    return NotFound(); // Return 404 if the member was not found in the API
                }
                else
                {
                    return StatusCode((int)response.StatusCode, response.ReasonPhrase); // Return the status from the API
                }
            }
        }

        private bool MemberExists(int id)
        {
            return (_context.Members?.Any(e => e.MemberId == id)).GetValueOrDefault();
        }
    }
}
