using Microsoft.AspNetCore.Mvc;

namespace CardService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CardController : ControllerBase
    {
        private readonly CardService _cardService;

        public CardController()
        {
            _cardService = new CardService();
        }

        [HttpGet]
        [Route("actions")]
        public IActionResult GetCardActions([FromQuery] string userId, [FromQuery] string cardNumber)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(cardNumber))
            {
                return BadRequest("User ID and Card Number are required");
            }

            var cardDetails = _cardService.GetCardDetails(userId, cardNumber);
            if (cardDetails == null)
            {
                return NotFound("Card not found");
            }

            var actions = _cardService.GetActions(cardDetails);
            return Ok(actions);
        }
    }
}
