using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Nauktion.Helpers;
using Nauktion.Models;

namespace Nauktion.Controllers
{
    public class HomeController : Controller
    {
        private readonly SignInManager<NauktionUser> signInManager;

        public HomeController(SignInManager<NauktionUser> signInManager)
        {
            this.signInManager = signInManager;
        }

        public IActionResult Index()
        {
            if (signInManager.IsSignedIn(User))
                return RedirectToAction("Index", "Auktion");

            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult Privacy()
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
