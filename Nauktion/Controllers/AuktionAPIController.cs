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
            NauktionUser currentUser = await _userManager.GetUserAsync(User);

            string error = await _service.ValidateBud(AuktionID, Summa, currentUser);

            if (error is null)
                return new JsonResult(true);

            return new JsonResult(error);
        }
    }
}