var TradeViewModel = function () {
    var self = this;
    this.Items = ko.observableArray();
    this.BuyersItem = ko.observableArray();
    this.Selected = ko.observableArray();
    this.SortedTrade = ko.observableArray();
    this.currentItems = [];
    this.CurrentItems = function () {

        AjaxNinja.Invoke(ODataApi.User + "('" + global.User.UserId() + "')/Items?$orderby=ModifiedDate desc&$expand=Owner,ItemImages,Category,ItemCommented,ItemUserFollowers", "GET", {}, function (data) {

            var result = [];
            $(data.value).each(function (index, value) {
                var res = $.extend(value, { CommentText: "", ImageClass: "metro-" + value.ItemImages.length });

                result.push(res);
            });
            self.Items(result);


            var foo = document.getElementById("source");


            new Sortable(foo, {
                draggable: '.tile',
                handle: '.tile__name',
                onAdd: function (evt) {
                    self.currentItems.splice($.inArray(evt.item.id, self.currentItems), 1);
                },
                onRemove: function (evt) {
                    self.currentItems.push(evt.item.id);
                }
            });


            [].forEach.call(multi.getElementsByClassName('tile__list'), function (el) {
                new Sortable(el, { group: 'photo' });
            });

        });

    }

    this.SelectedItem = function () {

        var result = [];
        AjaxNinja.Invoke(ODataApi.Item + "('" + $("#currentItem").data("text") + "')?$expand=Owner,ItemImages", "GET", {}, function (data) {
            var res = $.extend(data, { CommentText: "" });
            result.push(res);
            self.Selected(result);
        });

    }


    this.SelectedUser = function (item) {

        window.location.href = '/Profile/Dashboard/' + item.FollowedUserId;

    }


    this.TradeCommentPost = function (data, item) {

        var data = {

            TradeCommenterId: global.User.UserId(),
            TradeId: self.BuyersItem()[0].Id,
            Comment: item.CommentText,
            ModifiedDate: Helper.time()

        }

        AjaxNinja.Invoke(ODataApi.TradeComment, "POST", JSON.stringify(data), function (data) {
            self.SelectedItem();
            self.TradeItem();
            recycleHub.sendNotification("", global.User.UserName() + " Commented on your trade", self.Selected()[0].OwnerId, 5);
        });

    }


    this.TradeComment = function (item) {


        AjaxNinja.Invoke(ODataApi.TradeComment, "GET", {}, function (data) {
            alert('success');
        });

    }


    this.TradeItemBuyer = function () {

        AjaxNinja.Invoke(ODataApi.Trade + "?$orderby=ModifiedDate desc&$filter=ItemId eq " + $("#currentItem").data("text") + " and BuyerId eq '" + global.User.UserId() + "' and Status ne 'OPEN'&$expand=Trades/Item/ItemImages,TradeItem/TradeCommenter", "GET", {}, function (data) {
            self.BuyersItem(data.value);
        });
    }

    this.Trade = ko.observableArray();
    this.TradeItem = function () {

        AjaxNinja.Invoke(ODataApi.Trade + "?$orderby=ModifiedDate desc&$filter=ItemId eq " + $("#currentItem").data("text") + " and BuyerId eq '" + global.User.UserId() + "' and Status ne 'OPEN'&$expand=Seller,Buyer,TradeItem/TradeCommenter", "GET", {}, function (data) {
            self.Trade(data.value);
            if (items.Trade().length > 0) {
                var item = items.Trade()[0].TradeItem.sort(function (left, right) {
                    return left.Id == right.Id ? 0 : (left.Id > right.Id ? -1 : 1);
                });

                items.SortedTrade(item);
            }
        });

    }

    this.Buyer = function () {

    }



    this.TradeItemPost = function (item) {


        var data = {

            BuyerId: global.User.UserId(),
            SellerId: self.Selected()[0].OwnerId,
            ItemId: $("#currentItem").data("text").toString(),
            ModifiedDate: Helper.time(),
            Status: "OPEN"

        }

        AjaxNinja.Invoke(ODataApi.Trade, "POST", JSON.stringify(data), function (data) {

            $(self.currentItems).each(function (index, value) {


                var items = {
                    ItemId: value,
                    TradeId: data.Id,
                    ModifiedDate: Helper.time()

                }

                AjaxNinja.Invoke(ODataApi.TadeBuyerItem, "POST", JSON.stringify(items), function (data) {
                    recycleHub.sendNotification("", global.User.UserName() + " Wants to trade", self.Selected()[0].OwnerId, 4);
                });
            });
        });
    }
}

var items = new TradeViewModel();
ko.applyBindings(items, document.getElementById("panelContainer"));
items.CurrentItems();
items.SelectedItem();
items.TradeItemBuyer();
items.TradeItem();