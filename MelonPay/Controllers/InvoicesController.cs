using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MelonPay.Controllers
{
    using MelonPay.Abstractions;

    [Route("api/[controller]")]
    public class InvoicesController : Controller
    {
        private readonly IInvoiceRepository _invoices;

        public InvoicesController(IInvoiceRepository invoices)
        {
            _invoices = invoices;
        }


        [Route("{cardHolderId:int}")]
        public async Task<IActionResult> Index([FromRoute]int cardHolderId)
        {
            return Ok(await _invoices.GetByCardHolderIdAsync(cardHolderId));
        }
    }
}
