var TimeLineViewModel = function () {

    var self = this;
    this.Items = ko.observableArray();
    this.SelectedItem = ko.observableArray();
    this.ItemTimeline = function () {

        AjaxNinja.Invoke(ODataApi.User + "('" + $("#currentUser").data("text") + "')/Items?$orderby=ModifiedDate desc&$expand=Owner,ItemImages,Category,ItemCommented,ItemUserFollowers", "GET", {}, function (data) {

            var result = [];
            $(data.value).each(function (index, value) {
                var res = $.extend(value, { CommentText: "", ImageClass: "metro-" + value.ItemImages.length });
                // $.extend(res.ItemImages, { Class: "metro" - value.ItemImages.length });

                result.push(res);
            });
            self.Items(result);

        });

    }


    this.ShowComment = function (currentItem, selectedImage) {

        self.SelectedItem(currentItem);

    }

    this.SelectedRecycleComment = function (item, selectedImage) {

        PostComment(item, 0);
   
    }

    this.RecycleComment = function (item) {

        PostComment(item,1);
    }

    function PostComment(item,type) {

        var data = {

            CommenterId: $("#currentUser").data("text"),
            CommentedItemId: item.Id.toString(),
            Comment: item.CommentText,
            ModifiedDate: Helper.time()

        }

        AjaxNinja.Invoke(ODataApi.ItemComment, "POST", JSON.stringify(data), function (result) {
            if (type == 1)
                timeline.ItemTimeline();
            else {
              
                //http://localhost:53480/odata/Item(3)?$expand=Owner,ItemImages,Category,ItemCommented,ItemUserFollowers
               // data.Id = result.Id;
                AjaxNinja.Invoke(ODataApi.Item+ "("+ item.Id +")" + "?$expand=Owner,ItemImages,Category,ItemCommented,ItemUserFollowers", "GET", {}, function (current) {

                    var res = $.extend(current, { CommentText: "" });
                   

                    $(current).push(res);

                    self.SelectedItem(current);
                });
            }
        });
    }

}
var timeline = new TimeLineViewModel();
ko.applyBindings(timeline, document.getElementById("timelineDiv"));
timeline.ItemTimeline();


jQuery(function ($) {


});