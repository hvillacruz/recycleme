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

                    setTimeout(function () {
                        recycleHub.sendNotification("Notification Activated");
                    }, 2000)
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



function SignIn(userName, password) {

    return $.post("http://recyclemeapi.azurewebsites.net/token", { grant_type: "password", username: userName, password: password })
            .done(function (data) {

                if (data && data.access_token) {

                    recycleHub.start(data.access_token).done(function () {


                        $.ajax({
                            url: 'Login',
                            type: 'POST',
                            //data: { model: { UserName: $("#UserName").val(), Password: $("#Password").val() }, returnUrl: '' },
                            data: AddAntiForgeryToken({ UserName: userName, Password: password, returnUrl: '' }),
                            success: function (result) {
                                window.location.href = window.location.origin + "/";
                            }
                        });


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


var recycleHub = new HubWrapper();