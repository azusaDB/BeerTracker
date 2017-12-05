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

var saveRequest = {
    uid: "saveTester",
    bid: "1TLb7b"
};
QUnit.test("Client Side Unit Tests", function (assert) {
    assert.ok(1 == "1", "Passed!");

    /*
    I had to edit the randomBeer method in brewScript.js to return true if the method was successful
    assert.ok(<condition>, <name of test>); See above example for more simplified version of a test.
    */
    result = randomBeer();
    assert.ok(result == "Request Successful", "randomBeer - Expected Result: \"Request Successful\" - Actual Result: \"" + result + "\"");
    result = addBeer(beerObj);
    assert.ok(result == "Success: Beer Added", "addBeer - Expected Result: \"Success: Beer Added\" - Actual Result: \"" + result + "\"")
    /*favList() - NOT USED*/
    /*getUserStatus() - NOT USED*/
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
    
});


/*
*****TO ENABLE TESTING*****
1. Go to brewScript.js
2. Set variable testing to true

*****TO DISABLE TESTING*****
1. Go to brewScript.js
2. Set variable testing to false
*/