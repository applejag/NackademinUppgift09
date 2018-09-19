using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nauktion.Helpers;
using Nauktion.Models;
using Nauktion.Services;

namespace Nauktion.Controllers
{
    [Route("api/Auktion")]
    [ApiController]
    public class AuktionAPIController : ControllerBase
    {
        private readonly IAuktionService _service;

        public AuktionAPIController(IAuktionService service)
        {
            _service = service;
        }

        [AcceptVerbs("Get", "Post")]
        [Route(nameof(VerifyBudSumma))]
        public async Task<IActionResult> VerifyBudSumma(int AuktionID, int Summa)
        {
            AuktionBudViewModel auktion = await _service.GetAuktionBudsAsync(AuktionID);

            if (auktion is null)
            {
                return NotFound();
            }

            if (Summa <= auktion.MaxedPrice)
            {
                return auktion.HighestBid is null 
                    ? new JsonResult("Budet måste vara större än utropspriset!") 
                    : new JsonResult("Budet måste vara större än det högsta budet!");
            }

            return new JsonResult(true);
        }
    }
}