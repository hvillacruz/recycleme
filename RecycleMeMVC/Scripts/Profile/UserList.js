var UserListViewModel = function () {

    var self = this;
    this.MessageCount = ko.observable();
    this.Items = ko.observableArray();
    this.Users = function () {
  

        AjaxNinja.Invoke(ODataApi.User + "('" + global.User.UserId() + "')" + "/UserFollowers?$expand=FollowedUser", "GET", {}, function (data) {
            self.MessageCount( data.value.length);
            self.Items(data.value);
        });

    }

   
}
var user = new UserListViewModel();
ko.applyBindings(user, document.getElementById("grid"));
user.Users();