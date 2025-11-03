using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RealEstate.Api.Domain
{
    public class Property
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        // [BsonRepresentation(BsonType.ObjectId)] 
        public string IdOwner { get; set; } = string.Empty; 

        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string CodeInternal { get; set; } = string.Empty;
        public int Year { get; set; }
        public PropertyImage? Image { get; set; } 
        // public List<PropertyTrace> Traces { get; set; } = new List<PropertyTrace>();
    }
}