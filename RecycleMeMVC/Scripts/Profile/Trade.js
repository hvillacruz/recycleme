var TradeViewModel = function () {
    var self = this;
    this.Items = ko.observableArray();
    this.BuyersItem = ko.observableArray();
    this.Selected = ko.observableArray();
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
                    self.currentItems.splice($.inArray(evt.item.id, currentItems), 1);
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

        AjaxNinja.Invoke(ODataApi.Item + "('" + $("#currentItem").data("text") + "')?$expand=Owner,ItemImages", "GET", {}, function (data) {

            self.Selected(data);

        });

    }


    this.SelectedUser = function (item) {

        window.location.href = '/Profile/Dashboard/' + item.FollowedUserId;

    }
    this.TradeComment = function (item) {

        var data = {

            TradeCommenterId: global.User.UserId(),
            TradeId: "1",//item.Id,
            Comment: "sample",//item.CommentText,
            ModifiedDate: Helper.time()

        }

        AjaxNinja.Invoke(ODataApi.TradeComment, "POST", JSON.stringify(data), function (data) {
            alert('success');
        });

    }


    this.TradeItem = function (){
     
        AjaxNinja.Invoke(ODataApi.Trade + "?$orderby=ModifiedDate desc&$filter=ItemId eq " + $("#currentItem").data("text") + " and BuyerId eq '" + global.User.UserId() + "' and Status eq 'OPEN'&$expand=Item/ItemImages", "GET", {}, function (data) {
            self.BuyersItem(data.value);
        });
    }



    this.TradeItemPost = function (item) {


        var data = {

            BuyerId: global.User.UserId(),
            SellerId: self.Selected().OwnerId,
            ItemId: $("#currentItem").data("text").toString(),
            ModifiedDate: Helper.time(),
            Status: "OPEN"

        }

        AjaxNinja.Invoke(ODataApi.Trade, "POST", JSON.stringify(data), function (data) {

            var items = {
                ItemId: $("#currentItem").data("text").toString(),
                TradeId: data.Id,
                ModifiedDate: Helper.time()

            }
            
            AjaxNinja.Invoke(ODataApi.TadeBuyerItem, "POST", JSON.stringify(items), function (data) {

            });

        });
    }
}

var items = new TradeViewModel();
ko.applyBindings(items, document.getElementById("tradeItemContainer"));
items.CurrentItems();
items.SelectedItem();
items.TradeItem();