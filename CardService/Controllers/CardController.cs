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

        [HttpPost]
        [Route("actions")]
        public IActionResult GetCardActions([FromBody] CardDetails cardDetails)
        {
            if (cardDetails == null)
            {
                return BadRequest("Invalid card details");
            }

            var actions = _cardService.GetActions(cardDetails.CardType, cardDetails.CardStatus, cardDetails.IsPinSet);
            return Ok(actions);
        }
    }
}
