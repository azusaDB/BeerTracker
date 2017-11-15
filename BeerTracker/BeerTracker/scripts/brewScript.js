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
    $('#showdata').empty();
    $('#showImage').empty();

    $(document).on("click", 'a', function (event) {
        var parm = $(this).attr("data-parm");  //Get the para from the attribute in the <a> tag
        $("#detailParmHere").html(parm); //set the hidden <p> to the parm
    });
});

$(document).on('pagebeforeshow', '#search', function () {
    $(document).on("click", '#submitSearch', function (event) {
        var searchCat = $("input[name*=search]:checked").val();
        var searchString = $("#searchInput").val();
        var id = "&q=" + searchString + "&type=" + searchCat;
        var apiCall = {
            call: "search",
            parameters: id
        };
        var li = "";
        $.ajax({
            url: brewUri + "/Search/" + apiCall,
            type: "POST",
            data: apiCall,
            async: false,
            success: function (data) {
                var searchResults = JSON.parse(data);
                $('#search-output').empty();
                $.each(searchResults.data, function (index, item) {
                    if (item.labels)
                    {
                        li += '<li><a data-transition="pop" data-parm=' + item.id + ' href="#details-page"><img src="' + item.labels.medium + '"><div hidden>' + item.name + '</div><h2>' + item.name + '</h2><p>ABV: ' + item.abv + '</p></a></li>';
                    }
                    else
                    {
                        li += '<li><a class="apiLi" data-transition="pop" data-parm=' + item.id + ' href="#details-page"><img src="https://brewmasons.co.uk/wp-content/uploads/2017/05/gold-10-247x300.jpg" width=150><div hidden>' + item.name + '</div><h2>' + item.name + '</h2><p>ABV: ' + item.abv + '</p></a ></li > ';
                    }
                    var beerJson = JSON.stringify(item);
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
                });
                $('#search-output').append(li);
                $('#search-output').listview().listview('refresh');

            },
            error: function () {
                $('#searchStatus').text("ERROR: Contact Caleb for support");
            }
        });
    });
    $(document).on("click", '.apiLi', function (event) {
        var parm = $(this).attr("data-parm");  //Get the para from the attribute in the <a> tag
        $("#apiParam").html(parm); //set the hidden <p> to the parm
    });
});

$(document).on('pagebeforeshow', '#details-page', function () {
    var Name;
    var Desc;
    var ABV;
    var breweryName = "N/A";
    var breweryUrl = "N/A";
    var lrgImage;
    var id = $('#detailParmHere').text();

    $.getJSON(brewUri + "/GetBrewery/" + id)
        .done(function (data) {
            //******This if statment can be removed*******/
            if (id == data.id) {
                Name = "<b>Beer Name: </b><br />" + data.name; 
                Desc = "<b>Beer Description: </b><br />" + data.Desc;
                ABV = "<b>Beer ABV: </b><br />" + data.abv;
                breweryName = "<b>Brewery Name: </b><br />" + data.breweryName;
                //breweryUrl = "<b>Brewery Url: </b><br />" + $('#showdata').click(function () { data.breweryUrl });
                breweryUrl = "<b>Brewery Url: </b><br />" + data.breweryUrl;
                lrgImage = $('#showImage').attr("src", data.lrgImage);

                $('#showdata').append(Name).append('<br />');
                $('#showdata').append(Desc).append('<br />');
                $('#showdata').append(ABV).append('<br />');
                $('#showdata').append(breweryName).append('<br />');
                $('#showdata').append(breweryUrl).append('<br />');
                $('#showImage').append(lrgImage).append('<br />');

                empty();
            }
           
            //**********************************************
        });

    //NOW CALL GETBEER TO GET ALL DATA NEEDED TO SHOW ALL BEER DETAILS
    //$.getJSON(brewUri + "/GetBeer")
//        .done(function (data) {
//            $.each(data, function (index, record) {
//                if (id == record.id) {
//                    Name = "Name: " + record.name;
//                    Desc = " Description: " + record.description;
//                    ABV = " ABV: " + record.abv;
//                    $('#showdata').text(Name).append('<br />');;
//                    $('#showdata').append(Desc).append('<br />');;
//                    $('#showdata').append(ABV);
//                }
//            });
//        });
});

$(document).on('pagebeforeshow', '#signup', function () {
    $(document).on("click", '#submitSignUp', function (event) {
        var password = $("#password").val();
        var username = $("#username").val();

        $.ajax({
            url: brewUri + "/SignUp?username=" + username + "&password=" + password,
            type: "POST",
            async: false,
            success: function (data) {

            },
            error: function () {
                $('#searchStatus').text("ERROR: Contact Caleb for support");
            }
        });
    });
    $(document).on("click", '.apiLi', function (event) {
        var parm = $(this).attr("data-parm");  //Get the para from the attribute in the <a> tag
        $("#apiParam").html(parm); //set the hidden <p> to the parm
    });
});

$(document).on('pagebeforeshow', '#signin', function () {
    $(document).on("click", '#SignInSubmit', function (event) {
        var username = $("#loginusername").val();
        var password = $("#loginpsw").val();

        $.ajax({
            url: brewUri + "/SignIn?username=" + username + "&password=" + password,
            type: "POST",
            async: false,
            success: function (data) {

            },
            error: function () {
                $('#SignInStatus').text("Sign In ERROR!");
            }
        });
    });
    $(document).on("click", '.apiLi', function (event) {
        var parm = $(this).attr("data-parm");  //Get the para from the attribute in the <a> tag
        $("#apiParam").html(parm); //set the hidden <p> to the parm
    });
});






