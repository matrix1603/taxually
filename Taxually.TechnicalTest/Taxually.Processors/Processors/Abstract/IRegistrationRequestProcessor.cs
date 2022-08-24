using Taxually.Models.Enums;
using Taxually.Processors.Models.Requests;
using Taxually.Processors.Models.Responses;

namespace Taxually.Processors.Processors.Abstract
{
   public interface IRegistrationRequestProcessor
    {
        ProcessorType ProcessorType { get; }
        Task<VatRegistrationResponse> ProcessAsync(VatRegistrationRequest request);
    }
}
