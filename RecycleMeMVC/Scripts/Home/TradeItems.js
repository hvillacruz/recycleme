
var TradeItemsViewModel = function () {

    var self = this;
    this.Items = ko.observableArray();
    this.Message = ko.observableArray();
    this.TradeItem = function () {

        AjaxNinja.Invoke(ODataApi.Item + "?$orderby=ModifiedDate desc&$expand=ItemImages,Owner", "GET", {}, function (data) {
            self.Items(data.value);
            self.Refresh();
        });

        self.GetMessage();
    }

    this.Search = function (item, data) {
        var result = [];
        AjaxNinja.Invoke(ODataApi.Item + "?$orderby=ModifiedDate desc&$expand=ItemImages,Owner&$filter=substringof('" + item + "',Name) or substringof('" + item + "',TradeTag)", "GET", {}, function (data) {

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

    this.GetMessage = function () {



        AjaxNinja.Invoke(ODataApi.Message + "?$filter=ReceiverId eq '" + global.User.UserId() + "'&$orderby=DateReceived desc&$expand=Sender", "GET", {}, function (data) {
            self.MessageCount("(" + data.value.length + ")");
            var result = [];
            $(data.value).each(function (i, value) {
                var date = new Date(Date.parse(value.DateSent));
                value.DateSent = date;
                if (date != null)
                    var res = $.extend(value, { Time: formatAMPM(date) });

                result.push(res);


            });

            self.Message(result);

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