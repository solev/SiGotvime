
// This is called with the results from from FB.getLoginStatus().
function LoginCallback(response) {
    var url = window.location.href.toLowerCase();
    // The response object is returned with a status field that lets the
    // app know the current login status of the person.
    // Full docs on the response object can be found in the documentation
    // for FB.getLoginStatus().
    if (response.status === 'connected') {
        // Logged into your app and Facebook. 
        LoginToServer();        
    } else if (response.status === 'not_authorized') {
       
    } else {
        
    }
}

function fbStatusCallback(response)
{    
    var url = window.location.href.toLowerCase();
    console.log(response);
    // The response object is returned with a status field that lets the
    // app know the current login status of the person.
    // Full docs on the response object can be found in the documentation
    // for FB.getLoginStatus().
    if (response.status === 'connected') {
        // Logged into your app and Facebook.        
        if (isAuthenticated == 'False') {
            FB.logout(function (response) {
                // Person is now logged out
            });
            return;
        }        
    } else if (response.status === 'not_authorized') {
        
    } else {
        if (isAuthenticated == 'True' && isFbUser == 'True') {
            Logout();
        }
    }
}

function Logout()
{
    window.location.href = "/Account/Logout";
}

// This function is called when someone finishes with the Login
// Button.  See the onlogin handler attached to it in the sample
// code below.
function checkLoginState() {
    FB.getLoginStatus(function (response) {
        LoginCallback(response);
    });
}

window.fbAsyncInit = function () {
    FB.init({
        appId: '885684978186413',
        cookie: true,  // enable cookies to allow the server to access 
        // the session
        xfbml: true,  // parse social plugins on this page
        version: 'v2.4' // use version 24.
    });

    // Now that we've initialized the JavaScript SDK, we call 
    // FB.getLoginStatus().  This function gets the state of the
    // person visiting this page and can return one of three states to
    // the callback you provide.  They can be:
    //
    // 1. Logged into your app ('connected')
    // 2. Logged into Facebook, but not your app ('not_authorized')
    // 3. Not logged into Facebook and can't tell if they are logged into
    //    your app or not.
    //
    // These three cases are handled in the callback function.

    FB.getLoginStatus(function (response) {
        fbStatusCallback(response);
    });

    
};

function LoginToServer()
{
    FB.api('/me?fields=id,name,picture.width(200).height(200),first_name,last_name,gender,link,location,sports,public_key,cover', function (response) {
        console.log('Successful login for: ' + response.name);
        console.log(response);
        response.url = response.picture.data.url;
        $.ajax({
            url: "/Account/FBLogin",
            method: "POST",
            data: {
                model: response
            }
        }).done(function (data) {
            window.location.href = "/";
        });

    });
}

// Load the SDK asynchronously
(function (d, s, id) {
    var js, fjs = d.getElementsByTagName(s)[0];
    if (d.getElementById(id)) return;
    js = d.createElement(s); js.id = id;
    js.src = "//connect.facebook.net/en_US/sdk.js";
    fjs.parentNode.insertBefore(js, fjs);
}(document, 'script', 'facebook-jssdk'));
