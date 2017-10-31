using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Linq;

namespace BeerTracker.Models
{
    public class Beer
    {
        public string id { get; set; }
        public string name { get; set; }
        public string shortName { get; set; }
        public double abv { get; set; }
        public int breweryId { get; set; } //known as styleId
        public string breweryName { get; set; }
        public string image_small { get; set; }
        public string image_large { get; set; }
        public string description { get; set; }
        public decimal ibu { get; set; }

        public Beer(string json)
        {
            double doubleParseTest;
            int intParseTest;

            id = !String.IsNullOrEmpty(JObject.Parse(json)["data"]["id"].ToString()) ? JObject.Parse(json)["data"]["id"].ToString() : "N/A";
            name = !String.IsNullOrEmpty(JObject.Parse(json)["data"]["name"].ToString()) ? JObject.Parse(json)["data"]["name"].ToString() : "N/A";
            shortName = !String.IsNullOrEmpty(JObject.Parse(json)["data"]["style"]["shortName"].ToString()) ? JObject.Parse(json)["data"]["style"]["shortName"].ToString() : "N/A";
            abv = double.TryParse(JObject.Parse(json)["data"]["abv"].ToString(), out doubleParseTest) ? doubleParseTest : 0.0;
            breweryId = int.TryParse(JObject.Parse(json)["data"]["styleId"].ToString(), out intParseTest) ? intParseTest : 0 ;
        }
    }
}