var ODataNinja = {
    Read: function (url, callbackFn) {
        OData.read({
            requestUri: url,
            headers: { 'Content-Type': "application/json" }
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
            data: data,
            processData: true,
            success: function (response) {
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