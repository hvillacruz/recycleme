var TradeItemsViewModel = function () {

    var self = this;
    this.Items = ko.observableArray();
    this.TradeItem = function () {
       
        AjaxNinja.Invoke(ODataApi.Item + "?$orderby=ModifiedDate desc&$expand=ItemImages", "GET", {}, function (data) {
            self.Items(data.value);
            self.Refresh();
        });

    }

    this.Search = function (item,data) {
        var result = [];
        AjaxNinja.Invoke(ODataApi.Item + "?$orderby=ModifiedDate desc&$expand=ItemImages&$filter=substringof('" + item + "',Name) or substringof('" + item + "',TradeTag)", "GET", {}, function (data) {
          
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

   
}
var trade = new TradeItemsViewModel();
ko.applyBindings(trade, document.getElementById("tradeContainer"));
trade.TradeItem();