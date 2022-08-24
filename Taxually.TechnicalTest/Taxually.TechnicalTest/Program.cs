using Taxually.Processors.Clients;
using Taxually.Processors.Processors;
using Taxually.Processors.Processors.Abstract;
using Taxually.Processors.Validators;
using Taxually.Repositories;
using Taxually.Services.VatRegistration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IVatProcessorMappingRepository, VatProcessorMappingRepository>();

builder.Services.AddTransient<IVatRegistrationRequestValidator, VatRegistrationRequestValidator>();

builder.Services.AddTransient<ITaxuallyHttpClient, TaxuallyHttpClient>();
builder.Services.AddTransient<ITaxuallyQueueClient, TaxuallyQueueClient>();

builder.Services.AddTransient<IRegistrationRequestProcessor, XmlRequestRequestProcessor>();
builder.Services.AddTransient<IRegistrationRequestProcessor, CsvRequestProcessor>();
builder.Services.AddTransient<IRegistrationRequestProcessor, UkTaxApiRequestProcessor>();

builder.Services.AddTransient<IVatRegistrationService, VatRegistrationService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
