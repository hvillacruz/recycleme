var ExchangeViewModel = function () {

    var self = this;
    this.Comment = ko.observableArray();
    this.Comments = function () {

        AjaxNinja.Invoke(ODataApi.User + "('" + global.User.UserId() + "')" + type, "GET", {}, function (data) {

            self.Comment(data.value);

        });

    }

    this.Buyer = function () {

    }

}
var exchange = new ExchangeViewModel();
ko.applyBindings(exchange, document.getElementById("panelContainer"));
exchange.Comments();