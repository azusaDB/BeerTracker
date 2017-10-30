using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
//using ZenOfBeer.BreweryDb.Pcl.Public;
using System.Threading.Tasks;
using BeerTracker.Models;

namespace BeerTracker.Controllers
{
    public class BrewDBController : ApiController
    {
        private string _address = "http://api.brewerydb.com/v2/";
        private string apiKey = "?key=cf17ecf24febe31afd664f4bd377a333";

        //Gets success repsonse from BreweryDB
        public async Task<IEnumerable<string>> GetResponse()
        {
            var result = await ExternalResponse();
            return new string[] { result };
        }


        //Takes in a POST call from JS with an object ApiCall. With they type of api call you want to use.
        [HttpPost]
        public async Task<IEnumerable<string>> GetRandomBeer(ApiCall apiCall)
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
            var result = await response.Content.ReadAsStringAsync();
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
    }
}
