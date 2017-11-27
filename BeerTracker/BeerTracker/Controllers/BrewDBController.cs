using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
//using ZenOfBeer.BreweryDb.Pcl.Public;
using System.Threading.Tasks;
using BeerTracker.Models;
using BeerUsers.Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using MongoDB.Driver;
using System.Configuration;
using MongoDB.Bson;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;

namespace BeerTracker.Controllers
{
    public class BrewDBController : ApiController
    {
        private string _address = "http://api.brewerydb.com/v2/";
        private string apiKey = "?key=cf17ecf24febe31afd664f4bd377a333";
        MongoDatabase mongoDatabase;
        List<Beer> beerList = new List<Beer>();
        private object userList;
        private object symbolcollection;

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
        public async Task<string> ApiRequest(ApiCall apiCall)
        {
            //var result = await ExternalResponse(apiCall);
            using (var client = new HttpClient())
            {
                return client.GetStringAsync(_address + apiCall.call + apiKey + apiCall.parameters).Result;
            }
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
            Beer foo = new Beer();
            var beerMasterList = mongoDatabase.GetCollection("BeerMaster");
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
            //***********************Make sure beer does already exist in mongo
            try
            {
                var mongoList = mongoDatabase.GetCollection("BeerMaster").FindAll().AsEnumerable();
                foo = (from beverage in mongoList
                       select new Beer
                       {
                           id = beverage["_id"].AsString,
                       }).Where(b => b.id == newBeer.id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw;
            }

            //var beer = beerList.FirstOrDefault((p) => p.id == newBeer.id);
            //*************************
            if (foo == null) //if it doesn't exist save it to mongo
            {
                result = beerMasterList.Insert<Beer>(newBeer);
                hasError = result.HasLastErrorMessage;
            }

            if (!hasError)
            {
                return newBeer;
            }
            else
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        public Beer Save(string id)
        {
            mongoDatabase = RetreiveMongohqDb();
            Beer beer = new Beer();
            var beerMasterList = mongoDatabase.GetCollection("BeerMaster");

            try
            {
                var mongoList = mongoDatabase.GetCollection("BeerMaster").FindAll().AsEnumerable();
                beer = (from beverage in mongoList
                        select new Beer
                        {
                            id = beverage["_id"].AsString,
                        }).Where(b => b.id == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            if (beer == null)
            {
                ApiCall apiCall = new ApiCall
                {
                    call = "beer/" + id
                };
                var result = ApiRequest(apiCall);
                string beerJson = FormatJson(JObject.Parse(result.Result).ToString());
                beer = JsonConvert.DeserializeObject<Beer>(JObject.Parse(beerJson)["data"].ToString());
                //JsonConvert.DeserializeObject<Beer>(beerJson);
                //beer = JsonConvert.DeserializeObject<Beer>(
                Save(beer);
                return beer;
            }
            else
            {
                return beer;
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
                                iconImage = b["iconImage"].AsString,
                                abv = b["abv"].AsString
                            }).ToList();
                //.Where(x => x.medImage != "")
                Random rand = new Random();
                int toSkip = rand.Next(0, beerList.Count);
                beerList = beerList.Skip(toSkip).Take(10).ToList().OrderBy(x => x.name).ToList();
            }
            catch (Exception ex)
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

        [HttpPost]
        public IHttpActionResult AddNewBeer(AddBeer newBeer)
        {
            mongoDatabase = RetreiveMongohqDb();
            var newBeerList = mongoDatabase.GetCollection("AddBeer");
            WriteConcernResult result;
            bool hasError = false;
            try
            {
                if (string.IsNullOrEmpty(newBeer.Id))
                {
                    newBeer.Id = ObjectId.GenerateNewId().ToString();
                    result = newBeerList.Insert<AddBeer>(newBeer);
                }
                else {
                    result = newBeerList.Insert<AddBeer>(newBeer);
                    hasError = result.HasLastErrorMessage;
                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Ok();
        }

        [HttpGet]
        public IHttpActionResult GetBrewery(string id)
        {
            mongoDatabase = RetreiveMongohqDb();
            WriteConcernResult writeResult;
            var mongoList = mongoDatabase.GetCollection("BeerMaster").FindAll().AsEnumerable();

            try
            {
                beerList = (from beverage in mongoList
                            select new Beer
                            {
                                id = beverage["_id"].AsString,
                                name = beverage["name"].AsString,
                                description = beverage["description"].AsString,
                                abv = beverage["abv"].AsString,
                                breweryName = beverage["breweryName"].AsString,
                                breweryUrl = beverage["breweryUrl"].AsString,
                                lrgImage = beverage["lrgImage"].AsString,
                                medImage = beverage["medImage"].AsString
                            }).ToList();
            }
            catch (Exception ex)
            {
                throw;
            }

            var beer = beerList.FirstOrDefault((b) => b.id == id);
            //If beer already has breweryName and breweryUrl no need to call the api just return the beer
            if (!String.IsNullOrEmpty(beer.breweryName) || !String.IsNullOrEmpty(beer.breweryUrl))
            {
                return Ok(beer);
            }
            else
            {
                var brewList = mongoDatabase.GetCollection("BeerMaster");
                ApiCall apiCall = new ApiCall
                {
                    call = "beer/" + id + "/breweries"
                };
                var result = ApiRequest(apiCall);
                string breweryData = FormatJson(JObject.Parse(result.Result).ToString());

                var obj = JObject.Parse(breweryData);
                var brewName = (string)obj["data"][0]["name"];
                var brewUrl = (string)obj["data"][0]["website"];

                //Update Mongo
                IMongoQuery query = Query.EQ("_id", id);
                IMongoUpdate update = Update
                    .Set("breweryName", brewName)
                    .Set("breweryUrl", brewUrl);
                writeResult = brewList.Update(query, update);

                //After update send updated beer to the client
                beer.breweryName = brewName;
                beer.breweryUrl = brewUrl;

                return Ok(beer);
            }

        }

        [HttpPost]
        public IHttpActionResult Search(ApiCall apiCall)
        {
            var result = ApiRequest(apiCall);
            string searchData = JObject.Parse(result.Result).ToString();

            return Ok(searchData);
        }

        private string FormatJson(string json)
        {
            json = json.Replace("'", "");
            json = json.Replace("\"", "'");
            json = json.Replace("\r\n", "");
            return json;
        }

        [HttpPost]
        public IHttpActionResult SignUp(User user)
        {
            mongoDatabase = RetreiveMongohqDb();
            var userList = mongoDatabase.GetCollection("BeerUser");
            User localUser = new BeerUsers.Models.User();
            WriteConcernResult result;
            bool hasError = false;
            try
            {
                var mongoList = mongoDatabase.GetCollection("BeerUser").FindAll().AsEnumerable();
                localUser = (from u in mongoList
                              select new User
                              {
                                  uid = u["_id"].AsString,
                                  password = u["password"].AsString
                              }).Where(b => b.uid == user.uid).FirstOrDefault();

                if (localUser != null)
                {
                    return Content(HttpStatusCode.Forbidden, "Error: Username already exists");
                }
                else
                {
                    result = userList.Insert<User>(user);
                    hasError = result.HasLastErrorMessage;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Ok(user);
        }

        [HttpPost]
        public IHttpActionResult SignIn(User user)
        {
            mongoDatabase = RetreiveMongohqDb();
            User signinUser = new User();
            var userList = mongoDatabase.GetCollection("BeerUser");

            try
            {
                var mongoList = mongoDatabase.GetCollection("BeerUser").FindAll().AsEnumerable();
                signinUser = (from u in mongoList
                        select new User
                        {
                            uid = u["_id"].AsString,
                            password = u["password"].AsString
                        }).Where(b => b.uid == user.uid).FirstOrDefault();

                if (signinUser!=null)
                {
                    if (signinUser.password == user.password)
                    {
                        return Ok(user);
                    } else
                    {
                        return Content(HttpStatusCode.BadRequest, "Password does not match");
                    }
                } else
                {
                    return Content(HttpStatusCode.BadRequest, "User does not exist");
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

        }

        [HttpPost]
        public IHttpActionResult SignOut()
        {
            return Ok();
        }

        [HttpPost]
        public IHttpActionResult SaveTriedBeer(User user)
        {
            mongoDatabase = RetreiveMongohqDb();
            UserBeer beenSaved = new UserBeer();
            Beer beer = new Beer();
            WriteConcernResult result;
            var triedBeerList = mongoDatabase.GetCollection("UserTriedBeer");

            try
            {
                var mongoList = mongoDatabase.GetCollection("UserTriedBeer").FindAll().AsEnumerable();
                beenSaved = (from beverage in mongoList
                        select new UserBeer
                        {
                            beerId = beverage["beerId"].AsString,
                            username = beverage["username"].AsString
                        }).Where(b => b.beerId == user.bid && b.username == user.uid).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            if(beenSaved == null)
            {
                try
                {
                    var mongoList = mongoDatabase.GetCollection("BeerMaster").FindAll().AsEnumerable();
                    beer = (from beverage in mongoList
                                 select new Beer
                                 {
                                     id = beverage["_id"].AsString
                                 }).Where(b => b.id == user.bid).FirstOrDefault();
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                if(beer != null)
                {
                    UserBeer toBeSaved = new UserBeer(user.bid, user.uid);
                    result = triedBeerList.Insert<UserBeer>(toBeSaved);
                }
                return Ok();
            }

            return NotFound();
        }
        [HttpPost]
        public IHttpActionResult saveToWishList(User user)
        {
            mongoDatabase = RetreiveMongohqDb();
            UserBeer beenSaved = new UserBeer();
            Beer beer = new Beer();
            WriteConcernResult result;
            var wishList = mongoDatabase.GetCollection("BeerWishList");

            try
            {
                var mongoList = mongoDatabase.GetCollection("BeerWishList").FindAll().AsEnumerable();
                beenSaved = (from beverage in mongoList
                             select new UserBeer
                             {
                                 beerId = beverage["beerId"].AsString,
                                 username = beverage["username"].AsString
                             }).Where(b => b.beerId == user.bid && b.username == user.uid).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            if (beenSaved == null)
            {
                try
                {
                    var mongoList = mongoDatabase.GetCollection("BeerMaster").FindAll().AsEnumerable();
                    beer = (from beverage in mongoList
                            select new Beer
                            {
                                id = beverage["_id"].AsString
                            }).Where(b => b.id == user.bid).FirstOrDefault();
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                if (beer != null)
                {
                    UserBeer toBeSaved = new UserBeer(user.bid, user.uid);
                    result = wishList.Insert<UserBeer>(toBeSaved);
                }
                return Ok();
            }

            return NotFound();
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