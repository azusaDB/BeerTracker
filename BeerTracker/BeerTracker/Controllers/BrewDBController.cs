using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
//using ZenOfBeer.BreweryDb.Pcl.Public;
using System.Threading.Tasks;
using BeerTracker.Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using MongoDB.Driver;
using System.Configuration;
using MongoDB.Bson;
using MongoDB.Driver.Builders;

namespace BeerTracker.Controllers
{
    public class BrewDBController : ApiController
    {
        private string _address = "http://api.brewerydb.com/v2/";
        private string apiKey = "?key=cf17ecf24febe31afd664f4bd377a333";
        MongoDatabase mongoDatabase;
        List<Beer> beerList = new List<Beer>();

        private MongoDatabase RetreiveMongohqDb()
        {
            MongoUrl myMongoURL = new MongoUrl(ConfigurationManager.ConnectionStrings["MongoHQ"].ConnectionString);
            MongoClient mongoClient = new MongoClient(myMongoURL);
            MongoServer server = mongoClient.GetServer();
            return mongoClient.GetServer().GetDatabase("reddevteam");
        }

        //Gets success repsonse from BreweryDB
        public async Task<IEnumerable<string>> GetResponse()
        {
            var result = await ExternalResponse();
            return new string[] { result };
        }

        //Takes in a POST call from JS with an object ApiCall. With they type of api call you want to use.
        [HttpPost]
        public async Task<IEnumerable<string>> ApiRequest(ApiCall apiCall)
        {
            var result = await ExternalResponse(apiCall);
            return new string[] { result };
        }

        //Calls the Api
        private async Task<string> ExternalResponse(ApiCall apiCall)
        {
            var client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(_address + apiCall.call + apiKey);
            response.EnsureSuccessStatusCode();
            string result = await response.Content.ReadAsStringAsync();
            return result;
        }

        //Basic call to API -- For Testing
        private async Task<string> ExternalResponse()
        {
            var client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(_address);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return result;
        }

        [HttpPost]
        public Beer Save(Beer newBeer)
        {
            mongoDatabase = RetreiveMongohqDb();
            var beerList = mongoDatabase.GetCollection("BeerMaster");
            WriteConcernResult result;
            bool hasError = false;
                        try
            {
                newBeer.iconImage = newBeer.labels.icon;
                newBeer.medImage = newBeer.labels.medium;
                newBeer.lrgImage = newBeer.labels.large;
            }
            catch
            {
                newBeer.iconImage = "";
                newBeer.medImage = "";
                newBeer.lrgImage = "";
            }
            
            result = beerList.Insert<Beer>(newBeer);
            hasError = result.HasLastErrorMessage;

            if (!hasError)
            {
                return newBeer;
            }
            else
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        public IEnumerable<Beer> GetRndBeer()
        {
            mongoDatabase = RetreiveMongohqDb();
            try
            {
                var mongoList = mongoDatabase.GetCollection("BeerMaster").FindAll().AsEnumerable();
                beerList = (from b in mongoList
                           select new Beer
                           {
                               id = b["_id"].AsString,
                               name = b["name"].AsString,
                               medImage = b["medImage"].AsString,
                               abv = b["abv"].AsString
                           }).ToList();
                //.Where(x => x.medImage != "")
                Random rand = new Random();
                int toSkip = rand.Next(0, beerList.Count);
                beerList = beerList.Skip(toSkip).Take(10).ToList().OrderBy(x => x.name).ToList();
            }
            catch(Exception ex)
            {
                throw;
            }
            return beerList;
        }

        [HttpGet]
        public IEnumerable<Beer> GetFavBeer()
        {
            mongoDatabase = RetreiveMongohqDb();
            try
            {
                var mongoList = mongoDatabase.GetCollection("BeerMaster").FindAll().AsEnumerable();
                beerList = (from beverage in mongoList
                            select new Beer
                            {
                               id = beverage["_id"].AsString,
                               name = beverage["name"].AsString
                            }).ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
            return beerList;
        }



        public IHttpActionResult GetBeer(string id)
        {
            mongoDatabase = RetreiveMongohqDb();

            try
            {
                var mongoList = mongoDatabase.GetCollection("BeerSaved").FindAll().AsEnumerable();
                beerList = (from beverage in mongoList
                            select new Beer
                            {
                                id = beverage["id"].AsString,
                                name = beverage["name"].AsString,
                                description = beverage["description"].AsString,
                                abv = beverage["abv"].AsString,
                            }).ToList();
            }
            catch (Exception ex)
            {
                throw;
            }

            var beer = beerList.FirstOrDefault((p) => p.id == id);
            if (beer == null)
            {
                return NotFound();
            }
            return Ok(beer);
        }

    }
}
////Get only data json
//string beer = JObject.Parse(result)["data"].ToString();
////Fix up json so the deserializer can read it
//beer = beer.Replace("'", "");
//            beer = beer.Replace("\"", "'");
//            beer = beer.Replace("\r\n", "");
//            //string json = @"{'id':'9e2CN5','name':'Colorado Native Saison','nameDisplay':'Colorado Native Saison','description':'Saisons were originally farmhouse style ales produced in Wallonia, the French-speaking Southern area of Belgium. They were brewed by farmers in the winter, to last through the warmer summer months. These beers were intended to be provided to the farm workers.It is said that each worker was allowed up to five liters per day. Our Saison is brewed traditionally, as a toast to the Colorado farmers who provide us with the highest quality ingredients.Colorado white wheat and Pilsner malts are balanced with Western Slope hops and fermented with Belgian and French style yeasts from Colorado, to produce a light, fruity taste and a spicy aroma.','abv':'5','ibu':'25','glasswareId':8,'availableId':2,'styleId':72,'isOrganic':'N','labels':{'icon':'https:\/\/ s3.amazonaws.com\/ brewerydbapi\/ beer\/ 9e2CN5\/ upload_QKN6se - icon.png','medium':'https:\/\/ s3.amazonaws.com\/ brewerydbapi\/ beer\/ 9e2CN5\/ upload_QKN6se - medium.png','large':'https:\/\/ s3.amazonaws.com\/ brewerydbapi\/ beer\/ 9e2CN5\/ upload_QKN6se - large.png'},'status':'verified','statusDisplay':'Verified','originalGravity':'1.04','createDate':'2012 - 01 - 03 02:42:47','updateDate':'2015 - 12 - 16 08:49:37','glass':{'id':8,'name':'Tulip','createDate':'2012 - 01 - 03 02:41:33'},'available':{'id':2,'name':'Limited','description':'Limited availability.'},'style':{'id':72,'categoryId':5,'category':{'id':5,'name':'Belgian And French Origin Ales','createDate':'2012 - 03 - 21 20:06:46'},'name':'French & Belgian - Style Saison','shortName':'Saison','description':'Beers in this category are golden to deep amber in color.There may be quite a variety of characters within this style.Generally: They are light to medium in body.Malt aroma is low to medium - low.Esters are medium to high in  aroma, while, complex alcohols, herbs, spices, low Brettanomyces character and even clove and smoke - like phenolics may or may not be evident in the overall balanced beer. Hop aroma and flavor may be at low to medium levels.Malt flavor is low but provides foundation for the overall balance.Hop bitterness is moderate to moderately assertive.Herb and\/ or spice flavors, including black pepper - like notes, may or may not be evident.Fruitiness from fermentation is generally in character.A balanced small amount of sour or acidic flavors is acceptable when in balance with other components.Earthy, cellar - like, musty aromas are okay.Diacetyl should not be perceived.Chill or slight yeast haze is okay.Often bottle conditioned with some yeast character and high carbonation.French & Belgian - Style Saison may have Brettanomyces characters that are slightly acidity, fruity, horsey, goaty and\/ or leather - like.','ibuMin':'20','ibuMax':'40','abvMin':'4.5','abvMax':'8.5','srmMin':'4','srmMax':'14','ogMin':'1.055','fgMin':'1.004','fgMax':'1.016','createDate':'2012 - 03 - 21 20:06:46','updateDate':'2015 - 04 - 07 15:34:55'}}";
//            Beer NewBeer = new Beer();
//NewBeer = JsonConvert.DeserializeObject<Beer>(beer);