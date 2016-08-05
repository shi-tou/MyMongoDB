using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongodbView.Model
{
    public class BaseModel
    {
        [BsonId]
        private ObjectId Id { get; set; }
    }
}
