var PillViewModel = function () {

    var self = this;
    this.Category = ko.observableArray();
    this.PillView = ko.observableArray();
    this.SelectedChoice = ko.observable();
    this.Pill = function () {
        var myObservableArray = ko.observableArray();    // Initially an empty array
        myObservableArray.push('Some value');

        AjaxNinja.Invoke(ODataApi.ItemCategory, "GET", null, function (data) {

            if (data.value.length != 0)
                self.Category(data.value);
        });

        var form = { Name: "", ExchangeTag: "Exchange", TradeTag: "Trade", OwnerId: 0, ModifiedDate: Helper.time(), ItemCategoryId: 0 };
        self.PillView(form);
    }

    this.recycle = function () {

        self.PillView().OwnerId = global.User.UserId();
        self.PillView().ModifiedDate = Helper.time();
        self.PillView().ItemCategoryId = this.SelectedChoice()[0];
        self.PillView().TradeTag = $("#tagTrade").text().replace("×", "").replace("# ", "");
        self.PillView().ExchangeTag = $("#tagExchange").text().replace("×", "").replace("# ", "");

        AjaxNinja.Invoke(ODataApi.Item, "POST", JSON.stringify(this.PillView()), function (data) {

            $.each(ImageIdResult, function (key, value) {
                var form = { Id: "" + value.Id + "", IsDeleted: false, ItemId: data.Id, Name: value.Name, Path: value.Path };
                AjaxNinja.Invoke(ODataApi.ItemImage + "(" + value.Id + ")", "PUT", JSON.stringify(form), function (data) {
                    ImageIdResult = [];
                });
            });
        });




    }

}
var pill = new PillViewModel();
ko.bindingHandlers.kendoMultiSelect.options.filter = "contains";
ko.applyBindings(pill, document.getElementById("pillbox"));
pill.Pill();