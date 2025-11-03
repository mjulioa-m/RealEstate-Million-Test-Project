using MongoDB.Driver;
using MongoDB.Bson;
using RealEstate.Api.Domain;
using RealEstate.Api.Settings;
using MongoDB.Driver.Linq; 
namespace RealEstate.Api.Repositories
{
    public class PropertyRepository : IPropertyRepository
    {
        private readonly IMongoCollection<Property> _collection;

        public PropertyRepository(MongoDbSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var db = client.GetDatabase(settings.DatabaseName);
            _collection = db.GetCollection<Property>(settings.PropertiesCollectionName);
        }
        public async Task<List<Property>> GetPropertiesAsync(string? name, string? address, decimal? minPrice, decimal? maxPrice, int page = 1, int pageSize = 20)
        {
            var query = _collection.AsQueryable();

            if (!string.IsNullOrEmpty(name))
                query = query.Where(p => p.Name.Contains(name));

            if (!string.IsNullOrEmpty(address))
                query = query.Where(p => p.Address.Contains(address));

            if (minPrice.HasValue)
                query = query.Where(p => p.Price >= minPrice.Value);

            if (maxPrice.HasValue)
                query = query.Where(p => p.Price <= maxPrice.Value);

            return await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
        public async Task<Property?> GetByIdAsync(string id)
        {
            return await _collection.Find(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(Property property)
        {
            await _collection.InsertOneAsync(property);
        }

        public async Task UpdateAsync(string id, Property property)
        {
            await _collection.ReplaceOneAsync(p => p.Id == id, property);
        }

        public async Task DeleteAsync(string id)
        {
            await _collection.DeleteOneAsync(p => p.Id == id);
        }

        public async Task CreateManyAsync(List<Property> properties)
        {
            await _collection.InsertManyAsync(properties);
        }

public async Task AddImageAsync(string propertyId, PropertyImage image)
{
    var filter = Builders<Property>.Filter.Eq(p => p.Id, propertyId);
    var update = Builders<Property>.Update.Set(p => p.Image, image);
    
    await _collection.UpdateOneAsync(filter, update);
}

// public async Task AddTraceAsync(string propertyId, PropertyTrace trace)
// {
//     var filter = Builders<Property>.Filter.Eq(p => p.Id, propertyId);
//     var update = Builders<Property>.Update.Push(p => p.Traces, trace);
    
//     await _collection.UpdateOneAsync(filter, update);
// }

    }
}
