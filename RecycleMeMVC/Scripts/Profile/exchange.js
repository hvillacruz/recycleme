var ExchangeViewModel = function () {

    var self = this;
    this.Status = ko.observable("Pending");
    this.Trade = ko.observableArray();
    this.BuyersItem = ko.observableArray();
    this.Selected = ko.observableArray();
    this.SortedTrade = ko.observableArray();
    this.ItemId = ko.observable(0);
    this.TradeItem = function () {

        AjaxNinja.Invoke(ODataApi.Trade + "?$orderby=ModifiedDate desc&$filter=Id eq " + $("#currentItem").data("text") + "&$expand=Seller,Buyer,TradeItem/TradeCommenter,Item/ItemImages", "GET", {}, function (data) {
            self.Trade(data.value);
            self.SelectedItem(data.value[0].ItemId);
            self.ItemId(data.value[0].ItemId);
            self.Status(data.value[0].Status);
            var item = exchange.Trade()[0].TradeItem.sort(function (left, right) {
                return left.Id == right.Id ? 0 : (left.Id > right.Id ? -1 : 1);
            });

            exchange.SortedTrade(item);

        });

    }


    this.SelectedItem = function (id) {

        var result = [];
        AjaxNinja.Invoke(ODataApi.Item + "('" + id + "')?$expand=Owner,ItemImages", "GET", {}, function (data) {
            var res = $.extend(data, { CommentText: "" });
            result.push(res);
            self.Selected(result);
        });

        self.TradeItemBuyer(id, self.Trade()[0].BuyerId);

    }

    this.TradeItemBuyer = function (id, buyer) {

        AjaxNinja.Invoke(ODataApi.Trade + "?$orderby=ModifiedDate desc&$filter=ItemId eq " + id + " and BuyerId eq '" + buyer + "' and Status ne 'Deleted'&$expand=Trades/Item/ItemImages,TradeItem/TradeCommenter", "GET", {}, function (data) {
            self.BuyersItem(data.value);
        });
    }



    this.TradeCommentPost = function (data, item) {

        var data = {

            TradeCommenterId: global.User.UserId(),
            TradeId: $("#currentItem").data("text").toString(),
            Comment: item.CommentText,
            ModifiedDate: Helper.time()

        }

        AjaxNinja.Invoke(ODataApi.TradeComment, "POST", JSON.stringify(data), function (data) {

            self.TradeItem();
            recycleHub.sendNotification("", global.User.UserName() + " Commented on your trade", self.Trade()[0].BuyerId, 5);
        });

    }

    this.ChangeStatus = function () {

        var data = {
            Status: self.Status(),
            ModifiedDate: Helper.time()

        }

        var status = self.Status() == 'Approve' ? "Approved" : "Rejected";
        self.UpdateItem(1);//0-OPEN;1-CLOSED
        AjaxNinja.Invoke(ODataApi.Trade + "(" + $("#currentItem").data("text") + ")", "PATCH", JSON.stringify(data), function (result) {
            recycleHub.sendNotification("", global.User.UserName() + " " + status + " your offer!", self.Trade()[0].BuyerId, 6);
        });
    }



    this.UpdateItem = function (status) {

        var data = {
            Status: status,
            ModifiedDate: Helper.time()

        }

        AjaxNinja.Invoke(ODataApi.Item + "(" + self.ItemId() + ")", "PATCH", JSON.stringify(data), function (result) {
            
        });
    }


    $("#status.dropdown-menu li a").click(function () {

        //$(".status.btn:first-child").text($(this).text());
        //$(".status.btn:first-child").val($(this).text());

        self.Status($(this).text());
    });

}
var exchange = new ExchangeViewModel();
ko.applyBindings(exchange, document.getElementById("panelContainer"));
exchange.TradeItem();
