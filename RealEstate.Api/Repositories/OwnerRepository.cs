using MongoDB.Driver;
using RealEstate.Api.Domain;
using RealEstate.Api.Settings;

namespace RealEstate.Api.Repositories
{
    public class OwnerRepository : IOwnerRepository
    {
        private readonly IMongoCollection<Owner> _collection;

        public OwnerRepository(MongoDbSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var db = client.GetDatabase(settings.DatabaseName);
            _collection = db.GetCollection<Owner>(settings.OwnersCollectionName); 
        }

        public async Task<List<Owner>> GetAsync()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }

        public async Task<Owner?> GetByIdAsync(string id)
        {
            return await _collection.Find(o => o.Id == id).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(Owner owner)
        {
            await _collection.InsertOneAsync(owner);
        }

        public async Task UpdateAsync(string id, Owner owner)
        {
            await _collection.ReplaceOneAsync(o => o.Id == id, owner);
        }

        public async Task DeleteAsync(string id)
        {
            await _collection.DeleteOneAsync(o => o.Id == id);
        }
    }
}