using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Nauktion.Helpers;
using Nauktion.Models;
using Nauktion.Services;

namespace Nauktion.Controllers
{
    [Route("api/Auktion")]
    [ApiController]
    [AuthorizeRole(NauktionRoles.Regular)]
    public class AuktionAPIController : ControllerBase
    {
        private readonly IAuktionService _service;
        private readonly UserManager<NauktionUser> _userManager;

        public AuktionAPIController(IAuktionService service, 
            UserManager<NauktionUser> userManager)
        {
            _service = service;
            _userManager = userManager;
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
                    ? new JsonResult("Budet måste vara större än utropspriset.") 
                    : new JsonResult("Budet måste vara större än det högsta budet.");
            }

            NauktionUser topBidder = await _userManager.FindByIdAsync(auktion.HighestBid?.Budgivare);
            NauktionUser currentUser = await _userManager.GetUserAsync(User);
            if (topBidder?.Id == currentUser.Id)
            {
                return new JsonResult("Du kan inte buda när du har högsta budet.");
            }

            return new JsonResult(true);
        }
    }
}