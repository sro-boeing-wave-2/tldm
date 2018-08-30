using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace tryingWithAngularChat.Models
{
    public class Channel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public int channelId { get; set; }
        [BsonElement("channelName")]
        public string channelName { get; set; }
        [BsonElement("users")]
        public List<User> users { get; set; }
        [BsonElement("admin")]
        public User admin { get; set; }
        [BsonElement("messages")]
        public List<Message> messages { get; set; }
    }
}
