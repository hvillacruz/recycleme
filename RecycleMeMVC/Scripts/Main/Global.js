var GlobalViewModel = function () {

    var self = this;
    self.Login = ko.observableArray();
    self.Message = ko.observableArray();
    self.MessageCount = ko.observable(0);
    self.WasNotified = ko.observable(false);

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



        AjaxNinja.Invoke(ODataApi.Message + "?$filter=ReceiverId eq '" + global.User.UserId() + "'&$orderby=DateReceived desc&$expand=Sender", "GET", {}, function (data) {
            self.MessageCount(data.value.length);
            var result = [];
            $(data.value).each(function (i, value) {
                var date = new Date(Date.parse(value.DateSent));
                value.DateSent = date;
                if (date != null)
                    var res = $.extend(value, { Time: formatAMPM(date) });

                result.push(res);


            });

            self.Message(result);

        });



    }


    this.AttachEvents = function () {

        var header = new Header();
        header.Logout();

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

}
