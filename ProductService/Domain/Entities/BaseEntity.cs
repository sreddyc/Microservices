using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace Domain.Entities
{
    public class BaseEntity
    {
      //  [BsonId(IdGenerator=typeof(StringObjectIdGenerator))]
         public ObjectId Id { get; set; }
    }
}