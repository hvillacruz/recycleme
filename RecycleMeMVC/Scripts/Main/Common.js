if (typeof String.prototype.trimLeft !== "function") {
    String.prototype.trimLeft = function () {
        return this.replace(/^\s+/, "");
    };
}
if (typeof String.prototype.trimRight !== "function") {
    String.prototype.trimRight = function () {
        return this.replace(/\s+$/, "");
    };
}
if (typeof Array.prototype.map !== "function") {
    Array.prototype.map = function (callback, thisArg) {
        for (var i = 0, n = this.length, a = []; i < n; i++) {
            if (i in this) a[i] = callback.call(thisArg, this[i]);
        }
        return a;
    };
}
function getCookies() {
    var c = document.cookie, v = 0, cookies = {};
    if (document.cookie.match(/^\s*\$Version=(?:"1"|1);\s*(.*)/)) {
        c = RegExp.$1;
        v = 1;
    }
    if (v === 0) {
        c.split(/[,;]/).map(function (cookie) {
            var parts = cookie.split(/=/, 2),
                name = decodeURIComponent(parts[0].trimLeft()),
                value = parts.length > 1 ? decodeURIComponent(parts[1].trimRight()) : null;
            cookies[name] = value;
        });
    } else {
        c.match(/(?:^|\s+)([!#$%&'*+\-.0-9A-Z^`a-z|~]+)=([!#$%&'*+\-.0-9A-Z^`a-z|~]*|"(?:[\x20-\x7E\x80\xFF]|\\[\x00-\x7F])*")(?=\s*[,;]|$)/g).map(function ($0, $1) {
            var name = $0,
                value = $1.charAt(0) === '"'
                          ? $1.substr(1, -1).replace(/\\(.)/g, "$1")
                          : $1;
            cookies[name] = value;
        });
    }
    return cookies;
}
function getCookie(name) {
    return getCookies()[name];
}


var Resample = (function (canvas) {

    // (C) WebReflection Mit Style License

    // Resample function, accepts an image
    // as url, base64 string, or Image/HTMLImgElement
    // optional width or height, and a callback
    // to invoke on operation complete
    function Resample(img, width, height, onresample) {
        var
         // check the image type
         load = typeof img == "string",
         // Image pointer
         i = load || img
        ;
        // if string, a new Image is needed
        if (load) {
            i = new Image;
            // with propers callbacks
            i.onload = onload;
            i.onerror = onerror;
        }
        // easy/cheap way to store info
        i._onresample = onresample;
        i._width = width;
        i._height = height;
        // if string, we trust the onload event
        // otherwise we call onload directly
        // with the image as callback context
        load ? (i.src = img) : onload.call(img);
    }

    // just in case something goes wrong
    function onerror() {
        throw ("not found: " + this.src);
    }

    // called when the Image is ready
    function onload() {
        var
         // minifier friendly
         img = this,
         // the desired width, if any
         width = img._width,
         // the desired height, if any
         height = img._height,
         // the callback
         onresample = img._onresample
        ;
        // if width and height are both specified
        // the resample uses these pixels
        // if width is specified but not the height
        // the resample respects proportions
        // accordingly with orginal size
        // same is if there is a height, but no width
        width == null && (width = round(img.width * height / img.height));
        height == null && (height = round(img.height * width / img.width));
        // remove (hopefully) stored info
        delete img._onresample;
        delete img._width;
        delete img._height;
        // when we reassign a canvas size
        // this clears automatically
        // the size should be exactly the same
        // of the final image
        // so that toDataURL ctx method
        // will return the whole canvas as png
        // without empty spaces or lines
        canvas.width = width;
        canvas.height = height;
        // drawImage has different overloads
        // in this case we need the following one ...
        context.drawImage(
         // original image
         img,
         // starting x point
         0,
         // starting y point
         0,
         // image width
         img.width,
         // image height
         img.height,
         // destination x point
         0,
         // destination y point
         0,
         // destination width
         width,
         // destination height
         height
        );
        // retrieve the canvas content as
        // base4 encoded PNG image
        // and pass the result to the callback
        onresample(canvas.toDataURL("image/png"));
    }

    var
     // point one, use every time ...
     context = canvas.getContext("2d"),
     // local scope shortcut
     round = Math.round
    ;

    return Resample;

}(
 // lucky us we don't even need to append
 // and render anything on the screen
 // let's keep this DOM node in RAM
 // for all resizes we want
 this.document.createElement("canvas"))
);



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
        $.support.cors = true;
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


function formatMoment(date) {

    var cd = moment(date);
    var strTime = cd.fromNow();
    return strTime;

}

BrowserDetect.init();