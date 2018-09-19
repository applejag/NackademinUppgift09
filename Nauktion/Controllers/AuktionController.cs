using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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

        public AuktionController(IAuktionService service)
        {
            _service = service;
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

            return View(model);
        }

        public IActionResult Bid(BiddingViewModel bid)
        {
            TempData["JustBidded"] = true;

            return RedirectToAction("View", new {id = bid.AuktionID});
        }
    }
}