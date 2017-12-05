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
    }
}
