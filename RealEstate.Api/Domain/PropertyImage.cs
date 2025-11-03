using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RealEstate.Api.Domain
{
    public class PropertyImage
    {
        public string File { get; set; } = string.Empty; 
        public bool Enabled { get; set; }
    }
}