var ExchangeViewModel = function () {

    var self = this;
    this.Trade = ko.observableArray();
    this.Selected = ko.observableArray();
    this.TradeItem = function () {

        AjaxNinja.Invoke(ODataApi.Trade + "?$orderby=ModifiedDate desc&$filter=Id eq " + $("#currentItem").data("text") + "&$expand=Seller,Buyer,TradeItem/TradeCommenter,Item/ItemImages", "GET", {}, function (data) {
            self.Trade(data.value);
            self.SelectedItem(data.value[0].ItemId);
        });

    }

    this.SelectedItem = function (id) {

        var result = [];
        AjaxNinja.Invoke(ODataApi.Item + "('" + id + "')?$expand=Owner,ItemImages", "GET", {}, function (data) {
            var res = $.extend(data, { CommentText: "" });
            result.push(res);
            self.Selected(result);
        });

    }


    this.TradeCommentPost = function (data, item) {

        var data = {

            TradeCommenterId: global.User.UserId(),
            TradeId: $("#currentItem").data("text"),
            Comment: item.CommentText,
            ModifiedDate: Helper.time()

        }

        AjaxNinja.Invoke(ODataApi.TradeComment, "POST", JSON.stringify(data), function (data) {
            self.SelectedItem();
            self.TradeItem();
            recycleHub.sendNotification("", global.User.UserName() + " Commented on your trade", self.Selected()[0].OwnerId, 4);
        });

    }

}
var exchange = new ExchangeViewModel();
ko.applyBindings(exchange, document.getElementById("panelContainer"));
exchange.TradeItem();
