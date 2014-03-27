var ProfileBarViewModel = function () {

    var self = this;
    this.Bar = ko.observableArray();
    this.ProfileBar = function () {

        AjaxNinja.Invoke(ODataApi.User + "('" + global.User.UserId() + "')" + "?$expand=UserFollowerUsers,UserCommenter,UserFollowing", "GET", {}, function (data) {
            self.Bar(data);
        });

    }

}
var profileBar = new ProfileBarViewModel();
ko.applyBindings(profileBar, document.getElementById("profileBarDiv"));
profileBar.ProfileBar();