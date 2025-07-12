using dotnetRinha.Entities;
using dotnetRinha.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace dotnetRinha.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class PaymentsController(IPaymentProcessorService processorService) : ControllerBase
    {
        private readonly IPaymentProcessorService _processorService = processorService;
                
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PaymentRequest req)
        {
            var success = await _processorService.ProcessPaymentAsync(req);            
            if (success) 
                return Ok(new PaymentResponse());
            return StatusCode(502, new { message = "processor failed" });
        }
    }
}
