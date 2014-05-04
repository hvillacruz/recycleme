var TimeLineViewModel = function () {

    var self = this;
    this.Items = ko.observableArray();

    this.ItemTimeline = function () {

        AjaxNinja.Invoke(ODataApi.User + "('" + $("#currentUser").data("text") + "')/Items?$expand=Owner,ItemImages,Category,ItemCommented,ItemUserFollowers", "GET", {}, function (data) {

            var result = [];
            $(data.value).each(function (index, value) {
                var res = $.extend(value, { CommentText: "" });
                result.push(res);
            });
            self.Items(result);

        });

    }



    this.RecycleComment = function (item) {

        var data = {

            CommenterId: $("#currentUser").data("text"),
            CommentedItemId: item.Id.toString(),
            Comment: item.CommentText,
            ModifiedDate: Helper.time()

        }

        AjaxNinja.Invoke(ODataApi.ItemComment, "POST", JSON.stringify(data), function (data) {

            alert('Success');
        });
    }



}
var timeline = new TimeLineViewModel();
ko.applyBindings(timeline, document.getElementById("timelineDiv"));
timeline.ItemTimeline();