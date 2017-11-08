using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using MongoDB.Bson.Serialization.Attributes;

namespace BeerTracker.Models
{
    public class Beer
    {
        [BsonId]
        public string id { get; set; }
        public string name { get; set; }
        public string nameDisplay { get; set; }
        public string description { get; set; } = "";
        public string abv { get; set; } = "N/A";
        public string ibu { get; set; }
        public int glasswareId { get; set; }
        public int availableId { get; set; }
        public int styleId { get; set; }
        public string isOrganic { get; set; }
        public Label labels { get; set; }
        public string iconImage { get; set; } = "";
        public string medImage { get; set; } = "";
        public string lrgImage { get; set; } = "";
        public string status { get; set; }
        public string statusDisplay { get; set; }
        public string originalGravity { get; set; }
        public string createDate { get; set; }
        public string updateDate { get; set; }
        public Glass glass { get; set; }
        public Available available { get; set; }
        public Style style { get; set; }
    }

    public class Label
    {
        public string icon { get; set; }
        public string medium { get; set; }
        public string large { get; set; }
    }

    public class Glass
    {
        public int id { get; set; }
        public string name { get; set; }
        public string createDate { get; set; }
    }

    public class Available
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
    }

    public class Style
    {
        public int id { get; set; }
        public int categoryId { get; set; }
        public Category category { get; set; }
        public string name { get; set; }
        public string shortName { get; set; }
        public string description { get; set; }
        public string ibuMin { get; set; }
        public string ibuMax { get; set; }
        public string abvMin { get; set; }
        public string abvMax { get; set; }
        public string srmMin { get; set; }
        public string srmMax { get; set; }
        public string ogMin { get; set; }
        public string fgMin { get; set; }
        public string fgMax { get; set; }
        public string createDate { get; set; }
        public string updateDate { get; set; }
    }

    public class Category
    {
        public int id { get; set; }
        public string name { get; set; }
        public string createDate { get; set; }
    }

}

    //public class Beer
    //{
    //    public string id { get; set; }
    //    public string name { get; set; }
    //    public string shortName { get; set; }
    //    public double abv { get; set; }
    //    public int breweryId { get; set; } //known as styleId
    //    public string breweryName { get; set; }
    //    public string image_medium { get; set; }
    //    public string image_large { get; set; }
    //    public string description { get; set; }
    //    public double ibu { get; set; }

    //    public Beer(string json)
    //    {
    //        JObject beerObject = JObject.Parse(json);
    //        try
    //        {
    //            id = VerifyDataExist("id", beerObject) ? beerObject["data"]["id"].ToString() : "N/A";
    //            name = VerifyDataExist("name", beerObject) ? beerObject["data"]["name"].ToString() : "N/A";
    //            shortName = VerifyData2Exist("style", "shortName", beerObject) ? beerObject["data"]["style"]["shortName"].ToString() : "N/A";
    //            abv = VerifyDataExist("abv", beerObject) ? Convert.ToDouble(beerObject["data"]["abv"]) : 0.0;
    //            breweryId = VerifyDataExist("styleId", beerObject) ? Convert.ToInt32(beerObject["data"]["styleId"]) : 0;
    //            breweryName = VerifyData3Exist("style", "category", "name", beerObject) ? beerObject["data"]["style"]["category"]["name"].ToString() : "N/A";
    //            image_medium = VerifyData2Exist("labels", "medium", beerObject) ? beerObject["data"]["labels"]["medium"].ToString() : "";
    //            image_large = VerifyData2Exist("labels", "large", beerObject) ? beerObject["data"]["labels"]["large"].ToString() : "";
    //            description = VerifyDataExist("description", beerObject) ? beerObject["data"]["description"].ToString() : "N/A";
    //        }
    //        catch (Exception ex)
    //        {
    //            string err = ex.ToString();
    //        }
    //    }

    //    private bool VerifyDataExist(string field, JObject beerObject)
    //    {
    //        string test;
    //        try
    //        {
    //            test = !String.IsNullOrEmpty(beerObject["data"][field].ToString()) ? "pass" : "fail";
    //            return true;
    //        }
    //        catch
    //        {
    //            return false;
    //        }
    //    }
    //    private bool VerifyData2Exist(string field1, string field2, JObject beerObject)
    //    {
    //        string test;
    //        try
    //        {
    //            test = !String.IsNullOrEmpty(beerObject["data"][field1][field2].ToString()) ? "pass" : "fail";
    //            return true;
    //        }
    //        catch
    //        {
    //            return false;
    //        }
    //    }
    //    private bool VerifyData3Exist(string field1, string field2, string field3, JObject beerObject)
    //    {
    //        string test;
    //        try
    //        {
    //            test = !String.IsNullOrEmpty(beerObject["data"][field1][field2][field3].ToString()) ? "pass" : "fail";
    //            return true;
    //        }
    //        catch
    //        {
    //            return false;
    //        }
    //    }
    //}