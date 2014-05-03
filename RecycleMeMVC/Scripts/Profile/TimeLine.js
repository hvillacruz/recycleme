var TimeLineViewModel = function () {

    var self = this;
    this.Items = ko.observableArray();
    
    this.ItemTimeline = function () {
         alert("ProfileBar-" + $("#currentUser").data("text"));
        
        //http://localhost:53480/odata/Item(6)/ItemImages?$expand=Item
         AjaxNinja.Invoke(ODataApi.User + "('" + $("#currentUser").data("text") + "')/Items?$expand=ItemImages,Category,ItemCommented,ItemUserFollowers", "GET", {}, function (data) {
             self.Items(data.value);
        });

    }

    this.likeUser = function () {

        var data = {
            FollowerId: "0fecf5f4-b0f4-42be-b016-414df2bf76d6",
            FollowedUserId: "bed7cfdc-4e83-46da-b7c4-1a9ddfe39d0b",
            ModifiedDate: "2014-03-27T10:19:19.217"
        }
        AjaxNinja.Invoke(ODataApi.UserFollow, "POST", JSON.stringify(data), function (data) {

            if (data.value.length != 0)
                self.Following(data.value);
        });

    }

  

}
var timeline = new TimeLineViewModel();
ko.applyBindings(timeline, document.getElementById("timelineDiv"));
timeline.ItemTimeline();