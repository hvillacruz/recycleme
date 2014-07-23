alert(global.User.UserId());
var MessageViewModel = function () {

    var self = this;
    this.SelectedItem = ko.observableArray();
    this.Message = ko.observableArray();
    this.Subject = ko.observable("");
    this.Body = ko.observable("");
    this.MessageCount = ko.observable("0");
    this.Recipient = ko.observable("");
    this.RecipientList = ko.observableArray();
    this.SelectedChoice = ko.observable();
    this.SendMessage = function (item, selectedImage) {


        var data = {
            
            SenderId: global.User.UserId(),
            ReceiverId: this.SelectedChoice()[0],
            Subject: item.Subject(),
            Body: item.Body(),
            DateSent: Helper.time()

        }

        AjaxNinja.Invoke(ODataApi.Message, "POST", JSON.stringify(data), function (result) {
            self.GetMessage();
            $('#myMessageModal').modal('hide')
        });


    }

    this.GetMessage = function () {



        AjaxNinja.Invoke(ODataApi.Message + "?$filter=ReceiverId eq '" + global.User.UserId() + "'&$orderby=DateReceived desc&$expand=Sender", "GET", {}, function (data) {
            self.MessageCount("(" + data.value.length + ")");
            var result = [];
            $(data.value).each(function (i, value) {
                var date = new Date(Date.parse(value.DateSent));
                value.DateSent = date;
                if (date != null)
                    var res = $.extend(value, { Time: formatAMPM(date) });
                // $.extend(res.ItemImages, { Class: "metro" - value.ItemImages.length });

                result.push(res);

                // alert(date.getFullYear() + "/" + (date.getMonth() + 1) + "/" + (date.getUTCDate()));
            });

            self.Message(result);
            SetMsgEvents();
        });


        AjaxNinja.Invoke(ODataApi.User, "GET", {}, function (result) {
            self.RecipientList(result.value);

        });
    }


    this.ShowMessage = function (currentItem, selectedItem) {

        self.SelectedItem(selectedItem);

    }


}




var msg = new MessageViewModel();
ko.applyBindings(msg, document.getElementById("messageInbox"));
msg.GetMessage();

$('#myMessageModal').on('show.bs.modal', function (e) {
    rescale();
})

//$('#myMessageModal').modal()


function formatAMPM(date) {
    var hours = date.getHours();
    var minutes = date.getMinutes();
    var ampm = hours >= 12 ? 'pm' : 'am';
    hours = hours % 12;
    hours = hours ? hours : 12; // the hour '0' should be '12'
    minutes = minutes < 10 ? '0' + minutes : minutes;
    var strTime = hours + ':' + minutes + ' ' + ampm;
    return strTime;
}



function rescale() {
    var size = { width: $(window).width(), height: $(window).height() }
    /*CALCULATE SIZE*/
    var offset = 20;
    var offsetBody = 150;
    $('#myModal').css('height', size.height - offset);
    $('.modal-body').css('height', ((size.height / 1.5) - (offset + offsetBody)));
    $('#myModal').css('top', 0);
}


function SetMsgEvents() {
    var cols = {},

		messageIsOpen = false;

    cols.showOverlay = function () {
        $('.body').addClass('show-main-overlay');
    };
    cols.hideOverlay = function () {
        $('.body').removeClass('show-main-overlay');
    };


    cols.showMessage = function () {
        $('.body').addClass('show-message');
        messageIsOpen = true;
    };
    cols.hideMessage = function () {
        $('.body').removeClass('show-message');
        $('#main .message-lists li').removeClass('active');
        messageIsOpen = false;
    };


    cols.showSidebar = function () {
        $('.body').addClass('show-sidebar');
    };
    cols.hideSidebar = function () {
        $('.body').removeClass('show-sidebar');
    };


    // Show sidebar when trigger is clicked

    $('.trigger-toggle-sidebar').on('click', function () {

        cols.showSidebar();
        cols.showOverlay();
    });


    $('.trigger-message-close').on('click', function () {

        cols.hideMessage();
        cols.hideOverlay();
    });


    // When you click on a message, show it

    $('#main .message-lists li').on('click', function (e) {
        var item = $(this),

			target = $(e.target);

        if (target.is('label')) {
            item.toggleClass('selected');
        } else {
            if (messageIsOpen && item.is('.active')) {
                cols.hideMessage();
                cols.hideOverlay();
            } else {
                if (messageIsOpen) {
                    cols.hideMessage();
                    item.addClass('active');
                    setTimeout(function () {
                        cols.showMessage();
                    }, 300);
                } else {
                    item.addClass('active');
                    cols.showMessage();
                }
                cols.showOverlay();
            }
        }
    });


    // This will prevent click from triggering twice when clicking checkbox/label

    $('input[type=checkbox]').on('click', function (e) {
        e.stopImmediatePropagation();
    });



    // When you click the overlay, close everything

    $('#main > .overlay').on('click', function () {

        cols.hideOverlay();
        cols.hideMessage();
        cols.hideSidebar();
    });



    // Enable sexy scrollbars
    //  $('.nano').nanoScroller();



    // Disable links

    //$('a').on('click', function (e) {
    //    e.preventDefault();
    //});



    // Search box responsive stuff

    $('.search-box input').on('focus', function () {
        if ($(window).width() <= 1360) {
            cols.hideMessage();
        }
    });
}
jQuery(document).ready(function ($) {

});



