using System.Net;
using AutoFixture;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Taxually.Models.Enums;
using Taxually.Processors.Models.Requests;
using Taxually.Processors.Models.Responses;
using Taxually.Processors.Processors.Abstract;
using Taxually.Processors.Validators;
using Taxually.Repositories;
using Taxually.Services.VatRegistration;

namespace Taxually.Services.Tests.VatRegistration
{
    public class VatRegistrationServiceTests
    {
        private Mock<IEnumerable<IRegistrationRequestProcessor>> _mockRegistrationRequestProcessors;
        private Mock<IRegistrationRequestProcessor> _mockRegistrationRequestProcessor;
        private Mock<ILogger<VatRegistrationService>> _mockLogger;
        private Mock<IVatProcessorMappingRepository> _mockVatProcessorMappingRepository;
        private Mock<IVatRegistrationRequestValidator> _mockVatRegistrationRequestValidator;

        private Fixture _fixture;
        private VatRegistrationRequest _request;
        private VatRegistrationService _registrationService;

        [SetUp]
        public void Setup()
        {
            _mockRegistrationRequestProcessors = new Mock<IEnumerable<IRegistrationRequestProcessor>>();
            _mockRegistrationRequestProcessor = new Mock<IRegistrationRequestProcessor>();
            _mockLogger = new Mock<ILogger<VatRegistrationService>>();
            _mockVatProcessorMappingRepository = new Mock<IVatProcessorMappingRepository>();
            _mockVatRegistrationRequestValidator = new Mock<IVatRegistrationRequestValidator>();

            _fixture = new Fixture();
            _request = _fixture.Create<VatRegistrationRequest>();
            
            _registrationService = new VatRegistrationService(_mockLogger.Object, _mockVatProcessorMappingRepository.Object, _mockRegistrationRequestProcessors.Object, _mockVatRegistrationRequestValidator.Object);
        }

        [Test]
        public async Task Register_When_Validation_Failed_Returns_BadRequest()
        {
            _request.Country = "";

           var result = await _registrationService.Register(_request);

           Assert.IsNotNull(result.StatusCode == HttpStatusCode.BadRequest);

           _mockVatRegistrationRequestValidator.Verify(x => x.Validate(It.IsAny<VatRegistrationRequest>()), Times.Once);
           _mockVatProcessorMappingRepository.Verify(x => x.Get(It.IsAny<string>()), Times.Never);
        }

        [Test]
        public async Task Register_When_Country_Not_Supported_Returns_NotImplemented()
        {
            _mockVatProcessorMappingRepository.Setup(m => m.Get(It.IsAny<string>())).Returns((ProcessorType?)null);
            _mockVatRegistrationRequestValidator.Setup(m => m.Validate(It.IsAny<VatRegistrationRequest>())).Returns((true, ""));
            
            var result = await _registrationService.Register(_request);

            Assert.IsNotNull(result.StatusCode == HttpStatusCode.NotImplemented);

            _mockVatRegistrationRequestValidator.Verify(x => x.Validate(It.IsAny<VatRegistrationRequest>()), Times.Once);
            _mockVatProcessorMappingRepository.Verify(x => x.Get(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task Register_When_Country_GB_Return_Returns_StatusOk()
        {
            _request.Country = "GB";

            _mockRegistrationRequestProcessor.Setup(x => x.ProcessAsync(It.IsAny<VatRegistrationRequest>())).ReturnsAsync(new VatRegistrationResponse { StatusCode = HttpStatusCode.OK});
            _mockRegistrationRequestProcessor.Setup(x => x.ProcessorType).Returns(ProcessorType.UKApi);
            var processors = new List<IRegistrationRequestProcessor> { _mockRegistrationRequestProcessor.Object  };
            _mockVatRegistrationRequestValidator.Setup(m => m.Validate(It.IsAny<VatRegistrationRequest>())).Returns((true,""));
            _mockVatProcessorMappingRepository.Setup(m => m.Get(It.IsAny<string>())).Returns(ProcessorType.UKApi);
            _mockRegistrationRequestProcessors.Setup(m => m.GetEnumerator()).Returns(() => processors.GetEnumerator());


            var result = await _registrationService.Register(_request);

            Assert.True(result.StatusCode == HttpStatusCode.OK);

            _mockVatRegistrationRequestValidator.Verify(x => x.Validate(It.IsAny<VatRegistrationRequest>()), Times.Once);
            _mockVatProcessorMappingRepository.Verify(x => x.Get(It.IsAny<string>()), Times.Once);
            _mockRegistrationRequestProcessor.Verify(x => x.ProcessAsync(It.IsAny<VatRegistrationRequest>()), Times.Once);
        }
    }
}
