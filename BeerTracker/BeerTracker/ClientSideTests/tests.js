QUnit.test("Client Side Unit Tests", function (assert) {
    assert.ok(1 == "1", "Passed!");

    /*
    I had to edit the randomBeer method in brewScript.js to return true if the method was successful
    assert.ok(<condition>, <name of test>); See above example for more simplified version of a test.
    */
    var rdnBeerResult = randomBeer();
    assert.ok(rdnBeerResult == "Request Successful", "randomBeer");
});


/*
*****TO ENABLE TESTING*****
1. Go to brewScript.js
2. Set variable testing to true

*****TO DISABLE TESTING*****
1. Go to brewScript.js
2. Set variable testing to false
*/