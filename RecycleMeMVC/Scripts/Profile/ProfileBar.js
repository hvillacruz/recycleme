var ProfileBarViewModel = function () {

    var self = this;
    this.Bar = ko.observableArray();
    this.ViewUser = ko.observable();
    this.ProfileBar = function () {

        AjaxNinja.Invoke(ODataApi.User + "('" + $("#currentUser").data("text") + "')" + "?$expand=UserFollowerUsers,UserCommenter,UserFollowing", "GET", {}, function (data) {
            self.Bar(data);
        });

    }

    this.likeUser = function () {

        var data = {
            FollowerId: global.User.UserId(),
            FollowedUserId: self.ViewUser,
            ModifiedDate: Helper.time()
        }

        AjaxNinja.Invoke(ODataApi.UserFollow, "POST", JSON.stringify(data), function (data) {

            if (data.value.length != 0)
                self.Following(data.value);
        });

    }

}
var profileBar = new ProfileBarViewModel();
ko.applyBindings(profileBar, document.getElementById("profileBarDiv"));
profileBar.ProfileBar();