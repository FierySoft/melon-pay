using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MelonPay.Api.Controllers
{
    using MelonPay.Common.Abstractions;
    using MelonPay.Common.Models;

    [Route("api/[controller]")]
    public class InvoicesController : Controller
    {
        private readonly IInvoiceRepository _invoices;

        public InvoicesController(IInvoiceRepository invoices)
        {
            _invoices = invoices;
        }


        [HttpGet("{cardHolderId:int}")]
        public async Task<IActionResult> Index([FromRoute]int cardHolderId)
        {
            return Ok(await _invoices.GetByCardHolderIdAsync(cardHolderId));
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody]InvoiceCreate invoice)
        {
            return Ok(await _invoices.CreateAsync(invoice.FromWalletId, invoice.ToWalletId, invoice.Amount, invoice.Comment));
        }

        [HttpPost("pay/{id:int}")]
        public async Task<IActionResult> Pay([FromRoute]int id)
        {
            var result = await _invoices.PayAsync(id);

            if (result.Status.Code == "Payed")
            {
                return Ok(result.Status);
            }
            else
            {
                return BadRequest(result.Status);
            }
        }

        [HttpPost("decline/{id:int}")]
        public async Task<IActionResult> Decline([FromRoute]int id)
        {
            return Ok((await _invoices.DeclineAsync(id))?.Status);
        }
    }
}
