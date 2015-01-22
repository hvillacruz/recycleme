var GlobalViewModel = function () {

    ko.extenders.numeric = function (target, precision) {
        //create a writeable computed observable to intercept writes to our observable
        var result = ko.computed({
            read: target,  //always return the original observables value
            write: function (newValue) {
                var current = target(),
                    roundingMultiplier = Math.pow(10, precision),
                    newValueAsNum = isNaN(newValue) ? 0 : parseFloat(+newValue),
                    valueToWrite = Math.round(newValueAsNum * roundingMultiplier) / roundingMultiplier;

                //only write if it changed
                if (valueToWrite !== current) {
                    target(valueToWrite);
                } else {
                    //if the rounded value is the same, but a different value was written, force a notification for the current field
                    if (newValue !== current) {
                        target.notifySubscribers(valueToWrite);
                    }
                }
            }
        }).extend({ notify: 'always' });

        //initialize with current value to make sure it is rounded appropriately
        result(target());

        //return the new computed observable
        return result;
    };

    var self = this;
    self.Login = ko.observableArray();
    self.Message = ko.observableArray();
    self.Notifications = ko.observableArray();
    self.MessageCount = ko.observable(0).extend({ numeric: 0 });
    self.NotificationCount = ko.observable(0).extend({ numeric: 0 });
    self.WasNotified = ko.observable(false);
    self.SelectedModalImage = ko.observableArray();
    this.User = function (UserId, UserName) {

        this.User.UserId = ko.observable(UserId);
        this.User.UserName = ko.observable(UserName);

    },

    this.Profile = function () {

        //if (BrowserDetect.browser = "Safari") {
        //    ODataNinja.Read(ODataApi.User + "('" + this.User.UserId() + "')", function (data) {
        //        self.Login(data);
        //        self.AttachEvents();
        //    });
        //} else {
        AjaxNinja.Invoke(ODataApi.User + "('" + this.User.UserId() + "')", "GET", {}, function (data) {
            self.Login(data);
            self.AttachEvents();
        });

        // }

    }

    this.GetMessage = function () {

        AjaxNinja.Invoke(ODataApi.Message + "?$filter=ReceiverId eq '" + global.User.UserId() + "'&$orderby=DateSent desc&$expand=Sender", "GET", {}, function (data) {
            self.MessageCount(data.value.length);
            var result = [];
            $(data.value).each(function (i, value) {
                //var date = new Date(Date.parse(value.DateSent.replace("T", " ")));
                var date = BrowserDetect.browser != "Firefox" && BrowserDetect.browser != "Safari" ? new Date(Date.parse(value.DateSent.replace("T", " "))) : new Date(Date.parse(value.DateSent));
                value.DateSent = date;
                if (date != null)
                    var res = $.extend(value, { Time: formatMoment(date) });

                result.push(res);


            });
            self.Message(result);
        });
    }


    this.GetNotifications = function () {

        AjaxNinja.Invoke(ODataApi.Notification + "?$filter=OwnerId eq '" + global.User.UserId() + "' and Type gt " + 1 + "&$orderby=ModifiedDate desc&$expand=Owner,Sender", "GET", {}, function (data) {
            self.NotificationCount(data.value.length);
            var result = [];
            $(data.value).each(function (i, value) {
                //var date = new Date(Date.parse(value.ModifiedDate.replace("T", " ")));
                var date = BrowserDetect.browser != "Firefox" && BrowserDetect.browser != "Safari" ? new Date(Date.parse(value.ModifiedDate.replace("T", " "))) : new Date(Date.parse(value.ModifiedDate));
                value.ModifiedDate = date;
                if (date != null)
                    var res = $.extend(value, { Time: formatMoment(date) });

                result.push(res);


            });
            self.Notifications(result);
        });
    }


    this.AttachEvents = function () {

        var header = new Header();
        header.Logout();

    }

    this.ChangeDate = function (d) {
        var date = BrowserDetect.browser != "Firefox" && BrowserDetect.browser != "Safari" ? new Date(Date.parse(d.replace("T", " "))) : new Date(Date.parse(d));
        console.log('date:' + formatMoment(date));
        return formatMoment(date)
    }



    this.Notification = function (msg, title, type) {


        toastr.options = {
            "closeButton": true,
            "debug": false,
            "positionClass": "toast-bottom-right",
            "showDuration": "300",
            "hideDuration": "1000",
            "showMethod": "slideDown",
            "hideMethod": "slideUp"
        }

        toastr.options.onShown = function () {

        }

        toastr.options.onHidden = function () {

        }

        switch (type) {

            case "info": return toastr.info(msg, title);

            case "success": return toastr.success(msg, title);

            case "warning": return toastr.warning(msg, title);

            case "error": return toastr.error(msg, title);

            default: return toastr.info(msg, title);
        }

    }

    this.AddMsgCount = function () {

        var total = self.MessageCount() + 1;
        self.MessageCount(total);
    }

    this.AddNotificationCount = function () {

        var total = self.NotificationCount() + 1;
        self.NotificationCount(total);
    }


    this.GetModalItem = function (type, datum) {
      
        var ItemId = 0;
        if (type == 'main')
            ItemId = datum.Id;
        else
            ItemId = datum.UrlId;

        $(".search-box").hide();

        AjaxNinja.Invoke(ODataApi.Item + "(" + ItemId + ")" + "?$expand=Owner,ItemImages,Owner,Category,ItemCommented,ItemCommented/Commenter,ItemUserFollowers", "GET", {}, function (current) {
            //global.SelectedModalImage({});
            var res = $.extend(current, { CommentText: "" });
            $(current).push(res);
            global.SelectedModalImage(current);
            console.log('GetModalItem');
            console.log(global.SelectedModalImage().Owner.Avatar);
            console.log(global.SelectedModalImage());

        });
    }
}


