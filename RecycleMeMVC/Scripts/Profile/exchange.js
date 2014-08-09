var ExchangeViewModel = function () {

    var self = this;
    this.Trade = ko.observableArray();
    this.TradeItem = function () {

        AjaxNinja.Invoke(ODataApi.Trade + "?$orderby=ModifiedDate desc&$filter=Id eq " + $("#currentItem").data("text") + "&$expand=Seller,Buyer,TradeItem/TradeCommenter", "GET", {}, function (data) {
            self.Trade(data.value);
        });

    }

    this.Buyer = function () {

    }

}
var exchange = new ExchangeViewModel();
ko.applyBindings(exchange, document.getElementById("panelContainer"));
exchange.TradeItem();