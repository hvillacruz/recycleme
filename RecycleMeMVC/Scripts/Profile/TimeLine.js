var TimeLineViewModel = function () {

    var self = this;
    this.Items = ko.observableArray();
    this.SelectedItem = ko.observableArray();
    this.message = ko.observable("Welcome to camp!");
    this.ItemTimeline = function () {

        AjaxNinja.Invoke(ODataApi.User + "('" + $("#currentUser").data("text") + "')/Items?$orderby=ModifiedDate desc&$expand=Owner,ItemImages,Category,ItemCommented,ItemUserFollowers", "GET", {}, function (data) {

            var result = [];
            $(data.value).each(function (index, value) {
                var res = $.extend(value, { CommentText: "", ImageClass: "metro-" + value.ItemImages.length });
                // $.extend(res.ItemImages, { Class: "metro" - value.ItemImages.length });

                result.push(res);
            });
            self.Items(result);
            setEditable();
        });

    }

    function setEditable() {
        $.fn.editable.defaults.mode = 'inline';
        $('.editable').editable('submit', {
            url: '/newuser',
            ajaxOptions: {
                dataType: 'json' //assuming json response
            },
            success: function (data, config) {
                alert('ss');
                if (data && data.id) {  //record created, response like {"id": 2}
                    //set pk
                    $(this).editable('option', 'pk', data.id);
                    //remove unsaved class
                    $(this).removeClass('editable-unsaved');
                    //show messages
                    var msg = 'New user created! Now editables submit individually.';
                    $('#msg').addClass('alert-success').removeClass('alert-error').html(msg).show();
                    $('#save-btn').hide();
                    $(this).off('save.newuser');
                } else if (data && data.errors) {
                    //server-side validation error, response like {"errors": {"username": "username already exist"} }
                    config.error.call(this, data.errors);
                }
            },
            error: function (errors) {
                alert('error');
                var msg = '';
                if (errors && errors.responseText) { //ajax error, errors = xhr object
                    msg = errors.responseText;
                } else { //validation error (client-side or server-side)
                    $.each(errors, function (k, v) { msg += k + ": " + v + "<br>"; });
                }
                $('#msg').removeClass('alert-success').addClass('alert-error').html(msg).show();
            }
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

                //http://localhost:53481/odata/Item(3)?$expand=Owner,ItemImages,Category,ItemCommented,ItemUserFollowers
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




jQuery(function ($) {


});