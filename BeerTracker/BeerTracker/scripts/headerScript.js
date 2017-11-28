var $userSession = sessionStorage.getItem('userSession');
var $userSessionMsg = sessionStorage.getItem('userSessionMsg');

if ($userSession) {

    $(document).on('pagebeforecreate', '#indexpage, #add-page,#favorites-page,#wishlist-page,#details-page,#about-page,#signin,#signup,#search,#myprofile', function () {
        $('<div>').attr({ 'data-role': 'header', 'data-theme': 'b', 'data-position': 'fixed', 'data-id': 'headerBar' }).append('<div data-role="controlgroup" data-type="horizontal" class="ui-mini ui-btn-left" id="headerBar"><a href= "#indexpage" class="ui-btn-icon-right ui-icon-home" ></a ></div><div style="width:464px;margin:0 auto;"><a href= "#indexpage"><img src="images/Header.png" alt="Beer Tracker" style="margin:0 auto;" /></a></div><div data-role="navbar" data-grid="d"><ul><li><a href="#add-page" data-icon="plus">Add</a></li><li><a href="#favorites-page" data-icon="heart">My Fav</a></li><li><a href="#wishlist-page" data-icon="star">Wishlist</a></li><li><a href="#about-page" data-icon="info">About</a></li><li><a href="#myprofile" data-icon="user">My Profile</a></li></ul></div>').appendTo($(this));
        $('<div>').attr({ 'data-role': 'footer', 'data-theme': 'b', 'data-position': 'fixed', 'data-id': 'footerBar' }).append('<h3>Team Red Production</h3>').appendTo($(this));
    });


} else {

    $(document).on('pagebeforecreate', '#indexpage, #add-page,#favorites-page,#wishlist-page,#details-page,#about-page,#signin,#signup,#search', function () {
        $('<div>').attr({ 'data-role': 'header', 'data-theme': 'b', 'data-position': 'fixed', 'data-id': 'headerBar' }).append('<div data-role="controlgroup" data-type="horizontal" class="ui-mini ui-btn-left" id="headerBar"><a href= "#indexpage" class="ui-btn-icon-right ui-icon-home" ></a ></div><div style="width:464px;margin:0 auto;"><a href= "#indexpage"><img src="images/Header.png" alt="Beer Tracker" style="margin:0 auto;" /></a></div><div data-role="navbar" data-grid="d"><ul><li><a href="#add-page" data-icon="plus">Add</a></li><li><a href="#favorites-page" data-icon="heart">My Fav</a></li><li><a href="#wishlist-page" data-icon="star">Wishlist</a></li><li><a href="#about-page" data-icon="info">About</a></li><li><a href="#signin" data-icon="user">Sign In</a></li></ul></div>').appendTo($(this));
        $('<div>').attr({ 'data-role': 'footer', 'data-theme': 'b', 'data-position': 'fixed', 'data-id': 'footerBar' }).append('<h3>Team Red Production</h3>').appendTo($(this));
    });

}


$(document).bind('pageinit', function () {
    $.mobile.loader.prototype.options.text = "Loading..";
    $.mobile.loader.prototype.options.textVisible = true;
    $.mobile.loader.prototype.options.theme = "c";
    $.mobile.loader.prototype.options.textonly = false;
});


$(document).on({
    ajaxSend: function () { $.mobile.loading('show'); },
    ajaxStart: function () { $.mobile.loading('show'); },
    ajaxStop: function () { $.mobile.loading('hide'); },
    ajaxError: function () { $.mobile.loading('hide'); }
});
