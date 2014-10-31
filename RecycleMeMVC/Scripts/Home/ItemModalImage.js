﻿var ItemModalImageViewModel = function () {

    var self = this;
    this.Items = ko.observableArray();
    this.SelectedItem = ko.observableArray();
    this.ItemImage = function () {



    }

    this.BindImages = function () {

        console.log(global.SelectedModalImage());

        setTimeout(function () {

            $(".imgGallery").nerveSlider({
                slideTransitionSpeed: 500,
                slideTransitionEasing: "easeOutExpo",
                sliderFullscreen: false,
                sliderKeepAspectRatio: true,
                sliderResizable: true,
                sliderWidth: "640px",
                sliderHeight: "480px",
                sliderHeightAdaptable: true
            });


        }, 50);

    }


    this.ConvertMoment = function (date) {

        return formatMoment(date);
    }


    this.RecycleComment = function () {


        var item = global.SelectedModalImage();
        var data = {

            CommenterId: global.Login().UserId,
            CommentedItemId: item.Id.toString(),
            Comment: item.CommentText,
            ModifiedDate: Helper.time()

        }

        AjaxNinja.Invoke(ODataApi.ItemComment, "POST", JSON.stringify(data), function (result) {
            AjaxNinja.Invoke(ODataApi.Item + "(" + item.Id + ")" + "?$expand=Owner,ItemImages,Owner,Category,ItemCommented,ItemCommented/Commenter,ItemUserFollowers", "GET", {}, function (current) {

                var res = $.extend(current, { CommentText: "" });
                $(current).push(res);


                global.SelectedModalImage(current);

                recycleHub.sendNotification("", global.User.UserName() + " Commented on your item", current.OwnerId, 3);
            });
        });

    }


}

setTimeout(function () {
    var itemModalImage = new ItemModalImageViewModel();
    ko.applyBindings(itemModalImage, document.getElementById("itemModalImageContainer"));
    itemModalImage.ItemImage();
    itemModalImage.BindImages();
}, 500);
