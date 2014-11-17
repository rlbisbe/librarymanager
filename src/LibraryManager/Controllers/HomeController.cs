using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Mvc;
using System.Security.Claims;
using Microsoft.AspNet.Security.Cookies;

namespace LibraryManager.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Login(string name)
        {
            if (!String.IsNullOrWhiteSpace(name))
            {
                var claims = new[]
            {
                new Claim(ClaimTypes.Name, name)
            };
                var identity = new ClaimsIdentity(claims,
                    CookieAuthenticationDefaults.AuthenticationType);
                Context.Response.SignIn(identity);

                return Redirect("~/");
            }

            return View();
        }

        public IActionResult Error()
        {
            return View("~/Views/Shared/Error.cshtml");
        }
    }
}