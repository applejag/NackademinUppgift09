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
        
        public async Task<IActionResult> Index(int? page)
        {
            AuktionBudPageinatedViewModel model = await _service.ListAuktionBudsPaginatedAsync(page ?? 1);

            if (page.HasValue && (page < 1 || page > model.NumOfPages))
            {
                if (page.Value == 1)
                    return RedirectToAction("View");
                return RedirectToAction("View", new {page = Math.Clamp(page.Value, 1, model.NumOfPages)});
            }

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

        public async Task<IActionResult> Search(SearchViewModel searchModel)
        {
            AuktionBudPageinatedViewModel auktionModel = await _service.ListAuktionBudsPaginatedAsync();
            var model = new SearchResultsViewModel
            {
                AuktionModel = auktionModel,
                SearchModel = searchModel
            };

            return View(model);
        }

    }
}