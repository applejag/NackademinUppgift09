using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nauktion.Models;
using Nauktion.Services;

namespace Nauktion.Controllers
{
    public class AuktionController : Controller
    {
        private readonly IAuktionService _service;

        public AuktionController(IAuktionService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            List<AuktionModel> auktions = await _service.ListAuktions();

            return View(auktions);
        }
    }
}