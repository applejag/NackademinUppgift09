using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nauktion.Helpers;
using Nauktion.Models;

namespace Nauktion.Controllers
{
    [AuthorizeRole(NauktionRoles.Admin)]
    public class AdminController : Controller
    {
        private readonly UserManager<NauktionUser> _userManager;

        public AdminController(UserManager<NauktionUser> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Users()
        {
            List<NauktionUser> users = await _userManager.Users.ToListAsync();

            return View(users);
        }

        public async Task<IActionResult> ElevateUser(string id)
        {
            NauktionUser user = await _userManager.FindByIdAsync(id);

            if (user is null)
            {
                TempData["Message"] = "Misslyckades med att befodra användare! Användaren finns inte.";
                return RedirectToAction("Users");
            }

            if (await _userManager.IsInRoleAsync(user, NauktionRoles.Admin))
            {
                TempData["Message"] = $"Misslyckades med att befodra användare \"{user.UserName}\"! Användaren är redan Admin.";
                return RedirectToAction("Users");
            }

            IdentityResult result = await _userManager.AddToRoleAsync(user, NauktionRoles.Admin);
            if (!result.Succeeded)
            {
                TempData["Message"] = $"Misslyckades med att befodra användaren \"{user.UserName}\"! Okänt fel: \"{string.Join("\", \"", result.Errors.Select(e => e.Description))}\"";
                return RedirectToAction("Users");
            }

            TempData["Message"] = $"Användare \"{user.UserName}\" har blivit befodrad till Admin!";
            return RedirectToAction("Users");
        }
    }
}