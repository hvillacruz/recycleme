var ItemModalImageViewModel = function () {

    var self = this;
    this.Items = ko.observableArray();
    this.SelectedItem = ko.observableArray();
    this.ItemImage = function () {

       
    }

  
 

}
var itemModalImage = new ItemModalImageViewModel();



ko.applyBindings(itemModalImage, document.getElementById("itemModalImageContainer"));
itemModalImage.ItemImage();
