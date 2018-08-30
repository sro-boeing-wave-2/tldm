using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace tryingWithAngularChat.Models
{
    public class Message
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public int messageId { get; set; }
        [BsonElement("messageBody")]
        public string messageBody { get; set; }
        [BsonElement("timeStamp")]
        public string timeStamp { get; set; }
        [BsonElement("isStarred")]
        public bool isStarred { get; set; }
        [BsonElement("sender")]
        public User sender { get; set; }
        
    }
}
