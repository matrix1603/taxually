using System.Net;
using System.Xml.Serialization;
using Taxually.Models.Enums;
using Taxually.Processors.Clients;
using Taxually.Processors.Models.Requests;
using Taxually.Processors.Models.Responses;
using Taxually.Processors.Processors.Abstract;

namespace Taxually.Processors.Processors
{
    public class XmlRequestRequestProcessor : RegistrationRequestProcessor
    {
        private readonly ITaxuallyQueueClient _taxuallyQueueClient;

        public XmlRequestRequestProcessor(ITaxuallyQueueClient taxuallyQueueClient)
        {
            _taxuallyQueueClient = taxuallyQueueClient;
        }

        public override ProcessorType ProcessorType => ProcessorType.XML;

        protected override async Task<VatRegistrationResponse> ExecuteAsync(VatRegistrationRequest request)
        {
            //// TODO Error handling
            //// TODO Logging
            
            using (var stringwriter = new StringWriter())
            {
                var serializer = new XmlSerializer(typeof(VatRegistrationRequest));
                serializer.Serialize(stringwriter, request);
                var xml = stringwriter.ToString();

                _taxuallyQueueClient.EnqueueAsync("vat-registration-xml", xml).Wait();

                return new VatRegistrationResponse { StatusCode = HttpStatusCode.OK, Message = "Success" };
            }
        }
    }
}
