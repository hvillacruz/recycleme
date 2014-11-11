var TradeItemsViewModel = function () {

    var self = this;
    this.Items = ko.observableArray();
    this.SelectedItem = ko.observableArray();
    this.TradeItem = function () {


        AjaxNinja.Invoke(ODataApi.Item + "?$filter=IsDeleted eq false&$orderby=ModifiedDate desc&$expand=ItemImages,Owner,ItemCommented,ItemCommented/Commenter,ItemUserFollowers", "GET", {}, function (data) {

            var result = [];
            $(data.value).each(function (index, value) {

                var res = $.extend(value, { CommentText: "", ImageClass: "metro-" + value.ItemImages.length});
                result.push(res);
            });

            self.Items(result);
            self.Refresh();
          
        });
        
    }


   

    this.Search = function (item, data) {
        var result = [];
        AjaxNinja.Invoke(ODataApi.Item + "?$orderby=ModifiedDate desc&$expand=ItemImages,Owner   &$filter=substringof('" + item + "',Name) or substringof('" + item + "',TradeTag)", "GET", {}, function (data) {

            self.Items(data.value);
            self.Refresh();

        });

    }

    this.Refresh = function () {
        setTimeout(function () {
            var scroll = new AnimOnScroll(document.getElementById('grid'), {
                minDuration: 0.4,
                maxDuration: 0.7,
                viewportFactor: 0.2
            });
        }, 100);
    };


    this.SelectedUser = function (item, parent) {

        window.location.href = '/Profile/Dashboard/' + parent.OwnerId;

    }


    this.SelectedImage = function (parent, selectedImage) {
       
        global.SelectedModalImage(selectedImage);
        $(".search-box").hide();
    }


    this.LikeImage = function (data) {

        console.log(data.OwnerId);
        var obj = {
            FollowerId: global.User.UserId(),
            FollowedItemId: data.Id,
            ModifiedDate: Helper.time()
        }

       

        AjaxNinja.Invoke(ODataApi.ItemFollower, "POST", JSON.stringify(obj), function (result) {
            recycleHub.sendNotification("", global.User.UserName() + " Likes your item.", data.OwnerId, 6);
        });

    }
 

}
var trade = new TradeItemsViewModel();

ko.bindingHandlers.button = {
    init: function (element) {
        $(element).click(function () {

        });
    }


}


ko.applyBindings(trade, document.getElementById("tradeContainer"));
trade.TradeItem();
