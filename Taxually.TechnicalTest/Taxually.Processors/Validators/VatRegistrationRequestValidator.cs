using Taxually.Processors.Models.Requests;

namespace Taxually.Processors.Validators
{
    public class VatRegistrationRequestValidator : IVatRegistrationRequestValidator
    {
        public (bool IsValid, string Message) Validate(VatRegistrationRequest request)
        {
            //// TODO could use fluentvalidation
            //// https://docs.fluentvalidation.net/en/latest/start.html
            
            if (string.IsNullOrWhiteSpace(request.CompanyId))
                return (false, $"{nameof(VatRegistrationRequest.CompanyId)} is required");

            if (string.IsNullOrWhiteSpace(request.CompanyName))
                return (false, $"{nameof(VatRegistrationRequest.CompanyName)} is required");

            if (string.IsNullOrWhiteSpace(request.Country))
                return (false, $"{nameof(VatRegistrationRequest.Country)} is required");

            return (true, string.Empty);
        }
    }
}
