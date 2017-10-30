using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BeerTracker.Models
{
    public class Beer
    {
            public string id { get; set; }
            public string name { get; set; }
            public string shortName { get; set; }
            public decimal abv { get; set; }
            public int breweryId { get; set; } //known as styleId
            public string breweryName { get; set; }
            public string image_small { get; set; }
            public string image_large { get; set; }
            public string description { get; set; }
            public decimal ibu { get; set; }
    }
}