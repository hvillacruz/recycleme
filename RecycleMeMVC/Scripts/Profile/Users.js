var UsersViewModel = function () {
    var self = this;
    this.Followers = ko.observableArray();
    this.Following = ko.observableArray();
    this.UserFollower = function () {

        AjaxNinja.Invoke(ODataApi.User + "('" + global.User.UserId() + "')" + "/UserFollowers?$expand=User", "GET", {}, function (data) {
            if (data.value.length != 0)
                self.Followers(data.value);
        });

    },
    this.UserFollowing = function () {

        AjaxNinja.Invoke(ODataApi.User + "('" + global.User.UserId() + "')" + "/UserFollowing?$expand=User", "GET", {}, function (data) {
            if (data.value.length != 0)
                self.Following(data.value);
        });

    }


}

var users = new UsersViewModel();
ko.applyBindings(users, document.getElementById("usersDiv"));
users.UserFollower();
users.UserFollowing();