using System.Net;
using Microsoft.AspNetCore.Mvc;
using Taxually.Processors.Models.Requests;
using Taxually.Services.VatRegistration;

namespace Taxually.TechnicalTest.Controllers
{
    //// TODO Add tests for Taxually.Processors and component testing.
    //// TODO Implement OpenTelemetry
    //// TODO Implement Jaeger Tracing
    //// TODO Implement Retry, Circuit Breaker and Timeout policies using Poly nuget package
    //// TODO CancellationToken cancellationToken
    //// TODO Error handling
    //// TODO Logging
    //// TODO Use IHttpClientFactory
    //// TODO Improve messages
    //// TODO Remove the dependency of HttpStatusCode from services
    //// TODO Use Cache
    ///  TODO Implement docker

    [Route("api/[controller]")]
    [ApiController]
    public class VatRegistrationController : ControllerBase
    {
        private readonly IVatRegistrationService _vatRegistrationService;

        public VatRegistrationController(IVatRegistrationService vatRegistrationService)
        {
            _vatRegistrationService = vatRegistrationService;
        }
        /// <summary>
        /// Registers a company for a VAT number in a given country
        /// </summary>
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IActionResult))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(IActionResult))]
        public async Task<IActionResult> Post([FromBody] VatRegistrationRequest request)
        {
          var result = await _vatRegistrationService.Register(request);

          return StatusCode((int)result.StatusCode, result.Message);
        }
    }
}