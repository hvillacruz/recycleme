var ProfileBarViewModel = function () {


    this.ProfileBar = function () {
        var self = this;
        self.ProfileBar = ko.observableArray();
        self.ProfileBar(global.Profile.slice());

        ko.utils.arrayForEach(self.ProfileBar, function (feature) {
            consol.write(feature);
        });
        ko.applyBindings(self, document.getElementById("ProfileBarDiv"));
        
    }

}
var profileBar = new ProfileBarViewModel();
profileBar.ProfileBar();