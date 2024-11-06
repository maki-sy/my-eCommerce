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

namespace eStore.Controllers.Auth
{
    public class AuthController : Controller
    {
        private readonly PRN231_AS1Context _context;
        private IConfiguration _configuration;
        private readonly CookieService _cookieService;
        public AuthController(PRN231_AS1Context context, IConfiguration configuration, CookieService cookieService)
        {
            _context = context;
            _configuration = configuration;
            _cookieService = cookieService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginDTO credentials)
        {
            string adminEmail = _configuration["AdminAccount:Email"];
            string adminPassword = _configuration["AdminAccount:Password"];

            if (credentials.Email == adminEmail && credentials.Password == adminPassword)
            {
                Response.Cookies.Append("Email", adminEmail, new CookieOptions
                {
                    HttpOnly = true, // prevent access to cookie via JS
                    Secure = true, // ensure cookie only sent over HTTPS
                    SameSite = SameSiteMode.Strict, // CSRF protection
                    Expires = DateTimeOffset.Now.AddMinutes(60)
                });

                Response.Cookies.Append("Role", "ADMIN", new CookieOptions
                {
                    HttpOnly = true, // prevent access to cookie via JS
                    Secure = true, // ensure cookie only sent over HTTPS
                    SameSite = SameSiteMode.Strict, // CSRF protection
                    Expires = DateTimeOffset.Now.AddMinutes(60)
                });

                return RedirectToAction("Index","Members");
            }

            Member? m = _context.Members.Where(x => x.Email == credentials.Email && x.Password == credentials.Password).FirstOrDefault();
            if (m == null)
            {
                ViewData["loi"] = "mksai";
                return View();
            }

            Response.Cookies.Append("Email", credentials.Email, new CookieOptions
            {
                HttpOnly = true, // prevent access to cookie via JS
                Secure = true, // ensure cookie only sent over HTTPS
                SameSite = SameSiteMode.Strict, // CSRF protection
                Expires = DateTimeOffset.Now.AddMinutes(60)
            });

            Response.Cookies.Append("Role", "NORMAL", new CookieOptions
            {
                HttpOnly = true, // prevent access to cookie via JS
                Secure = true, // ensure cookie only sent over HTTPS
                SameSite = SameSiteMode.Strict, // CSRF protection
                Expires = DateTimeOffset.Now.AddMinutes(60)
            });

            return RedirectToAction("Index","Products");

        }
        [HttpGet]
        public IActionResult Logout()
        {
            _cookieService.ClearCookie("Email");
            _cookieService.ClearCookie("Role");
            _cookieService.ClearCookie("MyObjectList");

            return RedirectToAction("Login", "Auth");
        }

    }
}