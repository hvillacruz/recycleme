var UsersViewModel = function () {
    var self = this;
    this.Users = ko.observableArray();
    this.UserFollower = function () {
        if (BrowserDetect.browser != "Safari") {
            ODataNinja.Read(ODataApi.ProfileBar + "('" + global.User.UserId() + "')" + "/UserFollowers?$expand=User", function (data) {
                if (data.results.length != 0)
                    self.Users(data.results);
            });
        } else {
            AjaxNinja.Invoke(ODataApi.ProfileBar + "('" + global.User.UserId() + "')" + "/UserFollowers?$expand=User", "GET", {}, function (data) {
                if (data.results.length != 0)
                    self.Users(data.results[0].User);
            });

        }
    }

}

var users = new UsersViewModel();
ko.applyBindings(users, document.getElementById("followersDiv"));
users.UserFollower();