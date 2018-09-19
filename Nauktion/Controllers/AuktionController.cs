using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Nauktion.Helpers;
using Nauktion.Models;
using Nauktion.Services;

namespace Nauktion.Controllers
{
    [AuthorizeRole(NauktionRoles.Regular)]
    public class AuktionController : Controller
    {
        private readonly IAuktionService _service;
        private readonly UserManager<NauktionUser> _userManager;

        public AuktionController(IAuktionService service,
            UserManager<NauktionUser> userManager)
        {
            _service = service;
            _userManager = userManager;
        }
        
        public async Task<IActionResult> Index()
        {
            List<AuktionBudViewModel> model = await _service.ListAuktionBudsAsync();

            return View(model);
        }
        
        public async Task<IActionResult> View(int id)
        {
            AuktionBudViewModel model = await _service.GetAuktionBudsAsync(id);

            if (model == null)
                return RedirectToAction("Index");

            NauktionUser currentUser = await _userManager.GetUserAsync(User);

            string error = await _service.ValidateBud(model.AuktionID, model.MaxedPrice + 1, currentUser);
            if (!(error is null))
                TempData["BidErrors"] = new[] { error };

            return View(model);
        }

        public async Task<IActionResult> Bid(BiddingViewModel bid)
        {
            NauktionUser currentUser = await _userManager.GetUserAsync(User);

            // Validate
            if (ModelState.IsValid)
            {
                string error = await _service.ValidateBud(bid.AuktionID, bid.Summa, currentUser);

                if (!(error is null))
                    ModelState.AddModelError(nameof(bid.Summa), error);
            }

            // Redirect if invalid
            if (!ModelState.IsValid)
            {
                TempData["BidErrors"] = ModelState[nameof(bid.Summa)].Errors
                    .Select(e => e.ErrorMessage)
                    .ToArray();

                return RedirectToAction("View", new {id = bid.AuktionID});
            }

            // Valid! Let's create that bid!
            await _service.CreateBudAsync(bid, currentUser);

            TempData["BidSuccess"] = true;
            return RedirectToAction("View", new {id = bid.AuktionID});
        }

        [HttpGet]
        [AuthorizeRole(NauktionRoles.Admin)]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [AuthorizeRole(NauktionRoles.Admin)]
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
        [AuthorizeRole(NauktionRoles.Admin)]
        public async Task<IActionResult> Alter(int id)
        {
            AuktionModel auktionModel = await _service.GetAuktionAsync(id);
            if (auktionModel is null)
                return NotFound();

            NauktionUser currentUser = await _userManager.GetUserAsync(User);

            if (auktionModel.SkapadAv != currentUser.Id)
                return LocalRedirect("/Identity/Account/AccessDenied");

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
        [AuthorizeRole(NauktionRoles.Admin)]
        public async Task<IActionResult> Alter(AuktionViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            AuktionModel auktionModel = await _service.GetAuktionAsync(model.AuktionID);
            if (auktionModel is null)
                return NotFound();

            NauktionUser currentUser = await _userManager.GetUserAsync(User);
            if (auktionModel.SkapadAv != currentUser.Id)
                return LocalRedirect("/Identity/Account/AccessDenied");

            await _service.AlterAuktionAsync(model);

            TempData["Message"] = $"Dina ändringar till auktionen \"{model.Titel}\" har sparats!";
            return View(model);
        }
    }
}