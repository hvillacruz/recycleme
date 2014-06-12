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
        });


    }

    this.UploadUrl = function () {

        return ODataApi.Item + "UploadFile";
    }

}
var ImageIdResult = [];
var pill = new PillViewModel();
ko.bindingHandlers.kendoMultiSelect.options.filter = "contains";
ko.applyBindings(pill, document.getElementById("pillbox"));
pill.Pill();





jQuery(function ($) {
    var myDropzoneInput = null;
    try {
        $(".dropzone").dropzone({
            init: function () {
                var that = this;
                myDropzoneInput = this;
                this.on("addedfile", function (file) {
                    $("#pillBox").animate({ "height": "280px" }, 500);
                });
                this.on("removedfile", function (file) {
                    if (that.getAcceptedFiles().length <= 0)
                        $("#pillBox").animate({ "height": "230px" }, 500);
                });


            },
            uploadMultiple: true,
            successmultiple: function (file, result) {


                $.each(result, function (key, value) {

                    ImageIdResult.push(value);
                });
            },
            complete: function (file, result) {

            },

            paramName: "file", // The name that will be used to transfer the file
            maxFilesize: 10, // MB

            addRemoveLinks: true,
            dictDefaultMessage:
            '<br /><i class="upload-icon icon-cloud-upload blue icon-2x"></i>'
                  ,
            dictResponseError: 'Error while uploading file!',

            //change the previewTemplate to use Bootstrap progress bars
            previewTemplate: "<div class=\"dz-preview dz-file-preview\">\n  <div class=\"dz-details\">\n    <div class=\"dz-filename\"><span data-dz-name></span></div>\n    <div class=\"dz-size\" data-dz-size></div>\n    <img data-dz-thumbnail />\n  </div>\n  <div class=\"progress progress-small progress-striped active\"><div class=\"progress-bar progress-bar-success\" data-dz-uploadprogress></div></div>\n  <div class=\"dz-success-mark\"><span></span></div>\n  <div class=\"dz-error-mark\"><span></span></div>\n  <div class=\"dz-error-message\"><span data-dz-errormessage></span></div>\n</div>"
        });

    } catch (e) {
        alert('Dropzone.js does not support older browsers!');
    }

    $('#clickMe').click(function () {
        $('.dropzone').click();
    });

    $('#openDropZone').click(function () {
        $('.dropzone').click();
    });


    $('#uploadOk').click(function () {
        myDropzoneInput.removeAllFiles();
    });

    Dropzone.autoDiscover = false;
});
