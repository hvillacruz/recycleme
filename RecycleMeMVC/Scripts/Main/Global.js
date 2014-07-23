var GlobalViewModel = function () {

    var self = this;
    self.Login = ko.observableArray();
    self.Message = ko.observableArray();
    self.MessageCount = ko.observable(0);
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

}