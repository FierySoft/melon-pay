using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MelonPay.Api.Controllers
{
    using MelonPay.Common.Abstractions;

    [Route("api/[controller]")]
    public class CardHoldersController : Controller
    {
        private readonly ICardHolderRepository _cardHolders;

        public CardHoldersController(ICardHolderRepository repo)
        {
            _cardHolders = repo;
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return Ok(await _cardHolders.GetAllAsync());
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> ById([FromRoute]int id)
        {
            return Ok(await _cardHolders.GetByIdAsync(id));
        }
    }
}
