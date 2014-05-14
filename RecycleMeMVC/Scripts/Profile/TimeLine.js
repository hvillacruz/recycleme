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

        PostComment(item, 1);
    }

    this.ModifyComment = function (item) {

        var data = {
            CommenterId: $("#currentUser").data("text"),
            Comment: item.CommentText,
            ModifiedDate: Helper.time()

        }

        AjaxNinja.Invoke(ODataApi.ItemComment + "(" + item.Id + ")", "PATCH", JSON.stringify(data), function (result) {
            alert(result);
        });
    }

    this.DeleteComment = function (item) {

        var data = {
            CommenterId: $("#currentUser").data("text"),
            IsDeleted: true,
            ModifiedDate: Helper.time()

        }

        AjaxNinja.Invoke(ODataApi.ItemComment + "(" + item.Id + ")", "PATCH", JSON.stringify(data), function (result) {
            alert(result);
        });
    }


    function PostComment(item, type) {

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
                AjaxNinja.Invoke(ODataApi.Item + "(" + item.Id + ")" + "?$expand=Owner,ItemImages,Category,ItemCommented,ItemUserFollowers", "GET", {}, function (current) {

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



//ko.bindingHandlers.hidden = {
//    update: function (element, valueAccessor) {
//        ko.bindingHandlers.visible.update(element, function () { return !ko.utils.unwrapObservable(valueAccessor()); });
//    }
//};

//ko.bindingHandlers.clickToEdit = {
//    init: function (element, valueAccessor) {
//        var observable = valueAccessor(),
//            link = document.createElement("a"),
//            input = document.createElement("input");

//        element.appendChild(link);
//        element.appendChild(input);

//        observable.editing = ko.observable(false);

//        ko.applyBindingsToNode(link, {
//            text: observable,
//            hidden: observable.editing,
//            click: function () { observable.editing(true); }
//        });

//        ko.applyBindingsToNode(input, {
//            value: observable,
//            visible: observable.editing,
//            hasfocus: observable.editing
//        });
//    }
//};

//ko.applyBindings({ message: ko.observable("Welcome to camp!") });



jQuery(function ($) {


});