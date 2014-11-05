var BidViewModel = function () {

    var self = this;
    this.Items = ko.observableArray();
    this.SelectedItem = ko.observableArray();
    this.BidItem = function () {

        //?$filter=ItemId%20eq%207&$expand=Trades/Item,Buyer
        AjaxNinja.Invoke(ODataApi.Trade + "?$filter=ItemId eq " + $("#ItemId").text() + "&$expand=Trades/Item,Buyer,Item,Item/ItemImages", "GET", {}, function (data) {

            var result = [];
            $(data.value).each(function (index, value) {

                var res = $.extend(value, { CommentText: "", ImageClass: "metro-" + value.Item.ItemImages.length });
                result.push(res);
            });

            self.Items(result);
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


    


  
 

}
var bid = new BidViewModel();

ko.bindingHandlers.button = {
    init: function (element) {
        $(element).click(function () {

        });
    }


}


ko.applyBindings(bid, document.getElementById("bidContainer"));
bid.BidItem();
