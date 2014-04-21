var WallViewModel = function () {

    var self = this;
    this.Info = ko.observableArray();

    this.Wall = function () {

        //AjaxNinja.Invoke(ODataApi.User + "('" + $("#currentUser").data("text") + "')" + "?$expand=UserFollowerUsers,UserCommenter,UserFollowing", "GET", {}, function (data) {
        //    self.Info(data);
        //});
        var data = { title: "Hello Wall" };
        self.Info(data);
    }






}
var wall = new WallViewModel();
ko.applyBindings(wall, document.getElementById("wallDiv"));
wall.Wall();
var ImageIdResult = [];


jQuery(function ($) {

    try {
        $(".dropzone").dropzone({
            init: function () {
                var that = this;
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
            maxFilesize: 0.5, // MB

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

});
