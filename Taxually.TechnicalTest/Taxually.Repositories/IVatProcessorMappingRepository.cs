using Taxually.Models.Enums;

namespace Taxually.Repositories
{
    public interface IVatProcessorMappingRepository
    {
        ProcessorType? Get(string country);
    }
}
