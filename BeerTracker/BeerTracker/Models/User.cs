using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using MongoDB.Bson.Serialization.Attributes;

namespace BeerUsers.Models
{
    public class User
    {
        [BsonId]
        public string uid { get; set; }
        public string password { get; set; }
        public bool status { get; set; }
        public string bid { get; set; }
    }
}