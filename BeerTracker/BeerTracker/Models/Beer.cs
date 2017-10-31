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
        public string image_medium { get; set; }
        public string image_large { get; set; }
        public string description { get; set; }
        public double ibu { get; set; }

        public Beer(string json)
        {
            //abv = 0;
            //ibu = 0;
            //bool throwError = false;
            //JObject beer = JObject.Parse(json);

            //try
            //{
            //    id = (string)beer.SelectToken("data.id", throwError);
            //    name = (string)beer.SelectToken("data.name", throwError);
            //    shortName = (string)beer.SelectToken("data.style.shortName", throwError);
            //    //abv = (double)beer.SelectToken("data.abv", throwError);
            //    breweryId = (int)beer.SelectToken("data.styleId", throwError);
            //    breweryName = (string)beer.SelectToken("data.style.category.name", throwError);
            //    image_medium = (string)beer.SelectToken("data.style.labels.medium", throwError);
            //    image_large = (string)beer.SelectToken("data.style.labels.large", throwError);
            //    description = (string)beer.SelectToken("data.description", throwError);
            //    //ibu = (double)beer.SelectToken("data.style.ibuMax", throwError);
            //}
            //catch(Exception ex)
            //{
            //    string foo = ex.ToString();
            //}

            

            
        }
    }
}


//id = idToken != null ? JObject.Parse(json)["data"] ["id"].ToString() : "N/A";
//                name = !String.IsNullOrEmpty(JObject.Parse(json)["data"]["name"].ToString()) ? JObject.Parse(json)["data"] ["name"].ToString() : "N/A";
//                shortName = !String.IsNullOrEmpty(JObject.Parse(json)["data"]["style"]["shortName"].ToString()) ? JObject.Parse(json)["data"] ["style"] ["shortName"].ToString() : "N/A";
//                abv = double.TryParse(JObject.Parse(json)["data"]["abv"].ToString(), out doubleParseTest) ? doubleParseTest : 0.0;
//                breweryId = int.TryParse(JObject.Parse(json)["data"]["styleId"].ToString(), out intParseTest) ? intParseTest : 0;
//                breweryName = !String.IsNullOrEmpty(JObject.Parse(json)["data"]["style"]["category"]["name"].ToString()) ? JObject.Parse(json)["data"] ["style"] ["category"] ["name"].ToString() : "N/A";
//                image_medium = !String.IsNullOrEmpty(JObject.Parse(json)["data"]["style"]["labels"]["medium"].ToString()) ? JObject.Parse(json)["data"] ["style"] ["labels"] ["medium"].ToString() : "";
//                image_large = !String.IsNullOrEmpty(JObject.Parse(json)["data"]["style"]["labels"]["large"].ToString()) ? JObject.Parse(json)["data"] ["style"] ["labels"] ["large"].ToString() : "";
//                description = !String.IsNullOrEmpty(JObject.Parse(json)["data"]["description"].ToString()) ? JObject.Parse(json)["data"] ["description"].ToString() : "N/A";













//JToken idToken = JObject.Parse(json)["data"]["id"];
//JToken nameToken = JObject.Parse(json)["data"]["name"];
//JToken shortNameToken = JObject.Parse(json)["data"]["style"]["shortName"];
//JToken abvToken = JObject.Parse(json)["data"]["abv"];
//JToken breweryIdToken = JObject.Parse(json)["data"]["styleId"];
//JToken breweryNameToken = JObject.Parse(json)["data"]["style"]["category"]["name"];
//JToken image_mToken = JObject.Parse(json)["data"]["labels"]["medium"];
//JToken image_lToken = JObject.Parse(json)["data"]["labels"]["large"];
//JToken descriptionToken = JObject.Parse(json)["data"]["description"];

//id = idToken != null ? JObject.Parse(json)["data"]["id"].ToString() : "N/A";
//name = nameToken != null ? JObject.Parse(json)["data"]["name"].ToString() : "N/A";
//shortName = shortNameToken != null ? JObject.Parse(json)["data"]["style"]["shortName"].ToString() : "N/A";
//abv = abvToken != null ? Convert.ToDouble(JObject.Parse(json)["data"]["abv"].ToString()) : 0.0;
//breweryId = breweryIdToken != null ? Convert.ToInt32(JObject.Parse(json)["data"]["styleId"].ToString()) : 0;
//breweryName = breweryNameToken != null ? JObject.Parse(json)["data"]["style"]["category"]["name"].ToString() : "N/A";
//image_medium = image_mToken != null ? JObject.Parse(json)["data"]["labels"]["medium"].ToString() : "";
//image_large = image_lToken != null ? JObject.Parse(json)["data"]["labels"]["large"].ToString() : "";
//description = descriptionToken != null ? JObject.Parse(json)["data"]["description"].ToString() : "N/A";