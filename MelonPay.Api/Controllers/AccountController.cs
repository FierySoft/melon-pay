using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MelonPay.Api.Controllers
{
    using MelonPay.Api.Abstractions;

    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }


        [HttpGet]
        public async Task<IActionResult> WhoAmI()
        {
            return Ok(await _userService.GetSignedInAsync());
        }
    }
}
