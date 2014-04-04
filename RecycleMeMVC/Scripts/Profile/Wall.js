var WallViewModel = function () {

    var self = this;
    this.Info = ko.observableArray();
    this.Wall = function () {

        //AjaxNinja.Invoke(ODataApi.User + "('" + $("#currentUser").data("text") + "')" + "?$expand=UserFollowerUsers,UserCommenter,UserFollowing", "GET", {}, function (data) {
        //    self.Info(data);
        //});
        var data = { title: "Hello Wall" };
        self.Info(data);
    }






}
var wall = new WallViewModel();
ko.applyBindings(wall, document.getElementById("wallDiv"));
wall.Wall();