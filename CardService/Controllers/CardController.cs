using CardService.Card;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CardService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserActionsController : ControllerBase
    {


        [HttpPost]
        public IActionResult GetAllowedActions([FromBody] UserCardRequest request)
        {
            var actions = GenerateAllowedActions(request.UserId, request.CardNumber);
            return Ok(actions);
        }

        private List<string> GenerateAllowedActions(string userId, string cardNumber)
        {
            // Implement logic to generate allowed actions based on userId and cardNumber
            return new List<string> { "Action1", "Action2" };
        }
    }

}
