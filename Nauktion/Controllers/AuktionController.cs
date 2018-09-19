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
            List<AuktionModel> auktions = await _service.ListAuktionsAsync();
            List<AuktionBudViewModel> model = auktions
                .Select(a => new AuktionBudViewModel(a, new List<BudModel>()))
                .ToList();

            return View(model);
        }
        
        public async Task<IActionResult> View(int id)
        {
            AuktionModel auktion = await _service.GetAuktionAsync(id);

            if (auktion == null)
                return RedirectToAction("Index");

            var model = new AuktionBudViewModel(auktion, new List<BudModel>());

            return View("Auktion", model);
        }
    }
}