using MongoDB.Bson;

namespace Persistence.Extensions
{
    public static class ConvertToObjectId
    {
        public static ObjectId ToObjectId(this string id)
        {
            if (!ObjectId.TryParse(id, out ObjectId internalId))
                internalId = ObjectId.Empty;

            return internalId;
        }
    }
}