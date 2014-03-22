var ProfileBarViewModel = function () {

    var self = this;
    this.Bar = ko.observableArray();
    this.ProfileBar = function () {

        if (BrowserDetect.browser != "Safari") {
            ODataNinja.Read(ODataApi.ProfileBar + "('" + global.User.UserId() + "')", function (data) {
                self.Bar(data);
            });
        } else {
            AjaxNinja.Invoke(ODataApi.ProfileBar + "('" + global.User.UserId() + "')", "GET", {}, function (data) {
                self.Bar(data);
            });

        }


    }

}
var profileBar = new ProfileBarViewModel();
ko.applyBindings(profileBar, document.getElementById("profileBarDiv"));
profileBar.ProfileBar();