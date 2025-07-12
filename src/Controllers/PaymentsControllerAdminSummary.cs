using dotnetRinha.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace dotnetRinha.Controllers
{
    [ApiController]
    [Route("/admin/payments-summary")]
    public class PaymentsControllerAdminSummary(IPaymentSummaryService summaryService) : ControllerBase
    {
        private readonly IPaymentSummaryService _summaryService = summaryService;

        [HttpGet]
        public async Task<IActionResult> GetAdminSummary([FromQuery] DateTime from, [FromQuery] DateTime to)
        {
            try
            {
                var summary = await _summaryService.GetSummaryAsync();
                return Ok(summary);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[PaymentControllerAdminSummary] Erro: {ex}");
                return StatusCode(500, new { message = "Erro interno" });
            }
        }
    }

}
