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

        theHub.client.recycleNotification = function (res, type) {

            switch (type) {

                case 2: if (window.location.href.toUpperCase().indexOf("DASHBOARD") > 0) {
                    profileBar.ProfileBar();
                    users.UserFollower();
                }
                    break;
                case 3: if (window.location.href.toUpperCase().indexOf("DASHBOARD") > 0)
                    timeline.ItemTimeline();
                    break;
                case 4:
                    break;
                case 5: if (window.location.href.toUpperCase().indexOf("EXCHANGE") > 0 || (window.location.href.toUpperCase().indexOf("TRADE") > 0)) {
                    if (window.location.href.toUpperCase().indexOf("TRADE") > 0)
                        items.TradeItem();
                    if (window.location.href.toUpperCase().indexOf("EXCHANGE") > 0)
                        exchange.TradeItem();
                }
                    break;
                case 6: if (window.location.href.toUpperCase().indexOf("EXCHANGE") > 0 || (window.location.href.toUpperCase().indexOf("TRADE") > 0)) {
                    if (window.location.href.toUpperCase().indexOf("TRADE") > 0)
                        items.TradeItem();
                    if (window.location.href.toUpperCase().indexOf("EXCHANGE") > 0)
                        exchange.TradeItem();
                }
                case 7:
                    break;
                case 8:
                    break;
                default:
                    break;
            }
            global.GetNotifications();
            global.Notification(res, "Notification");
            $("#notificationBell").addClass("icon-animated-bell");

            setTimeout(function () {
                $("#notificationBell").removeClass("icon-animated-bell");
            }, 10000);

        }

        theHub.client.messageNotification = function (res, user) {

            //global.AddMsgCount();
            console.log(user);
            global.GetMessage();
            global.Notification(res, "Message");

            $("#msgBell").addClass("icon-animated-vertical");

            setTimeout(function () {
                $("#msgBell").removeClass("icon-animated-vertical");
            }, 10000);



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

    self.sendNotification = function (type, message, user, cat) {
        if (isStarted) {
            switch (type) {
                case "MSG": theHub.server.messageNotification(message, user);
                    break;
                default: theHub.server.sendNotification(message, user, cat);

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