var NotificationViewModel = function () {
    var self = this;
    
    self.NotificationsList = ko.observableArray();
    self.NotificationCountList = ko.observable(0).extend({ numeric: 0 });
    this.NotificationItems = function () {
       
        AjaxNinja.Invoke(ODataApi.Notification + "?$filter=OwnerId eq '" + global.User.UserId() + "' and Type gt " + 1 + "&$orderby=ModifiedDate desc&$expand=Owner,Sender", "GET", {}, function (data) {
            self.NotificationCountList(data.value.length);
            var result = [];
            $(data.value).each(function (i, value) {
                //var date = new Date(Date.parse(value.ModifiedDate.replace("T", " ")));
                var date = BrowserDetect.browser != "Firefox" && BrowserDetect.browser != "Safari" && BrowserDetect.browser != "IE 11" ? new Date(Date.parse(value.ModifiedDate.replace("T", " "))) : new Date(Date.parse(value.ModifiedDate));
                value.ModifiedDate = date;
                if (date != null)
                    var res = $.extend(value, { Time: formatMoment(date) });

                result.push(res);


            });
            self.NotificationsList(result);
        });

    }

   
}

var items = new NotificationViewModel();
ko.applyBindings(items, document.getElementById("notificationContainer"));
items.NotificationItems();