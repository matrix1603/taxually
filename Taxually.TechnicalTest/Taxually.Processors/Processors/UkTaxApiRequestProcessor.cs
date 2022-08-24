using System.Net;
using Taxually.Models.Enums;
using Taxually.Processors.Clients;
using Taxually.Processors.Models.Requests;
using Taxually.Processors.Models.Responses;
using Taxually.Processors.Processors.Abstract;

namespace Taxually.Processors.Processors
{
    public class UkTaxApiRequestProcessor : RegistrationRequestProcessor
    {
        private readonly ITaxuallyHttpClient _taxuallyHttpClient;

        public UkTaxApiRequestProcessor(ITaxuallyHttpClient taxuallyHttpClient)
        {
            _taxuallyHttpClient = taxuallyHttpClient;
        }

        public override ProcessorType ProcessorType => ProcessorType.UKApi;

        protected override async Task<VatRegistrationResponse> ExecuteAsync(VatRegistrationRequest request)
        {
            await _taxuallyHttpClient.PostAsync("https://api.uktax.gov.uk", request);  ////Move https://api.uktax.gov.uk into config

            return new VatRegistrationResponse { StatusCode = HttpStatusCode.OK, Message = "Success" };
        }
    }
}
