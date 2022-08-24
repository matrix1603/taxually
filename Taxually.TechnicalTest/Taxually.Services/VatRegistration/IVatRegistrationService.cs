using Taxually.Processors.Models.Requests;
using Taxually.Processors.Models.Responses;

namespace Taxually.Services.VatRegistration
{
    public interface IVatRegistrationService
    {
        Task<VatRegistrationResponse> Register(VatRegistrationRequest request);
    }
}
