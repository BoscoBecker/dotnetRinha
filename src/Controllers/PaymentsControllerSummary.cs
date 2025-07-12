using dotnetRinha.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace dotnetRinha.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class PaymentsControllerSummary(IPaymentLogService logService) : ControllerBase
    {
        private readonly IPaymentLogService _logService = logService;      

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
