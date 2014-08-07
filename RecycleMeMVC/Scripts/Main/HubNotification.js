var recycleUri = window.location.host.indexOf("localhost") !== 0 ? "http://recyclemeapi.azurewebsites.net/" : "http://localhost:53481/"
var HubWrapper = function () {
    var self = this;
    var theHub = null;
    var isStarted = false;
    var onMessageCallback = function () { };

    self.start = function (token) {

        theHub = $.connection.notificationHub;
        var connection = $.connection.hub;
        connection.url = recycleUri + "signalr";

        $.signalR.ajaxDefaults.headers = { Authorization: "Bearer " + token };

        theHub.client.onMessage = onMessageCallback;

        theHub.client.recycleNotification = function (res) {

            global.Notification(res, "Notification");
            $("#notificationBell").addClass("icon-animated-bell");

            setTimeout(function () {
                $("#notificationBell").removeClass("icon-animated-bell");
            }, 4000);

        }

        theHub.client.messageNotification = function (res) {
           
            global.AddMsgCount();
            global.Notification(res, "Message");

            $("#msgBell").addClass("icon-animated-vertical");

            setTimeout(function () {
                $("#msgBell").removeClass("icon-animated-vertical");
            }, 4000);

        }

        return $.connection.hub.start()
                .done(function () {
                    isStarted = true;
                });

    };

    self.stop = function () {
        isStarted = false;
        theHub = null;
        return $.connection.hub.stop();
    };

    self.sendNotification = function (type, message, user) {
        if (isStarted) {
            switch (type) {
                case "MSG": theHub.server.sendNotification(message, user);
                    break;
                default: theHub.server.sendNotification(message);

            }
        }
    };

    self.onMessage = function (callback) {
        onMessageCallback = callback;
        if (isStarted)
            theHub.client.onMessage = onMessageCallback;
    };

    self.clearAuthentication = function () {
        document.cookie = "BearerToken=; path=/; expires=" + new Date(0).toUTCString();
    }

};
function InvokeHub(access_token) {
    recycleHub.start(access_token).done(function () {
    });
}
var recycleHub = new HubWrapper();