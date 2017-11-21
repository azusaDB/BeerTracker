using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using MongoDB.Bson.Serialization.Attributes;

namespace BeerTracker.Models
{
    public class AddBeer
    {
        [BsonId]
        public string Id { get; set; }
        public string name { get; set; }
        public string nameDisplay { get; set; }
        public string description { get; set; }
        public string abv { get; set; }
        //public string ibu { get; set; }
        //public int glasswareId { get; set; }
        //public int availableId { get; set; }
        //public int styleId { get; set; }
        //public string isOrganic { get; set; }
        //public object labels { get; set; }
        //public string iconImage { get; set; }
        public string medImage { get; set; }
        //public string lrgImage { get; set; }
        //public string status { get; set; }
        //public string statusDisplay { get; set; }
        //public object originalGravity { get; set; }
        //public string createDate { get; set; }
        //public string updateDate { get; set; }
        public string breweryName { get; set; }
        public string breweryUrl { get; set; }
       
    }
}