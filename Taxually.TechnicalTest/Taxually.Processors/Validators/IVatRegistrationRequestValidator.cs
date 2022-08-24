using Taxually.Processors.Models.Requests;

namespace Taxually.Processors.Validators
{
    public interface IVatRegistrationRequestValidator
    {
        (bool IsValid, string Message) Validate(VatRegistrationRequest request);
    }
}
