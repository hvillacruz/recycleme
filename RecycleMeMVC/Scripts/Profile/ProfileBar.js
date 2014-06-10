var ProfileBarViewModel = function () {

    var self = this;
    this.Bar = ko.observableArray();
    this.ViewUser = ko.observable("Test");
    this.ProfileBar = function () {

        AjaxNinja.Invoke(ODataApi.User + "('" + $("#currentUser").data("text") + "')" + "?$expand=UserFollowerUsers,UserCommenter,UserFollowing", "GET", {}, function (data) {
            self.Bar(data);
        });

    }

    this.likeUser = function () {

        //var data = {
        //    FollowerId: global.User.UserId(),
        //    FollowedUserId: profileBar.ViewUser(),
        //    ModifiedDate: Helper.time()
        //}

        var data = {
            FollowingId: global.User.UserId(),
            FollowingUserId: profileBar.ViewUser(),
            ModifiedDate: Helper.time()
        }

        AjaxNinja.Invoke(ODataApi.UserFollowing, "POST", JSON.stringify(data), function (data) {

            //if (data.value.length != 0)
                //self.Following(data);
        });

    }

   
}
var profileBar = new ProfileBarViewModel();
ko.applyBindings(profileBar, document.getElementById("profileBarDiv"));
profileBar.ProfileBar();