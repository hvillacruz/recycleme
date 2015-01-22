

$(document).ready(function () {
    $("#tagTrade").tagging();
    $("#tagExchange").tagging();

    var $validation = false;
    var wiz = $('#fuelux-wizard').ace_wizard().on('change', function (e, info) {
        if (info.direction == "next") {
            if (info.step == 1) {
                $('.dropzone').click();

            }
            else if (info.step == 2) {
                $('#myModal').modal('show');
                //    if (!$('#validation-form').valid()) return false;
            }
            else if (info.step == 3) {
                pill.Recycle();

            }
        }
        else {
            if (info.step == 2) {
                $('.dropzone').click();

            }
            else if (info.step == 3) {
                $('#myModal').modal('show');

            }


        }

    }).on('finished', function (e) {

        pill.Recycle();
        pill.RemoveFiles();

        $('[data-target=#step1]').trigger("click");

        bootbox.dialog({
            message: "Trade Item is Uploaded",
            buttons: {
                "success": {
                    "label": "OK",
                    "className": "btn-sm btn-primary"
                }
            }
        });
        // optionally navigate back to 2nd step
        //$('#btnStep2').on('click', function (e, data) {
        //   $('[data-target=#step1]').trigger("click");
        //});
        //$('#fuelux-wizard').wizard('previous');
        //$('[data-target=#step1]').trigger("click");

    }).on('stepclick', function (e) {
        //return false;


    });

});
