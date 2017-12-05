using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using BeerTracker.Models;
using BeerTracker.Controllers;
using System.Web.Http;
using System.Web.Http.Results;
using System.Net.Http;
using System.Net;
using BeerUsers.Models;

namespace BeerTracker.Tests
{
    [TestClass]
    public class TestBrewDBController
    {
        List<Beer> beerList = new List<Beer>();
        List<AddBeer> addBeerList = new List<AddBeer>();
        private List<Beer> GenerateFakeBeerList()
        {
            List<Beer> workingList = new List<Beer>();
            for (int i = 0; i < 3; i++)
            {
                string index = i.ToString();
                Beer nextBeer = new Beer();
                nextBeer.id = index;
                nextBeer.name = "Test " + index;
                workingList.Add(nextBeer);
            }
            return workingList;
        }
        private List<AddBeer> GenerateFakeAddBeerList()
        {
            List<AddBeer> workingList = new List<AddBeer>();
            for (int i = 0; i < 3; i++)
            {
                string index = i.ToString();
                AddBeer nextBeer = new AddBeer();
                nextBeer.Id = index;
                nextBeer.name = "Test " + index;
                workingList.Add(nextBeer);
            }
            return workingList;
        }
        [TestMethod]
        public void SaveSingleBeer_FakeBeer_ByBeerObj()
        {
            List<Beer> testBeers = GenerateFakeBeerList();
            Beer beerToAdd = new Beer();
            beerToAdd.name = "Test Details";
            beerToAdd.id = 2.ToString();
            var controller = new BrewDBController(testBeers);

            Beer result = controller.Save(beerToAdd);

            Assert.AreEqual(beerToAdd, result);
        }

        [TestMethod]
        public void SaveSingleBeer_FakeBeer_ByBeerId()
        {
            List<Beer> testBeers = GenerateFakeBeerList();
            Beer beerToAdd = new Beer();
            beerToAdd.name = "Test Details";
            beerToAdd.id = 2.ToString();
            var controller = new BrewDBController(testBeers);

            Beer result = controller.Save("2");

            Assert.AreEqual(beerToAdd.name, result.name);
        }

        [TestMethod]
        public void GetRndBeerList_Fake()
        {
            List<Beer> testBeers = GenerateFakeBeerList();
            var controller = new BrewDBController(testBeers);
            List<Beer> rndBeers = (List<Beer>)controller.GetRndBeer();
            Assert.AreEqual(testBeers.Count, rndBeers.Count);
        }

        [TestMethod]
        public void GetUserTriedBeers_Mongo()
        {
            var controller = new BrewDBController();
            User user = new User
            {
                uid = "tester"
            };
            beerList = (List<Beer>)controller.GetTriedBeer(user);
            Assert.AreEqual(beerList.Count, 3);
        }
        [TestMethod]
        public void GetUserWishListBeers_Mongo()
        {
            var controller = new BrewDBController();
            User user = new User
            {
                uid = "tester"
            };
            beerList = (List<Beer>)controller.GetWishList(user);
            Assert.AreEqual(beerList.Count, 3);
        }

        [TestMethod]
        public void AddNewBeer_Fake()
        {
            List<AddBeer> testBeers = GenerateFakeAddBeerList();
            AddBeer beerToAdd = new AddBeer();
            beerToAdd.name = "Test Details";
            beerToAdd.Id = 2.ToString();
            var controller = new BrewDBController(testBeers);
            IHttpActionResult result = controller.AddNewBeer(beerToAdd);
            Assert.IsInstanceOfType(result, typeof(OkResult));
        }

        [TestMethod]
        public void GetBreweryDetails()
        {
            Beer beer = new Beer();
            var controller = new BrewDBController();
            IHttpActionResult result = controller.GetBrewery("1TLb7b");
            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<Beer>));
        }

        [TestMethod]
        public void SearchBeer()
        {
            ApiCall apiCall = new ApiCall
            {
                call = "search",
                parameters = "&q=trickster&type=beer"
            };
            var controller = new BrewDBController();

            IHttpActionResult result = controller.Search(apiCall);
            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<string>));
        }

        [TestMethod]
        public void SearchBrewery()
        {
            ApiCall apiCall = new ApiCall
            {
                call = "search",
                parameters = "&q=coors&type=brewery"
            };
            var controller = new BrewDBController();

            IHttpActionResult result = controller.Search(apiCall);
            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<string>));
        }

        [TestMethod]
        public void SignUpUser()
        {
            User user = new User
            {
                uid = "tester",
                password = "bcuser17"
            };
            var controller = new BrewDBController();
            IHttpActionResult result = controller.SignUp(user);
            Assert.IsInstanceOfType(result, typeof(OkResult));
        }
        [TestMethod]
        public void SignInUser()
        {
            User user = new User
            {
                uid = "tester",
                password = "bcuser17"
            };
            var controller = new BrewDBController();
            IHttpActionResult result = controller.SignIn(user);
            Assert.IsInstanceOfType(result, typeof(OkResult));
        }

        [TestMethod]
        public void SaveTriedBeer()
        {
            User user = new User
            {
                uid = "saveTester2",
                bid = "1TLb7b"
            };
            var controller = new BrewDBController();
            var result = controller.SaveTriedBeer(user);
            Assert.IsInstanceOfType(result, typeof(OkResult));
        }

        [TestMethod]
        public void SaveWishListBeer()
        {
            User user = new User
            {
                uid = "saveTester2",
                bid = "1TLb7b"
            };
            var controller = new BrewDBController();
            var result = controller.saveToWishList(user);
            Assert.IsInstanceOfType(result, typeof(OkResult));
        }

    }
}
