using BusinessObject.Models;
using eStore.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace eStore.Controllers.MyProfile
{
    public class MyProfileController : Controller
    {
        private readonly PRN231_AS1Context _context;
        private readonly HttpClient client = null;
        private string MemberApiUrl = "";


        public MyProfileController(PRN231_AS1Context context)
        {
            _context = context;
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(contentType);
            MemberApiUrl = "http://localhost:5008/api/MemberAPI";

        }
        public async Task<IActionResult> Index()
        {
            var roleCookie = Request.Cookies["Email"];
            int id = _context.Members.FirstOrDefault(x=>x.Email==roleCookie).MemberId;
            HttpResponseMessage response = await client.GetAsync(MemberApiUrl+"/getbyid?id="+id);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            Member member = JsonSerializer.Deserialize<Member>(strData, options);
            return View(member);
        }
        public async Task<IActionResult> Edit()
        {
            var roleCookie = Request.Cookies["Email"];
            if (roleCookie == null || _context.Members == null)
            {
                return NotFound();
            }
            int id = _context.Members.FirstOrDefault(x => x.Email == roleCookie).MemberId;
            HttpResponseMessage response = await client.GetAsync(MemberApiUrl + "/getbyid?id=" + id);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            Member member = JsonSerializer.Deserialize<Member>(strData, options);
            return View(member);
        }

        // POST: Members/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Member member)
        {
            if (member == null)
            {
                return BadRequest("Member data is null.");
            }
            var roleCookie = Request.Cookies["Email"];
            int id = _context.Members.FirstOrDefault(x => x.Email == roleCookie).MemberId;

            var jsonContent = JsonSerializer.Serialize(member);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PutAsync(MemberApiUrl + "/" + id, content);
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
    }
}
