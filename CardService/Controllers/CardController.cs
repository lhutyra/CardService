using CardService.Card;
using Microsoft.AspNetCore.Mvc;

namespace CardService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CardController : ControllerBase
    {
        private readonly ICardService _cardService;

        public CardController(ICardService cardService)
        {
            _cardService = cardService;
        }

        [HttpGet]
        [Route("actions")]
        public async Task<IActionResult> GetCardActions([FromQuery] string userId, [FromQuery] string cardNumber)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(cardNumber))
            {
                return BadRequest("User ID and Card Number are required");
            }

            var cardDetails =await _cardService.GetCardDetails(userId, cardNumber);
            if (cardDetails == null)
            {
                return NotFound("Card not found");
            }
            var actions =  _cardService.GetActions(cardDetails);
            return Ok(actions);
        }
    }

}
