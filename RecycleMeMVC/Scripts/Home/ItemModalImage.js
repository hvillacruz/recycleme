var ItemModalImageViewModel = function () {

    var self = this;
    this.Items = ko.observableArray();
    this.SelectedItem = ko.observableArray();
    this.ItemImage = function () {



    }

    this.BindImages = function () {

        console.log(global.SelectedModalImage());

        setTimeout(function () {

            $(".imgGallery").nerveSlider({
                slideTransitionSpeed: 500,
                slideTransitionEasing: "easeOutExpo",
                sliderFullscreen: false,
                sliderKeepAspectRatio: true,
                sliderResizable: true,
                sliderWidth: "640px",
                sliderHeight: "480px",
                sliderHeightAdaptable:true
            });


        }, 50);

    }


}
var itemModalImage = new ItemModalImageViewModel();



ko.applyBindings(itemModalImage, document.getElementById("itemModalImageContainer"));
itemModalImage.ItemImage();
itemModalImage.BindImages();