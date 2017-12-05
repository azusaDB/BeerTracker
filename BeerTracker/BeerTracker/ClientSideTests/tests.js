var result;
var beerObj = {
    name: "test",
    ABV: "0",
    Description: "testy mctester", 
    Brewery: "test brewery" ,
    Url: "",
    Image: "", 
};
var userObj = {
    uid: "tester",
    password: "bcuser17"
};
var signInUser = {
    uid: "admin",
    password: "bcuser17"
};

var signUpUser = {
    uid: "testy",
    password: "bcuser17"
};

var saveRequest = {
    uid: "saveTester",
    bid: "1TLb7b"
};

var searchBeer = {
    call: "search",
    parameters: "&q=trickster&type=beer"
};
var searchBrewery = {
    call: "search",
    parameters: "&q=coors&type=brewery"
};

QUnit.test("Client Side Unit Tests", function (assert) {
    /*
    I had to edit the randomBeer method in brewScript.js to return true if the method was successful
    assert.ok(<condition>, <name of test>);
    */
    result = randomBeer();
    assert.ok(result == "Request Successful", "randomBeer - Expected Result: \"Request Successful\" - Actual Result: \"" + result + "\"");
    result = addBeer(beerObj);
    assert.ok(result == "Success: Beer Added", "addBeer - Expected Result: \"Success: Beer Added\" - Actual Result: \"" + result + "\"")
    /*favList() - NOT USED*/
    /*getUserStatus() - NOT USED*/
    result = getTriedBeers(userObj);
    assert.ok(result == 3, "getTriedBeers - Expected Result: 3 - Acutal Result: " + result);
    result = getWishlistBeers(userObj);
    assert.ok(result == 3, "getWishlistBeers - Expected Result: 3 - Acutal Result: " + result);
    result = homePageList();
    assert.ok(result == 10, "homePageList - Expected Result: 10 - Acutal Result: " + result);
    /*randomBeer() - NOT USED*/
    /*refreshBeer() - NOT USED*/
    result = saveToTried(saveRequest);
    assert.ok(result == "Success: Beer Saved", "saveToTried - Expected Result: \"Success: Beer Saved\" - Acutal Result: \"" + result + "\"");
    result = saveToWishlist(saveRequest);
    assert.ok(result == "Success: Beer Saved", "saveToWishlist - Expected Result: \"Success: Beer Saved\" - Acutal Result: \"" + result + "\"");
    result = search(searchBeer, "beer");
    assert.ok(result == 10, "search beer - Expected Result: 10 beers found - Acutal Result: " + result + " beers found.");
    result = search(searchBrewery, "brewery");
    assert.ok(result == 3, "search brewery - Expected Result: 3 breweries found - Acutal Result: " + result + " breweries found");
    result = signIn(signInUser);
    assert.ok(result == "User has been signed in", "signIn - Expected Result: \"User has been signed in\" - Acutal Result: \"" + result + "\"");
    result = signOut();
    assert.ok(result == "User has been signed out", "signOut - Expected Result: \"User has been signed out\" - Acutal Result: \"" + result + "\"");
    result = signUp(signUpUser);
    assert.ok(result == "Success: User Signed up", "signUp - Expected Result: \"Success: User Signed up\" - Acutal Result: \"" + result + "\"");
});


/*
*****TO ENABLE TESTING*****
1. Go to brewScript.js
2. Set variable testing to true

*****TO DISABLE TESTING*****
1. Go to brewScript.js
2. Set variable testing to false
*/