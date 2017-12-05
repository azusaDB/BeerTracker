using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using BeerTracker.Models;
using BeerTracker.Controllers;
using System.Web.Http;
using System.Web.Http.Results;
using System.Net.Http;
using System.Net;

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
            List<Beer> testNotes = GenerateFakeBeerList();
            Beer beerToAdd = new Beer();
            beerToAdd.name = "Test Details";
            beerToAdd.id = 2.ToString();
            var controller = new BrewDBController(testNotes);

            Beer result = controller.Save(beerToAdd);

            Assert.AreEqual(beerToAdd, result);
        }

        [TestMethod]
        public void GetSingleBeer_FakeBeer_ByBeerId()
        {
            List<Beer> testNotes = GenerateFakeBeerList();
            Beer beerToAdd = new Beer();
            beerToAdd.name = "Test Details";
            beerToAdd.id = 2.ToString();
            var controller = new BrewDBController(testNotes);

            Beer result = controller.Save("2");

            Assert.AreEqual(beerToAdd.name, result.name);
        }
    }
}
