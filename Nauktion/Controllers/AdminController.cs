using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nauktion.Helpers;
using Nauktion.Models;
using Nauktion.Services;

namespace Nauktion.Controllers
{
    [AuthorizeRole(NauktionRoles.Admin)]
    public class AdminController : Controller
    {
        private readonly UserManager<NauktionUser> _userManager;
        private readonly IAuktionService _service;

        public AdminController(UserManager<NauktionUser> userManager, IAuktionService service)
        {
            _userManager = userManager;
            _service = service;
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


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(AuktionViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            NauktionUser currentUser = await _userManager.GetUserAsync(User);
            await _service.CreateAuktionAsync(model, currentUser);

            TempData["Message"] = $"Din auktion \"{model.Titel}\" har skapats!";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Alter(int id)
        {
            AuktionModel auktionModel = await _service.GetAuktionAsync(id);
            if (auktionModel is null)
            {
                TempData["Message"] = "Misslyckades med att redigera auktionen! Auktionen finns inte i databasen.";
                return RedirectToAction("Index");
            }

            NauktionUser currentUser = await _userManager.GetUserAsync(User);

            if (auktionModel.SkapadAv != currentUser.Id)
            {
                TempData["Message"] = "Misslyckades med att redigera auktionen! Du kan inte redigera någon annans auktion.";
                return RedirectToAction("Index");
            }

            var model = new AuktionViewModel
            {
                AuktionID = auktionModel.AuktionID,
                Titel = auktionModel.Titel,
                Beskrivning = auktionModel.Beskrivning,
                SlutDatum = auktionModel.SlutDatum,
                Utropspris = auktionModel.Utropspris ?? 0
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Alter(AuktionViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            AuktionModel auktionModel = await _service.GetAuktionAsync(model.AuktionID);
            if (auktionModel is null)
            {
                TempData["Message"] = "Misslyckades med att redigera auktionen! Auktionen finns inte i databasen.";
                return RedirectToAction("Index");
            }

            NauktionUser currentUser = await _userManager.GetUserAsync(User);
            if (auktionModel.SkapadAv != currentUser.Id)
            {
                TempData["Message"] = "Misslyckades med att redigera auktionen! Du kan inte redigera någon annans auktion.";
                return RedirectToAction("Index");
            }

            await _service.AlterAuktionAsync(model);

            TempData["Message"] = $"Dina ändringar till auktionen \"{model.Titel}\" har sparats!";
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(AuktionDeleteViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Alter", new { id = model.AuktionID });
            }

            AuktionBudViewModel auktionModel = await _service.GetAuktionBudsAsync(model.AuktionID);
            if (auktionModel is null)
            {
                TempData["Message"] = "Misslyckades med att ta bort auktionen! Auktionen finns inte i databasen.";
                return RedirectToAction("Index");
            }

            NauktionUser currentUser = await _userManager.GetUserAsync(User);

            if (auktionModel.SkapadAv != currentUser.Id)
            {
                TempData["Message"] = "Misslyckades med att ta bort auktionen! Du kan inte ta bort någon annans auktion.";
                return RedirectToAction("Index");
            }

            if (auktionModel.Bids.Count > 0)
            {
                TempData["Message"] =
                    "Misslyckades med att ta bort auktionen! Du kan inte ta bort en auktion som har blivit budad.";
                return RedirectToAction("Index");
            }

            await _service.DeleteAuktionAsync(model.AuktionID);
            TempData["Message"] = $"Auktionen \"{auktionModel.Titel}\" har blivit borttagen!";

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Statistics(StatisticsFilterModel filter)
        {
            string myId = _userManager.GetUserId(User);

            List<AuktionBudViewModel> allAuktions = await _service.ListAuktionBudsAsync(true);

            List<AuktionBudViewModel> filteredAuktions = allAuktions
                .WhereIf(!filter.ShowMine, a => a.SkapadAv != myId)
                .WhereIf(!filter.ShowOthers, a => a.SkapadAv == myId)
                .Where(a => a.SlutDatum.Year == filter.TimeYear && a.SlutDatum.Month == (int)filter.TimeMonth)
                .ToList();

            var model = new StatisticsViewModel
            {
                Filter = filter,
                Auktions = filteredAuktions
            };

            return View(model);
        }
    }
}