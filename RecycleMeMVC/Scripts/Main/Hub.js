var recycleUri = window.location.host.indexOf("localhost") !== 0 ? "http://recyclemeapi.azurewebsites.net/" : "http://localhost:53481/"
var HubWrapper = function () {
    var self = this;
    var theHub = null;
    var isStarted = false;
    var onMessageCallback = function () { };

    self.start = function (token) {

        theHub = $.connection.notificationHub;
        var connection = $.connection.hub;
        connection.url = "http://recyclemeapi.azurewebsites.net/signalr";
        //connection.url = "http://localhost:49892/signalr";

        $.signalR.ajaxDefaults.headers = { Authorization: "Bearer " + token };

        theHub.client.onMessage = onMessageCallback;
        theHub.client.recycleNotification = function (res) {
            toastr.info(res, "Info");
        }
        return $.connection.hub.start()
                .done(function () {
                    isStarted = true;
                    recycleHub.sendNotification("Notification Activated");
                });

    };

    self.stop = function () {
        isStarted = false;
        theHub = null;
        return $.connection.hub.stop();
    };

    self.sendNotification = function (message) {
        if (isStarted)
            theHub.server.sendNotification(message);

    };


    self.onMessage = function (callback) {
        onMessageCallback = callback;
        if (isStarted)
            theHub.client.onMessage = onMessageCallback;
    };


    self.useBearerToken = function (token) {
        var wasStarted = isStarted;


        if (isStarted)
            self.stop();


        if (wasStarted) {
            self.start();

            setTimeout(function () {
                self.sendNotfication("henry");
            }, 10000);

        }
    };



    self.clearAuthentication = function () {
        document.cookie = "BearerToken=; path=/; expires=" + new Date(0).toUTCString();
    }

};

function InvokeHub(access_token) {
    recycleHub.start(access_token).done(function () {
    });
}

function SignIn(userName, password, type, provider) {

    return $.post(recycleUri + "token", { grant_type: "password", username: userName, password: password })
            .done(function (data) {

                if (data && data.access_token) {

                    recycleHub.start(data.access_token).done(function () {

                        if (type == "login") {
                            $.ajax({
                                url: 'Login',
                                type: 'POST',
                                data: AddAntiForgeryToken({ UserName: userName, Password: password, returnUrl: '' }),
                                success: function (result) {
                                    window.location.href = window.location.origin + "/";
                                }
                            });
                        }
                        else {
                            $.ajax({
                                url: 'ExternalLogin',
                                type: 'POST',
                                data: AddAntiForgeryToken({ provider: provider, returnUrl: '' }),
                                success: function (result) {
                                    //window.location.href = window.location.origin + "/";
                                }
                            });
                        }

                    });

                }
            })
            .fail(function (xhr) {
                if (xhr.status == 400) {
                    toastr.error("Invalid user name or password");
                } else {
                    toastr.error("Unexpected error while signing in");
                }
            });

}


function ExternalLogin(provider, returnUrl) {


    AjaxNinja.Invoke('ExternalLogin', "POST", JSON.stringify({ provider: provider, returnUrl: returnUrl }), function (data) {
        alert(data);
    });


}

var recycleHub = new HubWrapper();