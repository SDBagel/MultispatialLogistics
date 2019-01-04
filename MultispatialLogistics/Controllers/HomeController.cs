using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MultispatialLogistics.Models;

namespace MultispatialLogistics.Controllers
{
    public class HomeController : Controller
    {
        private string token;
        private string CharacterId;

        public IActionResult Index()
        {
            token = Request.Cookies["accessToken"];
            if (token == null)
            {
                Response.Redirect("../login");
            }

            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Callback()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
