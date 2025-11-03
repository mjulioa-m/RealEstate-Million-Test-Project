using RealEstate.Api.Domain;

namespace RealEstate.Api.Repositories
{
    public interface IPropertyRepository
    {
        Task<List<Property>> GetPropertiesAsync(string? name, string? address, decimal? minPrice, decimal? maxPrice, int page = 1, int pageSize = 20);
        Task<Property?> GetByIdAsync(string id);
        Task CreateManyAsync(List<Property> properties);
        Task CreateAsync(Property property);
        Task UpdateAsync(string id, Property property);
        Task DeleteAsync(string id);
        Task AddImageAsync(string propertyId, PropertyImage image);
        // Task AddTraceAsync(string propertyId, PropertyTrace trace);
    }
}
