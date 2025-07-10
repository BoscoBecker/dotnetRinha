using dotnetRinha.Entities;
using dotnetRinha.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace dotnetRinha.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentProcessorService _processorService;
        private readonly IPaymentSummaryService _summaryService;
        private readonly IPaymentLogService _logService;

        public PaymentController(IPaymentProcessorService processorService, IPaymentSummaryService summaryService, IPaymentLogService logService)
        {
            _processorService = processorService;
            _summaryService = summaryService;
            _logService = logService;
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
        public async Task<IActionResult> GetSummary([FromQuery] DateTime from, [FromQuery] DateTime to)
        {
            var logs = await _logService.GetLogsAsync(from, to);

            var grouped = logs
                .GroupBy(e => e.Source)
                .ToDictionary(
                    g => g.Key,
                    g => new
                    {
                        totalRequests = g.Count(),
                        totalAmount = g.Sum(x => x.Amount)
                    }
                );

            return Ok(grouped);
        }
    }
}
