using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace tryingWithAngularChat.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public int userId { get; set; }
        [BsonElement("firstName")]
        public string firstName { get; set; }
        [BsonElement("lastName")]
        public string lastName { get; set; }
        [BsonElement("username")]
        public string username { get; set; }
        [BsonElement("designation")]
        public string designation { get; set; }
        [BsonElement("workspaceName")]
        public string workspaceName { get; set; } ///////////////doubt

    }
}
