var TimeLineViewModel = function () {

    var self = this;
    var feed = [];
    this.Items = ko.observableArray();
    this.SelectedItem = ko.observableArray();
    this.message = ko.observable("WTF!");
    this.ItemTimeline = function () {
        feed = [];

        AjaxNinja.Invoke(ODataApi.User + "('" + $("#currentUser").data("text") + "')/Items?$orderby=ModifiedDate desc&$expand=Owner,ItemImages,Category,ItemCommented,ItemCommented/Commenter,ItemUserFollowers", "GET", {}, function (data) {


            $(data.value).each(function (index, value) {
                var res = $.extend(value, { CommentText: "", ImageClass: "metro-" + value.ItemImages.length });

                feed.push(res);
            });


            timeline.Feed();
        });

    }

    this.Feed = function () {


        AjaxNinja.Invoke(ODataApi.User + "('" + $("#currentUser").data("text") + "')/UserFollowers?$expand=FollowedUser,FollowedUser/Items,FollowedUser/Items/ItemImages,FollowedUser/Items/ItemCommented,FollowedUser/Items/ItemUserFollowers,FollowedUser/Items/Owner,FollowedUser/Items/Category", "GET", {}, function (data) {

            if (global.User.UserId() == $("#currentUser").data("text")) {
                _.each(data.value, function (item) {

                    _.each(item.FollowedUser.Items, function (obj) {

                        var res = $.extend(obj, { CommentText: "", ImageClass: "metro-" + obj.ItemImages.length });
                        feed.push(res);
                    });


                });

            }

            self.Items(feed);
            setEditable();
        });
    }


    function setEditable() {
        $.fn.editable.defaults.mode = 'inline';
        $('.editable').editable();

        $('.editable').on('save', function (e, params) {

            var item = {
                CommentText: params.newValue,
                Id: $(this).data('editable').options.pk
            }

            self.ModifyComment(item);
        });


    }

    this.ShowComment = function (currentItem, selectedImage) {

        self.SelectedItem(currentItem);

    }

    this.ShowItemTrade = function (currentItem, selectedImage) {

        self.SelectedItem(selectedImage);
        window.location.href = '/Profile/Trade/' + selectedImage.Id;
    }


    this.ShowUser = function (data) {


        window.location.href = '/Profile/Dashboard/' + data.OwnerId;
    }


    this.SelectedRecycleComment = function (item, selectedImage) {

        PostComment(item, 0);

    }

    this.RecycleComment = function (item) {

        PostComment(item, 1);
    }

    this.ModifyComment = function (item) {

        var data = {
            CommenterId: $("#loginUser").data("text"),
            Comment: item.CommentText,
            ModifiedDate: Helper.time()

        }

        AjaxNinja.Invoke(ODataApi.ItemComment + "(" + item.Id + ")", "PATCH", JSON.stringify(data), function (result) {

        });
    }

    this.DeleteComment = function (item) {

        var data = {
            CommenterId: $("#loginUser").data("text"),
            IsDeleted: true,
            ModifiedDate: Helper.time()

        }

        AjaxNinja.Invoke(ODataApi.ItemComment + "(" + item.Id + ")", "PATCH", JSON.stringify(data), function (result) {

        });
    }



    this.DeleteItem = function (item) {

        var data = {
            IsDeleted: true,
            ModifiedDate: Helper.time()

        }

        AjaxNinja.Invoke(ODataApi.Item + "(" + item.Id + ")", "PATCH", JSON.stringify(data), function (result) {
            alert('success');
        });
    }

    this.PostFb = function (item) {

        var data = {
            UserId: $("#loginUser").data("text")
        }

        AjaxNinja.Invoke(ODataApi.Item + "PostFacebook", "POST", JSON.stringify(data), function (result) {
            alert('success');
        });
    }



    function PostComment(item, type) {

        var data = {

            CommenterId: $("#loginUser").data("text"),
            CommentedItemId: item.Id.toString(),
            Comment: item.CommentText,
            ModifiedDate: Helper.time()

        }

        AjaxNinja.Invoke(ODataApi.ItemComment, "POST", JSON.stringify(data), function (result) {



            if (type == 1) {

                AjaxNinja.Invoke(ODataApi.Item + "(" + item.Id + ")" + "?$expand=Owner,ItemImages,Category,ItemCommented,ItemUserFollowers", "GET", {}, function (current) {

                    recycleHub.sendNotification("", global.User.UserName() + " Commented on your item", current.OwnerId, 3);

                    if (global.User.UserName() != current.OwnerId)
                        timeline.ItemTimeline();
                });

            }
            else {


                AjaxNinja.Invoke(ODataApi.Item + "(" + item.Id + ")" + "?$expand=Owner,ItemImages,Category,ItemCommented,ItemUserFollowers", "GET", {}, function (current) {

                    var res = $.extend(current, { CommentText: "" });
                    $(current).push(res);

                    self.SelectedItem(current);

                    recycleHub.sendNotification("", global.User.UserName() + " Commented on your item", current.OwnerId, 3);
                });
            }
        });
    }

}



var timeline = new TimeLineViewModel();
ko.applyBindings(timeline, document.getElementById("timelineDiv"));
timeline.ItemTimeline();
