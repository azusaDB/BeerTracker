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
    /*favList() - NOT USED IN PRODUCTION*/
    /*getUserStatus() - NOT USED IN PRODUCTION*/
    result = getWishlistBeers(userObj);
    assert.ok(result == 3, "getWishlistBeers - Expected Result: 3 - Acutal Result: " + result);
    
});


/*
*****TO ENABLE TESTING*****
1. Go to brewScript.js
2. Set variable testing to true

*****TO DISABLE TESTING*****
1. Go to brewScript.js
2. Set variable testing to false
*/