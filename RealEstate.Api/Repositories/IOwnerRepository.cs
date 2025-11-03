using RealEstate.Api.Domain;

namespace RealEstate.Api.Repositories
{
    public interface IOwnerRepository
    {
        Task<List<Owner>> GetAsync();
        Task<Owner?> GetByIdAsync(string id);
        Task CreateAsync(Owner owner);
        Task UpdateAsync(string id, Owner owner);
        Task DeleteAsync(string id);
    }
}