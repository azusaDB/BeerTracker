//REAL API 
var brewUri;
var testingResult;
var testing = true;
if (testing)
    brewUri = '../api/BrewDB'
else
    brewUri = 'api/BrewDB';
//Disabled API
//var brewUri = 'disabled';

$(document).ready(function () {
    if (!testing) {
        //randomBeer();
        homePageList();
        //favList();
    }
});

$(document).on('pagebeforeshow', '#add-page', function () {
    var localuser = sessionStorage.getItem('userSession');
    document.getElementById('addbeer-content').style.visibility = "hidden";
    if (localuser) {
        document.getElementById('addbeer-content').style.visibility = "visible";
    } else {
        $.mobile.changePage("#signin");
    }
    
    $(document).on("click", '#submitNewBeer', function (event) {
        var beerObj = {
            name: $("#Name").val(),
            ABV: $("#ABV").val(),
            Description: $("#Desc").val(),
            Brewery: $("#Brewery").val(),
            Url: $("#Url").val(),
            Image: $("#Image").val()
        };

        addBeer(beerObj);

    });
});
$(document).on('pagebeforeshow', '#details-page', function () {

    var id = $('#detailParmHere').text();
    var localuser = sessionStorage.getItem('userSession');

    displayDetails(id);

    var saveRequest = {
        uid: localuser,
        bid: id
    };
    $(document).on("click", '#saveToBeerSave', function (event) {
        if (localuser) {
            saveToTried(saveRequest);

        } else {
            $.mobile.changePage("#signin");
        }
    });
    $(document).on("click", '#saveToWishList', function (event) {
        if (localuser) {
            saveToWishlist(saveRequest);

        } else {
            $.mobile.changePage("#signin");
        }
    });
});
$(document).on('pagebeforeshow', '#favorites-page', function () {
    document.getElementById('fav-content').style.visibility = "hidden";
    $("saveResponseLable").empty();
    $("#outputFavList").empty();
    var localuser = sessionStorage.getItem('userSession');

    if (localuser) {
        var userObj = {
            uid: localuser
        };
        getTriedBeers(userObj);

        document.getElementById('fav-content').style.visibility = "visible";
    } else {
        $.mobile.changePage("#signin");
    }


});
$(document).on('pagebeforeshow', '#indexpage', function () {
    var localuser = sessionStorage.getItem('userSession');
    var msg = sessionStorage.getItem('userSessionMsg');
    document.getElementById('adminUnitTest').style.visibility = 'hidden';
    if (localuser || testing)
    {
        if (localuser == 'admin' || testing)
        {
            document.getElementById('adminUnitTest').style.visibility = 'visible';
        }
        if (localuser)
        {
            $('#userSession').text(localuser);
            $('#userSessionMsg').text(msg);
        }
    }
    else
    {
        $('#userSession').text("");
        $('#userSessionMsg').text("");
    }
    
    //changed the onclick event. It used to look like $('a').on("click", function).......
    $(document).on("click", 'a', function (event) {
        var parm = $(this).attr("data-parm");  //Get the para from the attribute in the <a> tag
        $("#detailParmHere").html(parm); //set the hidden <p> to the parm
    });
    $(document).keypress(function (e) {
        if (e.which == 13)
        {
            e.preventDefault();
            $("#submitSearch").click();
        }
    });

    $(document).on("click", '#submitSearch', function (event) {
        $('#searchStatus').text("");
        var searchCat = $("input[name*=search]:checked").val();
        var searchString = $("#searchInput").val();
        var id = "&q=" + searchString + "&type=" + searchCat;
        var apiCall = {
            call: "search",
            parameters: id
        };
        
        if (searchString)
        {
            search(apiCall, searchCat);
        }
        else 
        {
            $('#searchStatus').text("No Results Found");
        }
    });
    $(document).on("click", '.apiLi', function (event) {
        var parm = $(this).attr("data-parm");  //Get the para from the attribute in the <a> tag
        $("#apiParam").html(parm); //set the hidden <p> to the parm
    });
});
$(document).on('pagebeforeshow', '#myprofile', function () {
    var localuser = sessionStorage.getItem('userSession');
    $(document).on("click", '#SignOutSubmit', function (event) {
        signOut();
    });
    if (localuser) {
        $("#profileName").text("Welcome: " + localuser);
    } else {
        $.mobile.changePage("#signin");
    }
});
$(document).on('pagebeforeshow', '#signin', function () {
    $(document).on("click", '#SignInSubmit', function (event) {
        var uid = $("#signin-username").val();
        var password = $("#signin-password").val();
        var userObj = {
            uid: uid,
            password: password
        };
        signIn(userObj);

    });
});
$(document).on('pagebeforeshow', '#signup', function () {

    $("#form-signup").validate({
        rules: {
            password: {
                required: true,
                minlength: 6,
                maxlength: 10,

            },

            cfmPassword: {
                equalTo: "#password",
                minlength: 6,
                maxlength: 10
            }
        },

        messages: {
            password: {
                required: "Password is required."

            }
        }

    });

    $(document).on("click", '#submitSignUp', function (event) {
        $('#signupError').empty();
        var password = $("#password").val();
        var username = $("#username").val();

        var userObj = {
            uid: username,
            password: password
        };
        signUp(userObj);

    });
    $(document).on("click", '.apiLi', function (event) {
        var parm = $(this).attr("data-parm");  //Get the para from the attribute in the <a> tag
        $("#apiParam").html(parm); //set the hidden <p> to the parm
    });
});
$(document).on('pagebeforeshow', '#wishlist-page', function () {
    $("saveResponseLable").empty();
    $("#outputWishList").empty();
    document.getElementById('wishlist-content').style.visibility = "hidden";
    var localuser = sessionStorage.getItem('userSession');
    if (localuser) {
        document.getElementById('wishlist-content').style.visibility = "visible";
        var userObj = {
            uid: localuser
        };
        getWishlistBeers(userObj);
    } else {
        $.mobile.changePage("#signin");
    }

});

function addBeer(beerObj) {
    
    $.ajax({
        url: brewUri + "/AddNewBeer/" + beerObj,
        type: "POST",
        async: false,
        data: beerObj,
        success: function (data) {
            if (testing)
                testingResult = data;
            else
                $('#saveResponse').text("Success: Saved Beer");
        },
        error: function (data) {
            if (testing)
                testingResult = data;
            else
                $('#saveResponse').text("Error: Save Failed");
        }
    });
    if (testing)
        return testingResult;
};
function displayDetails(id) {
    $("#saveResponseLable").empty();
    $('#showdata').empty();
    $('#showImage').attr("src", "");
    $('#beerName').empty();
    $('#beerDescription').empty();
    $('#beerABV').empty();
    $('#breweryName').empty();
    $('#beerUrlText').empty();
    $('#beerUrl').attr("href", "");
    var Name;
    var Desc = "N/A";
    var ABV;
    var breweryName = "N/A";
    var breweryUrl = "N/A";
    var lrgImage;

    $.ajax({
        url: "api/BrewDB/Save/" + id,
        type: "POST",
        contentType: "application/json",
        data: id,
        async: false,
        success: function (data) {
            $.getJSON(brewUri + "/GetBrewery/" + id)
                .done(function (data) {
                    if (id == data.id) {
                        Name = "<b>Beer Name: </b><br />" + data.name;
                        Desc = "<b>Beer Description: </b><br />" + data.Desc;
                        ABV = "<b>Beer ABV: </b><br />" + data.abv;
                        breweryName = "<b>Brewery Name: </b><br />" + data.breweryName;
                        breweryUrl = "<b>Brewery Url: </b><br />" + data.breweryUrl;


                        $('#beerName').text(data.name);
                        $('#beerDescription').text(data.description);
                        $('#beerABV').text(data.abv);
                        $('#breweryName').append(data.breweryName);
                        $('#beerUrlText').text(data.breweryUrl);
                        $('#beerUrl').attr("href", data.breweryUrl);
                        $('#showImage').attr("src", data.medImage);
                    }
                });
        },
        error: function () {
            $('#output').text("Error: Save Failed");
        }
    });
};
function favList() {
    //Displays items appended to mongo 'BeerSaved' to favorites list 
    $.getJSON(brewUri + "/GetFavBeer")
        .done(function (data) {
            // On success, 'data' contains a list of products.
            $.each(data, function (key, item) {
                // Add a list item for the product.
                // Change the way to format the string(Sunny)
                $('#outputFavList').append('<li><a  data-filtertext="' + item.abv + '" data-transition="pop" data-parm=' + item.id + ' href="#details-page"><div hidden>' + item.name + '</div>' + item.name + '</a></li>');
                // Listview refresh after each inner loop(Sunny)
                $("#outputFavList").listview().listview("refresh");
            });
        });
}
function getTriedBeers(userObj) {
    var li = "";
    $.ajax({
        url: brewUri + "/GetTriedBeer/" + userObj,
        type: "POST",
        async: false,
        data: userObj,
        success: function (data) {
            $.each(data, function (key, item) {
                if (item.medImage) {
                    $('#outputFavList').append('<li><a data-filtertext="' + item.abv + '" data-transition="pop" data-parm=' + item.id + ' href="#details-page"><img src="' + item.iconImage + '"><div hidden>' + item.name + '</div><h2>' + item.name + '</h2><p>ABV: ' + item.abv + '</p></a></li>');
                } else {
                    $('#outputFavList').append('<li><a data-filtertext="' + item.abv + '" data-transition="pop" data-parm=' + item.id + ' href="#details-page"><img src="https://brewmasons.co.uk/wp-content/uploads/2017/05/gold-10-247x300.jpg" width=150><div hidden>' + item.name + '</div><h2>' + item.name + '</h2><p>ABV: ' + item.abv + '</p></a ></li > ');
                }
                // Listview refresh after each inner loop(Sunny)
                $("#outputFavList").listview().listview("refresh");
            });
        },
        error: function () {
        }
    });
};
function getUserStatus() {


    if (userSession) {
        return "success";
    } else {
        return false;
    }
};
function getWishlistBeers(userObj) {
    var li = "";
    $.ajax({
        url: brewUri + "/GetWishList/" + userObj,
        type: "POST",
        async: false,
        data: userObj,
        success: function (data) {
            if(!testing){
                $.each(data, function (key, item) {
                    if (item.medImage) {
                        $('#outputWishList').append('<li><a data-filtertext="' + item.abv + '" data-transition="pop" data-parm=' + item.id + ' href="#details-page"><img src="' + item.iconImage + '"><div hidden>' + item.name + '</div><h2>' + item.name + '</h2><p>ABV: ' + item.abv + '</p></a></li>');
                    } else {
                        $('#outputWishList').append('<li><a data-filtertext="' + item.abv + '" data-transition="pop" data-parm=' + item.id + ' href="#details-page"><img src="https://brewmasons.co.uk/wp-content/uploads/2017/05/gold-10-247x300.jpg" width=150><div hidden>' + item.name + '</div><h2>' + item.name + '</h2><p>ABV: ' + item.abv + '</p></a ></li > ');
                    }
                    // Listview refresh after each inner loop(Sunny)
                    $("#outputWishList").listview().listview("refresh");
                    
                });
            } else {
                testingResult = data.length;
            }
        },
        error: function () {
            testingResult = 0;
        }
    });
    if (testing)
        return testingResult;
};
function homePageList() {
    //Currently gets all the beer in the BeerMaster and displays them to the homepage
    if (!testing) {
        $.getJSON(brewUri + "/GetRndBeer")
            .done(function (data) {
                // On success, 'data' contains a list of products.
                $.each(data, function (key, item) {
                    // Add a list item for the product.
                    // Change the way to format the string(Sunny)
                    //$('#output').append('<li><a data-transition="pop" data-parm=' + item.id + ' href="#details-page?id=' + item.id + '"><div hidden>' + item.name + '</div>' + item.name + '</a></li>');
                    if (item.medImage) {
                        $('#search-output').append('<li><a data-filtertext="' + item.abv + '" data-transition="pop" data-parm=' + item.id + ' href="#details-page"><img src="' + item.iconImage + '"><div hidden>' + item.name + '</div><h2>' + item.name + '</h2><p>ABV: ' + item.abv + '</p></a></li>');
                    } else {
                        $('#search-output').append('<li><a  data-filtertext="' + item.abv + '" data-transition="pop" data-parm=' + item.id + ' href="#details-page"><img src="https://brewmasons.co.uk/wp-content/uploads/2017/05/gold-10-247x300.jpg" width=150><div hidden>' + item.name + '</div><h2>' + item.name + '</h2><p>ABV: ' + item.abv + '</p></a ></li > ');
                    }
                    // Listview refresh after each inner loop(Sunny)
                    $('#search-output').listview().listview('refresh');
                });
            });
    } else {
        $.ajax({
            url: brewUri + "/GetRndBeer",
            type: "GET",
            async: false,
            success: function (data) {
                testingResult = data.length;
            },
            error: function (data) {
                testingResult = "FAIL";
            }
        });
        return testingResult;
    }
};
function randomBeer() {
    //Create By: Caleb
    //On page load, this method will call BreweryDB to get a random beer.
    var apiCall = {
        call: "beer/random"
    };
    var success;

    $.ajax({
        url: brewUri + "/ApiRequest/" + apiCall,
        type: "POST",
        data: apiCall,
        async: false,
        success: function (data) {
            var rndBeer = JSON.parse(data);
            //var foo = JSON.stringify(rndBeer, null, 4);
            //rndBeer is the beer object and data is the raw json string
            //document.getElementById("search-output").innerHTML = JSON.stringify(rndBeer, null, 4);
            var beerJson = JSON.stringify(rndBeer.data);
            //Save random beer to DB
            $.ajax({
                url: brewUri + "/Save",
                type: "POST",
                contentType: "application/json",
                data: beerJson,
                async: false,
                success: function (data) {
                    //document.getElementById("search-output").innerHTML = "SUCCESS MESSAGE: " + data.name + " saved to BeerMaster";
                    success = true;
                },
                error: function () {
                    $('#search-output').text("Error: Save Failed");
                    success = false;
                }
            });
            success = rndBeer.message;
        },
        error: function () {
            $('#output').text("ERROR: API has been disabled to avoid going over our api request limit. Change brewUri back to api/BrewDB to call api.");
            success = false;
        }
    });

    if (testing)
        return success;
}
function refreshBeer() {

}
function saveToTried(saveRequest) {
    $.ajax({
        url: brewUri + "/SaveTriedBeer/" + saveRequest,
        type: "POST",
        async: false,
        data: saveRequest,
        success: function (data) {
            $('#saveResponseLable').text("Saved!");
            testingResult = data;
        },
        error: function (data) {
            $('#saveResponseLable').text("Error: Beer not saved!");
        }
    });
    if (testing)
        return testingResult;
};
function saveToWishlist(saveRequest) {
    $.ajax({
        url: brewUri + "/SaveToWishlist/" + saveRequest,
        type: "POST",
        async: false,
        data: saveRequest,
        success: function (data) {
            $('#saveResponseLable').text("Saved to wishlist");
            testingResult = data;
        },
        error: function (data) {
            $('#saveResponseLable').text("Error: Beer not saved to wishlist!");
            testingResult = data;
        }
    });
    if (testing)
        return testingResult;
};
function search(apiCall, searchCat) {
    var li = "";
    $.ajax({
        url: brewUri + "/Search/" + apiCall,
        type: "POST",
        data: apiCall,
        async: false,
        success: function (data) {
            var searchResults = JSON.parse(data);
            testingResult = searchResults.totalResults;
            if (!testing) {
                $('#search-output').empty();
                if (searchResults.data) {
                    $.each(searchResults.data, function (index, item) {
                        if (searchCat == "beer") {
                            if (item.labels) {
                                li += '<li><a data-filtertext="' + item.abv + '" data-transition="pop" data-parm=' + item.id + ' href="#details-page"><img src="' + item.labels.medium + '"><div hidden>' + item.name + '</div><h2>' + item.name + '</h2><p>ABV: ' + item.abv + '</p></a></li>';
                            }
                            else {
                                li += '<li><a data-filtertext="' + item.abv + '" class="apiLi" data-transition="pop" data-parm=' + item.id + ' href="#details-page"><img src="https://brewmasons.co.uk/wp-content/uploads/2017/05/gold-10-247x300.jpg" width=150><div hidden>' + item.name + '</div><h2>' + item.name + '</h2><p>ABV: ' + item.abv + '</p></a ></li > ';
                            }
                        }
                        else if (searchCat == "brewery") {
                            if (item.images) {
                                li += '<li><a data-transition="pop" data-parm=' + item.id + ' href="' + item.website + '" target="_blank"><img src="' + item.images.squareMedium + '"><div hidden>' + item.name + '</div><h2>' + item.name + '</h2></a></li>';
                            }
                            else {
                                li += '<li><a class="apiLi" data-transition="pop" data-parm=' + item.id + ' href="' + item.website + '" target="_blank"><img src="https://brewmasons.co.uk/wp-content/uploads/2017/05/gold-10-247x300.jpg" width=150><div hidden>' + item.name + '</div><h2>' + item.name + '</h2></a ></li > ';
                            }
                        }
                    });
                }
                else {
                    $('#searchStatus').text("No Results Found");
                }
                $('#search-output').append(li);
                $('#search-output').listview().listview('refresh');
            }
        },
        error: function () {
            $('#searchStatus').text("ERROR: Contact Caleb for support");
            testingResult = "FAIL";
        }
    });
    return testingResult;
};
function signIn(userObj) {
    $.ajax({
        url: brewUri + "/SignIn/" + userObj,
        type: "POST",
        async: false,
        data: userObj,
        success: function (data) {
            testingResult = data;
            if (!testing) {
                sessionStorage.removeItem('userSession');
                sessionStorage.removeItem('userSessionMsg');

                sessionStorage.setItem('userSession', userObj.uid);
                sessionStorage.setItem('userSessionMsg', "Welcome " + userObj.uid);

                $.mobile.changePage("#indexpage");
                location.reload(true);
            }

        },
        error: function (data) {
            testingResult = data;
            $('#SignInStatus').text("Sign In ERROR!");
        }
    });
    if (testing)
        return testingResult;
};
function signOut() {
    $.ajax({
        url: brewUri + "/SignOut/",
        type: "POST",
        async: false,
        success: function (data) {
            testingResult = data;
            if (!testing) {
                sessionStorage.removeItem('userSession');
                sessionStorage.removeItem('userSessionMsg');
                $('#userSessionMsg').empty();
                $.mobile.changePage("#indexpage");
                location.reload(true);
            }
        },
        error: function (data) {
            testingResult = data;
            $('#SignOutStatus').text("Sign Out ERROR!");
        }
    });
    if (testing)
        return testingResult;
};
function signUp(userObj) {
    $.ajax({
        url: brewUri + "/SignUp/" + userObj,
        type: "POST",
        async: false,
        data: userObj,
        success: function (data) {

            sessionStorage.removeItem('userSession');
            sessionStorage.removeItem('userSessionMsg');

            sessionStorage.setItem('userSession', userObj.uid);
            sessionStorage.setItem('userSessionMsg', "Welcome " + userObj.uid);

            $.mobile.changePage("#indexpage");
            location.reload(true);
        },
        error: function (data) {
            $('#signupError').text(data.responseJSON);
        }
    });
};