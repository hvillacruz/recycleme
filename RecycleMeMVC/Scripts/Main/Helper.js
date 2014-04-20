var Helper = {

    callLater: function (callback, data) {

        if (callback) {
            setTimeout(function () {
                callback(data);
            });
        }

    },
    extend: function (a, b) {

        for (var key in b) {
            if (b.hasOwnProperty(key)) {
                a[key] = b[key];
            }
        }

        return a;

    },
    getObject: function (obj, property, value, callback) {

        var result = null;

        $.each(obj, function (i, obj) {
            if (obj[property] = value)
                result = obj;
        });

        Core.callLater(callback, result);

    },
    getQueryString: function (name) {

        name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
        var regexS = "[\\?&]" + name + "=([^&#]*)";
        var regex = new RegExp(regexS);
        var results = regex.exec(window.location.search);
        if (results === null)
            return "";
        else
            return decodeURIComponent(results[1].replace(/\+/g, " "));

    },
    isLive: function () {

        return Config.isDebug || false;

    },
    isNull: function (val) {

        var result = false;

        if (val === null || val === "" || val === undefined)
            result = true;

        return result;

    },
    nullRealValue: function (obj) {

        return obj && obj !== "null" && obj !== "undefined";

    },
    log: function (msg) {

        if (Core.isLive())
            console.log(msg);

    },
    setKeyValuePair: function (obj, arr) {

        if (Core.isNull(obj) !== true && Core.isNull(arr) !== true) {
            $.each(arr, function (key, value) {
                obj.attr(key, value);
            });
        } else {
            Core.log("setKeyValuePair : Null Value");
            return false;
        }

        return obj;

    },
    validateEmail: function (email) {
        var re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
        return re.test(email);

    },
    time: function () {

        var currentdate = new Date();
        return currentdate.getFullYear() + "-"
                        + ('0' + (currentdate.getMonth() + 1)).slice(-2) + "-"
                        + ('0' + currentdate.getDate()).slice(-2) + "T"
                        + ('0' + currentdate.getHours()).slice(-2) + ":"
                        + ('0' + currentdate.getMinutes()).slice(-2) + ":"
                        + ('0' + currentdate.getSeconds()).slice(-2);

    }
};