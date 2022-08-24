using Taxually.Models;
using Taxually.Models.Enums;

namespace Taxually.Repositories
{
    public class VatProcessorMappingRepository: IVatProcessorMappingRepository
    {
        public ProcessorType? Get(string country)
        {
            return GetMapping()
                .SingleOrDefault(x => string.Equals(x.Country, country, StringComparison.CurrentCultureIgnoreCase))?.ProcesorType;
        }

        private IReadOnlyList<VatProcessorMapping> GetMapping()
        {
            return new List<VatProcessorMapping>
            {
                new() { Country = "GB", ProcesorType = ProcessorType.UKApi },
                new() { Country = "FR", ProcesorType = ProcessorType.CSV },
                new() { Country = "DE", ProcesorType = ProcessorType.XML},
            };
        }
    }
}