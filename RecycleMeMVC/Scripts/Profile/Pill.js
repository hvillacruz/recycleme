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

        var form = { Name: "", ItemTag: "", TradeTag: "", OwnerId: 0, ModifiedDate: Helper.time(), ItemCategoryId: 0 };
        self.PillView(form);
    }

    this.recycle = function () {

        self.PillView().OwnerId = global.User.UserId();
        self.PillView().ModifiedDate = Helper.time();
        self.PillView().ItemCategoryId = this.SelectedChoice()[0];

        AjaxNinja.Invoke(ODataApi.Item, "POST", JSON.stringify(this.PillView()), function (data) {
            alert(data);
        });

    }

}
var pill = new PillViewModel();
ko.bindingHandlers.kendoMultiSelect.options.filter = "contains";
ko.applyBindings(pill, document.getElementById("pillbox"));
pill.Pill();