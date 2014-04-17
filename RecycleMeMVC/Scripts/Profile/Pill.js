var PillViewModel = function () {

    var self = this;
    this.Category = ko.observableArray();
    this.Pill = function () {

        
        self.Category(Categories.List);
        

    }

    this.likeUser = function () {

  
        AjaxNinja.Invoke(ODataApi.UserFollow, "POST", JSON.stringify(data), function (data) {

            if (data.value.length != 0)
                self.Following(data.value);
        });

    }

}
var pill = new PillViewModel();
ko.applyBindings(pill, document.getElementById("pillDiv"));
pill.Pill();