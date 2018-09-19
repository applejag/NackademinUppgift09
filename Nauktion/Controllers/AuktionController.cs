using System.Collections.Generic;
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

            return View(auktions);
        }
        
        public async Task<IActionResult> View(int id)
        {
            AuktionModel auktion = await _service.GetAuktionAsync(id);

            if (auktion == null)
                return RedirectToAction("Index");

            return View("Auktion", auktion);
        }
    }
}