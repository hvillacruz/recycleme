var TradeViewModel = function () {
    var self = this;
    this.Status = ko.observable("Pending");
    this.HasNotTraded = ko.observable(false);
    this.HasItem = ko.observable(false);
    this.IsClosed = ko.observable(false);
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
            if(data.value.length > 0)
                self.HasItem(true);
            else
                self.HasItem(false);
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


            $("#panelContainer").show(); 
        });

    }

    this.SelectedItem = function () {

        var result = [];
        AjaxNinja.Invoke(ODataApi.Item + "('" + $("#currentItem").data("text") + "')?$expand=Owner,ItemImages,ItemTrades", "GET", {}, function (data) {
            var res = $.extend(data, { CommentText: "" });
            result.push(res);
            self.Selected(result);
        });

    }


    this.SelectedUser = function (item) {

        window.location.href = '/Profile/Dashboard/' + item.FollowedUserId;

    }

    this.IsNotApprove = ko.computed(function () {
        return self.Status() != "Approve" && self.Status() != "Reject" ? true : false;
    });



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

        self.BuyersItem([]);
       
        AjaxNinja.Invoke(ODataApi.Trade + "?$orderby=ModifiedDate desc&$filter=ItemId eq " + $("#currentItem").data("text") + " and BuyerId eq '" + global.User.UserId() + "' and Status ne 'Deleted'&$expand=Trades/Item/ItemImages,TradeItem/TradeCommenter", "GET", {}, function (data) {
            
            $(data.value[0].Trades).each(function (index, value) {
                self.currentItems.push(value.ItemId);
            });
         
            self.BuyersItem(data.value);

        });
    }

   

    this.Trade = ko.observableArray();
    this.TradeItem = function () {

        AjaxNinja.Invoke(ODataApi.Trade + "?$orderby=ModifiedDate desc&$filter=ItemId eq " + $("#currentItem").data("text") + " and BuyerId eq '" + global.User.UserId() + "' and Status ne 'Deleted'&$expand=Seller,Buyer,TradeItem/TradeCommenter", "GET", {}, function (data) {
            if (data.value.length > 0) {
                self.HasNotTraded(false);
                self.Trade(data.value);
                self.Status(data.value[0].Status);
                if (items.Trade().length > 0) {
                    var item = items.Trade()[0].TradeItem.sort(function (left, right) {
                        return left.Id == right.Id ? 0 : (left.Id > right.Id ? -1 : 1);
                    });

                    items.SortedTrade(item);
                }
            }
            else {
                self.TradeItemPost(null);
                self.HasNotTraded(false);
            }
        });

    }

    this.Buyer = function () {

    }



    this.TradeItemPost = function (obj) {


        var data = {

            BuyerId: global.User.UserId(),
            SellerId: self.Selected()[0].OwnerId,
            ItemId: $("#currentItem").data("text").toString(),
            ModifiedDate: Helper.time(),
            Status: "OPEN"

        }

        AjaxNinja.Invoke(ODataApi.Trade, "POST", JSON.stringify(data), function (data) {
            items.TradeItemBuyer();
            $(self.currentItems).each(function (index, value) {


                var item = {
                    ItemId: value,
                    TradeId: data.Id,
                    ModifiedDate: Helper.time()

                }

                AjaxNinja.Invoke(ODataApi.TadeBuyerItem, "POST", JSON.stringify(item), function (data) {
                  
                    recycleHub.sendNotification("", global.User.UserName() + " Wants to trade", self.Selected()[0].OwnerId, 4);
                });
            });
        });
    }



    this.CheckItemStatus = function (obj) {

        AjaxNinja.Invoke(ODataApi.Item + "(" + obj.Selected()[0].Id + ")", "GET", {}, function (data) {
            if (data.Status != 1) {
                self.TradeItemPatch(obj);
            }
            else {
                self.IsClosed(true);
            }
        });
    }



    this.TradeItemPatch = function (obj) {


        var TradeId = {
            TradeId: obj.Selected()[0].ItemTrades[0].Id.toString()
        }


     

        AjaxNinja.Invoke(ODataApi.TadeBuyerItem + "/TradeBuyerItemDelete", "POST", JSON.stringify(TradeId), function (data) {
            $(self.currentItems).each(function (index, value) {


                var item = {
                    ItemId: value,
                    TradeId: obj.Selected()[0].ItemTrades[0].Id,
                    ModifiedDate: Helper.time()

                }

                AjaxNinja.Invoke(ODataApi.TadeBuyerItem, "POST", JSON.stringify(item), function (data) {
                    //items.TradeItemBuyer();
                   //recycleHub.sendNotification("", global.User.UserName() + " Wants to trade a new item", self.Selected()[0].OwnerId, 4);
                });



            });

            var dataUp = {
                Status: 'Open',
                ModifiedDate: Helper.time()

            }

            AjaxNinja.Invoke(ODataApi.Trade + "(" + TradeId.TradeId + ")", "PATCH", JSON.stringify(dataUp), function (result) {
                recycleHub.sendNotification("", global.User.UserName() + " Wants to trade a new item", self.Selected()[0].OwnerId, 4);
            });


        });
    }
}

var items = new TradeViewModel();
ko.applyBindings(items, document.getElementById("panelContainer"));
items.TradeItemBuyer();
items.CurrentItems();
items.SelectedItem();
items.TradeItem();