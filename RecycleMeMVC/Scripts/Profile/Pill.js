var PillViewModel = function () {

    var self = this;
    this.Category = ko.observableArray();
    this.PillView = ko.observableArray();
    this.SelectedChoice = ko.observable();
    this.Pill = function () {
        
        AjaxNinja.Invoke(ODataApi.ItemCategory, "GET", null, function (data) {

            if (data.value.length != 0)
                self.Category(data.value);
        });

        var form = { Name: "", ExchangeTag: "Exchange", TradeTag: "Trade", OwnerId: 0, ModifiedDate: Helper.time(), ItemCategoryId: 0 };
        self.PillView(form);
    }

    this.Recycle = function () {

     
        self.PillView().OwnerId = global.User.UserId();
        self.PillView().ModifiedDate = Helper.time();
        self.PillView().ItemCategoryId = this.SelectedChoice()[0];
        self.PillView().TradeTag = S($("#tagTrade").text()).replaceAll('×', '').replaceAll('#', '').s;
        self.PillView().ExchangeTag = S($("#tagExchange").text()).replaceAll('×', '').replaceAll('#', '').s;

        AjaxNinja.Invoke(ODataApi.Item, "POST", JSON.stringify(this.PillView()), function (data) {

            $.each(ImageIdResult, function (key, value) {
                var form = { Id: "" + value.Id + "", IsDeleted: false, ItemId: data.Id, Name: value.Name, Path: value.Path };
                AjaxNinja.Invoke(ODataApi.ItemImage + "(" + value.Id + ")", "PUT", JSON.stringify(form), function (data) {
                    timeline.ItemTimeline();
                });
            });
            ImageIdResult = [];
        });


    }

    this.UploadUrl = function () {

        return ODataApi.Item + "UploadFile";
    }


    this.RemoveFiles = function () {

        clearFiles();

    }
}
var ImageIdResult = [];
var pill = new PillViewModel();
ko.bindingHandlers.kendoDropDownList.options.optionLabel = "choose a category...";
//ko.bindingHandlers.kendoMultiSelect.options.filter = "contains";
ko.applyBindings(pill, document.getElementById("pillbox"));
pill.Pill();



var myDropzoneInput = null;
function clearFiles() {
    var form = { Name: "", ExchangeTag: "Exchange", TradeTag: "Trade", OwnerId: 0, ModifiedDate: Helper.time(), ItemCategoryId: 0 };
    pill.PillView(form);

    $("#tagTrade").tagging();
    $("#tagExchange").tagging();

    myDropzoneInput.removeAllFiles();
}
jQuery(function ($) {

    Dropzone.options.myFormDropzone = {

        urlUploader: pill.UploadUrl(),
        urlDownloader: "",
        urlDeleter: "",
        urlFileInfo: "",
        paramName: "file",
        maxFilesize: 5,
        maxThumbnailFilesize: 100,
        clickable:true,
        showTopPanel: false,
        createResizedImage: true,
        resizedWidth: 800,
        resizedHeight: 600,
        enqueueForUpload: true, 
        dictDefaultMessage:'<br/><i class="upload-icon fa fa-cloud-upload blue fa-2x"></i>',
                 
        init: function () {
            var that = this;
            myDropzoneInput = that;
           
        },
        accept: function (file, done) {
            if (file.name == "justinbieber.jpg") {
                done("Naha, you don't.");
            }
            else {

                if (this.files.length < 2)
                    $('#fuelux-wizard').wizard('next');

                done();
            }

        },
        success: function (file, result) {
           
            $.each(result, function (key, value) {

                ImageIdResult.push(value);
            });

            return file.previewTemplate.classList.add("success");
        }
    };


});
