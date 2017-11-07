//REAL API 
var brewUri = 'api/BrewDB';
//Disabled API
//var brewUri = 'disabled';

$(document).ready(function () {
    randomBeer();
    homePageList();
    favList();
});

function homePageList() {
    //Currently gets all the beer in the BeerMaster and displays them to the homepage
    $.getJSON(brewUri + "/GetRndBeer")
        .done(function (data) {
            // On success, 'data' contains a list of products.
            $.each(data, function (key, item) {
                // Add a list item for the product.
                // Change the way to format the string(Sunny)
                $('#output').append('<li><a data-transition="pop" data-parm=' + item.id + ' href="#details-page"><div hidden>' + item.name + '</div>' + item.name + '</a></li>');
                // Listview refresh after each inner loop(Sunny)
                $("#output").listview("refresh");
            });
        });
}

function favList() {
    //Displays items appended to mongo 'BeerSaved' to favorites list 
    $.getJSON(brewUri + "/GetFavBeer")
        .done(function (data) {
            // On success, 'data' contains a list of products.
            $.each(data, function (key, item) {
                // Add a list item for the product.
                // Change the way to format the string(Sunny)
                $('#outputFavList').append('<li><a data-transition="pop" data-parm=' + item.id + ' href="#details-page"><div hidden>' + item.name + '</div>' + item.name + '</a></li>');
                // Listview refresh after each inner loop(Sunny)
                $("#outputFavList").listview("refresh");
            });
        });
}

function randomBeer() {
    //Create By: Caleb
    //On page load, this method will call BreweryDB to get a random beer.
    var apiCall = {
        call: "beer/random"
    };

    $.ajax({
        url: brewUri + "/ApiRequest/" + apiCall,
        type: "POST",
        data: apiCall,
        async: false,
        success: function (data) {
            var rndBeer = JSON.parse(data);
            var foo = JSON.stringify(rndBeer, null, 4);
            //rndBeer is the beer object and data is the raw json string
            document.getElementById("output").innerHTML = JSON.stringify(rndBeer, null, 4);
            var beerJson = JSON.stringify(rndBeer.data);
            //Save random beer to DB
            $.ajax({
                url: "api/BrewDB/Save",
                type: "POST",
                contentType: "application/json",
                data: beerJson,
                async: false,
                success: function (data) {
                    document.getElementById("output").innerHTML = "SUCCESS MESSAGE: " + data.name + " saved to BeerMaster";
                },
                error: function () {
                    $('#output').text("Error: Save Failed");
                }
            });

        },
        error: function () {
            $('#output').text("ERROR: API has been disabled to avoid going over our api request limit. Change brewUri back to api/BrewDB to call api.");
        }
    });
}

function searchBrew() {
    var brewText = $('#brewSearchText').val();

    $.getJSON(brewUri)
        .done(function (data) {
            var foo = JSON.parse(data);
            $('#output').text(foo.data);
        });
}

function test() {

}