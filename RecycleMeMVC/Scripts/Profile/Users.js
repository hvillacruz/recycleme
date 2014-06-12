var UsersViewModel = function () {
    var self = this;
    this.Followers = ko.observableArray();
    this.Following = ko.observableArray();
    this.UserFollower = function () {

        AjaxNinja.Invoke(ODataApi.User + "('" + $("#currentUser").data("text") + "')" + "/UserFollowerUsers?$expand=Follower", "GET", {}, function (data) {
            if (data.value.length != 0)
                self.Followers(data.value);
        });

    },
    this.UserFollowing = function () {

        AjaxNinja.Invoke(ODataApi.User + "('" + $("#currentUser").data("text") + "')" + "/UserFollowers?$expand=FollowedUser", "GET", {}, function (data) {
            if (data.value.length != 0)
                self.Following(data.value);
        });

    }

    this.SelectedUser = function (item) {

        window.location.href = '/Profile/Dashboard/' + item.FollowedUserId;

    }

}

var users = new UsersViewModel();
ko.applyBindings(users, document.getElementById("usersDiv"));
users.UserFollower();
users.UserFollowing();