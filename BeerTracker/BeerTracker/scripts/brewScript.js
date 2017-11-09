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
                //$('#output').append('<li><a data-transition="pop" data-parm=' + item.id + ' href="#details-page?id=' + item.id + '"><div hidden>' + item.name + '</div>' + item.name + '</a></li>');
                if (item.medImage) {
                    $('#output').append('<li><a data-transition="pop" data-parm=' + item.id + ' href="#details-page"><img src="' + item.medImage + '"><div hidden>' + item.name + '</div><h2>' + item.name + '</h2><p>ABV: ' + item.abv + '</p></a></li>');
                } else {
                    $('#output').append('<li><a data-transition="pop" data-parm=' + item.id + ' href="#details-page"><img src="https://brewmasons.co.uk/wp-content/uploads/2017/05/gold-10-247x300.jpg" width=150><div hidden>' + item.name + '</div><h2>' + item.name + '</h2><p>ABV: ' + item.abv + '</p></a ></li > ');
                }
                // Listview refresh after each inner loop(Sunny)
                $('#output').listview().listview('refresh');
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
                $("#outputFavList").listview().listview("refresh");
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


$(document).on('pagebeforeshow', '#indexpage', function () {
    //changed the onclick event. It used to look like $('a').on("click", function).......
    $(document).on("click", 'a', function (event) {
        var parm = $(this).attr("data-parm");  //Get the para from the attribute in the <a> tag
        $("#detailParmHere").html(parm); //set the hidden <p> to the parm
    });
});

$(document).on('pagebeforeshow', '#details-page', function () {
    var Name;
    var Desc;
    var ABV;
    var id = $('#detailParmHere').text();
    $.getJSON(brewUri + "/GetBeer")
        .done(function (data) {
            $.each(data, function (index, record) {
                if (id == record.id) {
                    Name = "Name: " + record.name;
                    Desc = " Description: " + record.description;
                    ABV = " ABV: " + record.abv;
                    $('#showdata').text(Name).append('<br />');;
                    $('#showdata').append(Desc).append('<br />');;
                    $('#showdata').append(ABV);
                }
            });
        });
});