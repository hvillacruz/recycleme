var UserListViewModel = function () {

    var self = this;
    this.MessageCount = ko.observable();
    this.Type = ko.observable("Following");
    this.Items = ko.observableArray();
    this.Users = function () {

        var result = window.location.href.indexOf("Followers") < 0 ? this.Type("Following") : this.Type("Follower");
        var type = window.location.href.indexOf("Followers") < 0 ? "/UserFollowers?$expand=FollowedUser" : "/UserFollowerUsers?$expand=Follower";
        AjaxNinja.Invoke(ODataApi.User + "('" + global.User.UserId() + "')" + type, "GET", {}, function (data) {
            self.MessageCount(data.value.length);
            self.Items(data.value);
            self.Refresh();
        });

    }

   
    this.SelectedUser = function (item, parent) {

        window.location.href = '/Profile/Dashboard/' + parent.FollowerId;

    }

    this.SelectedUserFollowing = function (item, parent) {

        window.location.href = '/Profile/Dashboard/' + parent.FollowedUserId;
    }



    this.Refresh = function () {
        setTimeout(function () {
            var scroll = new AnimOnScroll(document.getElementById('grid'), {
                minDuration: 0.4,
                maxDuration: 0.7,
                viewportFactor: 0.2
            });
        }, 100);
    };


}
var user = new UserListViewModel();
ko.applyBindings(user, document.getElementById("grid"));
user.Users();