

$(document).ready(function () {
    var browsertype;

    console.log('screen dimensions');
    var width = $(window).width()
    var height = $(window).height()
    console.log(width);
    console.log(height);
    console.log(jQuery.browser);
    $.each($.browser, function (i, val) {

        if (val == true) {
              browsertype = i
        }
      
    });
    console.log(browsertype);
    SetBrowserData(width, height, browsertype, $.browser.version)
    console.log($.browser.version);
    
   // if (browsertype) { SetBrowserData(width, $(window).height(), browsertype, $.browser.version) }

});

function SetBrowserData(width, height, browser, version) {
    var url = qualifyURL('/Default.aspx');

    $.ajax({
        url: url + '/handlers/Utility/Client_DeviceData.ashx',
        // url: 'http://coreroute_test.standardtextile.com/handlers/Utility/Client_DeviceData.ashx',
        //url: 'http://coredemo.standardtextile.com/handlers/Utility/Client_DeviceData.ashx',
        type: 'GET',   // I'm doing a POST here just to show it can handle it too... 
        data: { method: 'SetUserBrowserVariables', args: { width: width, height: height, BrowserType: browser, BrowserVer: version } },
        success: function (data) {
            console.log('SetBrowserVariable: ' + true)


        },
        error: function (a, b, c) {
            alert(c);
            console.log('SetBrowserVariable: ' + false);
        }
    });


}
function qualifyURL(url) {
    var img = document.createElement('img');
    img.src = url; // set string url
    url = img.src; // get qualified url
    img.src = null; // no server request

    var urlsplit = String(url).split("/")
    console.log(urlsplit[2].split(":")[0]);
    console.log(url);
    if (urlsplit.length > 3) {
       
        if (urlsplit[2].split(":")[0].toString() == "localhost") {
            return urlsplit[0].toString() + '//' + urlsplit[2].toString() + "/8.14"
        }
        else {
            return urlsplit[0].toString() + '//' + urlsplit[2].toString();
        }
        
    }
    else {
        return null
    }
}

