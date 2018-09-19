using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nauktion.Helpers;
using Nauktion.Models;

namespace Nauktion.Controllers
{
    [AuthorizeRole(NauktionRoles.Admin)]
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Users()
        {
            return View();
        }

        public IActionResult ElevateUser(int id)
        {
            TempData["Message"] = $"User \"{1}\" has been promoted to Admin.";
            return RedirectToAction("Users");
        }
    }
}