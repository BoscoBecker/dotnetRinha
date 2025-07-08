using dotnetRinha.Entities;
using dotnetRinha.Service;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace dotnetRinha.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentProcessorService _processorService;
        private readonly IPaymentSummaryService _summaryService;
        //public PaymentController(IPaymentProcessorService processorService) => _processorService = processorService;        
        public PaymentController(IPaymentProcessorService processorService, IPaymentSummaryService summaryService)
        {
            _processorService = processorService;
            _summaryService = summaryService;
        }

        // POST api/<PaymentController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PaymentRequest req)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var success = await _processorService.ProcessPaymentAsync(req);
            if (success)  
                return Ok(new PaymentResponse());

            return StatusCode(502, new { message = "processor failed" });
        }


        // GET api/<PaymentController>/summary
        [HttpGet("/payments-summary")]
        public Task<PaymentSummary> Get() => _summaryService.GetSummaryAsync();
        

    }
}
