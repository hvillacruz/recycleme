var UserListViewModel = function () {

    var self = this;
    this.MessageCount = ko.observable();
    this.Items = ko.observableArray();
    this.Users = function () {
  
        var type = window.location.href.indexOf("Followers") < 0 ? "/UserFollowers?$expand=FollowedUser" : "/UserFollowerUsers?$expand=Follower";
        AjaxNinja.Invoke(ODataApi.User + "('" + global.User.UserId() + "')" + type, "GET", {}, function (data) {
            self.MessageCount( data.value.length);
            self.Items(data.value);
        });

    }

   
}
var user = new UserListViewModel();
ko.applyBindings(user, document.getElementById("grid"));
user.Users();