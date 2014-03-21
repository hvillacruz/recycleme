var UsersViewModel = function () {

    var self = this;
    self.Users = ko.observableArray();
    self.Users.Avatar = null;
    self.Users.FirstName = "";
    this.UserFollower = function () {
 
        if (BrowserDetect.browser != "Safari") {
        
            ODataNinja.Read(ODataApi.ProfileBar + "('" + global.User.UserId() + "')" + "/UserFollowers?$expand=User", function (data) {
               
                self.Users(data.results[0].User);
                //ko.applyBindings(self,$("#followersDiv"));
            });
        } else {
          
            AjaxNinja.Invoke(ODataApi.ProfileBar + "('" + global.User.UserId() + "')" + "/UserFollowers?$expand=User", "GET", {}, function (data) {
             
                self.Users(data.results[0].User);
                //ko.applyBindings(self, $("#followersDiv"));
            });

        }
    }

}

var users = new UsersViewModel();
ko.applyBindings(users, document.getElementById("followersDiv"));
users.UserFollower();