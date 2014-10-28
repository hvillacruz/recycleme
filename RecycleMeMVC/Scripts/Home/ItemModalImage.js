var ItemModalImageViewModel = function () {

    var self = this;
    this.Items = ko.observableArray();
    this.SelectedItem = ko.observableArray();
    this.ItemImage = function () {



    }

    this.BindImages = function () {


        setTimeout(function () {

            $(".iAmTest").nerveSlider({
                slideTransitionSpeed: 1000,
                slideTransitionEasing: "easeOutExpo",
                sliderFullscreen: false,
                sliderKeepAspectRatio: true,
                sliderResizable: true,
                //sliderWidth: "1024px",
                //sliderHeight: "768px",
                sliderHeightAdaptable:true
            });


        }, 50);

    }


}
var itemModalImage = new ItemModalImageViewModel();



ko.applyBindings(itemModalImage, document.getElementById("itemModalImageContainer"));
itemModalImage.ItemImage();
itemModalImage.BindImages();