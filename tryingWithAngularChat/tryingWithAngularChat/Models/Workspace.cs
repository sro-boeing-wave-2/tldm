using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace tryingWithAngularChat.Models
{
    public class Workspace
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public int workspaceId { get; set; }
        [BsonElement("workspaceName")]
        public string workspaceName { get; set; }
        [BsonElement("channels")]
        public List<Channel> channels { get; set; }
       
    }
}
