var TradeItemsViewModel = function () {

    var self = this;
    this.Items = ko.observableArray();
    this.TradeItem = function () {
       
        AjaxNinja.Invoke(ODataApi.Item + "?$expand=ItemImages", "GET", {}, function (data) {
            self.Items(data.value);
        });

    }

   
}
var trade = new TradeItemsViewModel();
ko.applyBindings(trade, document.getElementById("grid"));
trade.TradeItem();