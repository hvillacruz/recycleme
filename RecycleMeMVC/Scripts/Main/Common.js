var ODataNinja = {
    Read: function (url, callbackFn) {
        OData.read({
            requestUri: url,
            headers: { "Content-Type": "application/json" }
        },
        function (data, response) {
            CoreNinja.CallBack(callbackFn, data);
        },
        function (err) {
            alert(err.message);
        });
    }
}


var AjaxNinja = {
    Invoke: function (url, method, data, callbackFn) {

        $.ajax({
            type: method,
            url: url,
            dataType: "json",
            contentType: "application/json",
            data: data,
            processData: true,
            success: function (data) {
                CoreNinja.CallBack(callbackFn, data);
            },
            error: function (jqxhr, msg, err) {
                alert(msg);
            }
        });
    }
}

var CoreNinja = {
    CallBack: function (fn, data) {
        if (fn) {
            setTimeout(function () {
                fn(data);
            });
        }
    }
}


var BrowserDetect =
{
    init: function () {
        this.browser = this.searchString(this.dataBrowser) || "Other";
        this.version = this.searchVersion(navigator.userAgent) || this.searchVersion(navigator.appVersion) || "Unknown";
    },

    searchString: function (data) {
        for (var i = 0 ; i < data.length ; i++) {
            var dataString = data[i].string;
            this.versionSearchString = data[i].subString;

            if (dataString.indexOf(data[i].subString) != -1) {
                return data[i].identity;
            }
        }
    },

    searchVersion: function (dataString) {
        var index = dataString.indexOf(this.versionSearchString);
        if (index == -1) return;
        return parseFloat(dataString.substring(index + this.versionSearchString.length + 1));
    },

    dataBrowser:
    [
        { string: navigator.userAgent, subString: "Chrome", identity: "Chrome" },
        { string: navigator.userAgent, subString: "MSIE", identity: "Explorer" },
        { string: navigator.userAgent, subString: "Firefox", identity: "Firefox" },
        { string: navigator.userAgent, subString: "Safari", identity: "Safari" },
        { string: navigator.userAgent, subString: "Opera", identity: "Opera" },
        { string: navigator.userAgent, subString: "Trident", identity: "IE 11" }
    ]

};


function formatAMPM(date) {
    var hours = date.getHours();
    var minutes = date.getMinutes();
    var ampm = hours >= 12 ? 'pm' : 'am';
    hours = hours % 12;
    hours = hours ? hours : 12; // the hour '0' should be '12'
    minutes = minutes < 10 ? '0' + minutes : minutes;
    var strTime = hours + ':' + minutes + ' ' + ampm;
    return strTime;
}

BrowserDetect.init();