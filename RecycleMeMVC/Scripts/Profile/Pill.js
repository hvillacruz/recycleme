var PillViewModel = function () {

    var self = this;
    this.Category = ko.observableArray();
    this.PillView = ko.observableArray();
    this.Form = ko.observableArray();
    this.SelectedChoice =  ko.observable();
    this.Pill = function () {
        var myObservableArray = ko.observableArray();    // Initially an empty array
        myObservableArray.push('Some value');

        AjaxNinja.Invoke(ODataApi.ItemCategory, "GET", null, function (data) {

            if (data.value.length != 0)
                self.Category(data.value);
        });

        var form = { Name: "", ItemTag: "Tag", TradeTag: "Exchange" };
        self.PillView(form);
    }

    this.recycle = function () {
        alert('recycle');
        //{
        //    "OwnerId":"bc943640-22f8-42d3-8646-c9e40938f34b",
        //    "Name":"Item 1",
        //    "ImagePath":"item.jpg",
        //    "Description":"",
        //    "ItemTag":"test",
        //    "TradeTag":"trade",
        //    "IsDeleted":false,
        //    "ModifiedDate":"2014-04-20T21:37:21",
        //    "ItemCategoryId":1
        //}

        var data = {
            OwnerId: global.User.UserId(),
            ModifiedDate: Helper.time(),
            ItemCategoryId: this.SelectedChoice()[0]
        }
        
        self.Form(self.PillView());
        self.Form.push(data);
        //self.PillView.push(self.Form());
        AjaxNinja.Invoke(ODataApi.ItemCategory, "POST", JSON.stringify(this.PillView()), function (data) {
            alert(data);
        });

    }

}
var pill = new PillViewModel();
ko.bindingHandlers.kendoMultiSelect.options.filter = "contains";
ko.applyBindings(pill, document.getElementById("pillbox"));
pill.Pill();