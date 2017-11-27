using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace BeerTracker.Models
{
    public class UserBeer
    {
        [BsonId]
        public string id { get; set; }
        public string beerId { get; set; }
        public string username { get; set; }

        public UserBeer()
        {

        }
        public UserBeer(string bid, string uid)
        {
            id = ObjectId.GenerateNewId().ToString();
            beerId = bid;
            username = uid;
        }
    }
}
