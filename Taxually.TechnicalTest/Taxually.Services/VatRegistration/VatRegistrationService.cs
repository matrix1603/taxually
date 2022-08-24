using System.Net;
using Microsoft.Extensions.Logging;
using Taxually.Processors.Models.Requests;
using Taxually.Processors.Models.Responses;
using Taxually.Processors.Processors.Abstract;
using Taxually.Processors.Validators;
using Taxually.Repositories;

namespace Taxually.Services.VatRegistration
{
    public class VatRegistrationService : IVatRegistrationService
    {
        private readonly ILogger<VatRegistrationService> _logger;
        private readonly IEnumerable<IRegistrationRequestProcessor> _registrationRequestProcessors;
        private readonly IVatProcessorMappingRepository _vatProcessorMappingRepository;
        private readonly IVatRegistrationRequestValidator _vatRegistrationRequestValidator;

        public VatRegistrationService(ILogger<VatRegistrationService> logger,
            IVatProcessorMappingRepository vatProcessorMappingRepository,
            IEnumerable<IRegistrationRequestProcessor> registrationRequestProcessors,
            IVatRegistrationRequestValidator vatRegistrationRequestValidator)
        {
            _logger = logger;
            _vatProcessorMappingRepository = vatProcessorMappingRepository;
            _registrationRequestProcessors = registrationRequestProcessors;
            _vatRegistrationRequestValidator = vatRegistrationRequestValidator;
        }

        public async Task<VatRegistrationResponse> Register(VatRegistrationRequest request)
        {
            try
            {
                var validationResult = _vatRegistrationRequestValidator.Validate(request);

                if (!validationResult.IsValid)
                    return new VatRegistrationResponse { Message = validationResult.Message, StatusCode = HttpStatusCode.BadRequest };

                var processorType = _vatProcessorMappingRepository.Get(request.Country);
                if (processorType == null)
                {
                    _logger.LogWarning($"Country not supported, Country :'{request.CompanyName}'");
                    return new VatRegistrationResponse { Message = "Country not supported", StatusCode = HttpStatusCode.NotImplemented };
                }

                var processor = _registrationRequestProcessors.Single(x => x.ProcessorType == processorType);
                return await processor.ProcessAsync(request);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Unable to ProcessAsync CompanyName:'{request.CompanyName}'");

                return new VatRegistrationResponse
                {
                    Message = "There was an error with the request",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }
    }
}