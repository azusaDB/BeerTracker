QUnit.test("hello test", function (assert) {
    assert.ok(1 == "1", "Passed!");
});

QUnit.test("Random Beer Call", function (assert) {
    var foo = randomBeer();
    assert.ok(randomBeer() == true, "Passed!");
});