var ProfileBarViewModel = function () {

    var self = this;
    this.Bar = ko.observableArray();
    this.ProfileBar = function () {

        AjaxNinja.Invoke(ODataApi.User + "('" + global.User.UserId() + "')" + "?$expand=UserFollowerUsers,UserCommenter,UserFollowing", "GET", {}, function (data) {
            self.Bar(data);
        });

    }

    this.likeUser = function () {
    
        var data = {
            FollowerId: "0fecf5f4-b0f4-42be-b016-414df2bf76d6",
            FollowedUserId: "bed7cfdc-4e83-46da-b7c4-1a9ddfe39d0b",
            ModifiedDate: "2014-03-27T10:19:19.217"
        }
        AjaxNinja.Invoke(ODataApi.UserFollow, "POST", JSON.stringify(data), function (data) {
            alert('LikeUser');
            if (data.value.length != 0)
                self.Following(data.value);
        });

    }

}
var profileBar = new ProfileBarViewModel();
ko.applyBindings(profileBar, document.getElementById("profileBarDiv"));
profileBar.ProfileBar();