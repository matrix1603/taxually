using Taxually.Models.Enums;
using Taxually.Processors.Models.Requests;
using Taxually.Processors.Models.Responses;

namespace Taxually.Processors.Processors.Abstract
{

    public abstract class RegistrationRequestProcessor : IRegistrationRequestProcessor
    {
        public abstract ProcessorType ProcessorType { get; }

        public async Task<VatRegistrationResponse> ProcessAsync(VatRegistrationRequest request)
        {
            return await ExecuteAsync(request);
        }

        protected abstract Task<VatRegistrationResponse> ExecuteAsync(VatRegistrationRequest model);
    }
}
