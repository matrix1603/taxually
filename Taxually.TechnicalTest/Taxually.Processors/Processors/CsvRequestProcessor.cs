using System.Net;
using System.Text;
using Taxually.Models.Enums;
using Taxually.Processors.Clients;
using Taxually.Processors.Models.Requests;
using Taxually.Processors.Models.Responses;
using Taxually.Processors.Processors.Abstract;

namespace Taxually.Processors.Processors
{
    public class CsvRequestProcessor : RegistrationRequestProcessor
    {
        private readonly ITaxuallyQueueClient _taxuallyQueueClient;

        public override ProcessorType ProcessorType => ProcessorType.CSV;

        public CsvRequestProcessor(ITaxuallyQueueClient taxuallyQueueClient)
        {
            _taxuallyQueueClient = taxuallyQueueClient;
        }

        protected override async Task<VatRegistrationResponse> ExecuteAsync(VatRegistrationRequest request)
        {
            //// TODO Error handling
            //// TODO Logging
            
            var csvBuilder = new StringBuilder();
            csvBuilder.AppendLine("CompanyName,CompanyId");
            csvBuilder.AppendLine($"{request.CompanyName}{request.CompanyId}");
            var csv = Encoding.UTF8.GetBytes(csvBuilder.ToString());
            
            await _taxuallyQueueClient.EnqueueAsync("vat-registration-csv", csv);
            
            return new VatRegistrationResponse { StatusCode = HttpStatusCode.OK, Message = "Success" };
        }
    }
}
