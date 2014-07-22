var GlobalViewModel = function () {

    var self = this;
    self.Login = ko.observableArray();
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

    this.AttachEvents = function () {

        var header = new Header();
        header.Logout();

    }

}