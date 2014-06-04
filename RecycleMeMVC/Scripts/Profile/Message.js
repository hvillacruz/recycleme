var MessageViewModel = function () {

    var self = this;
    this.SelectedItem = ko.observableArray();
    this.Message = ko.observableArray();
    this.PostMessage = function () {

        var data = {

            SenderId: $("#currentUser").data("text"),
            ReceiverId: item.Receiver,
            Heading: item.Heading,
            Body:item.Body,
            DateSent: Helper.time()

        }

        AjaxNinja.Invoke(ODataApi.Message, "POST", JSON.stringify(data), function (result) {
            alert('success');
        });
    }

    this.GetMessage = function () {
       
        AjaxNinja.Invoke(ODataApi.Message + "?$filter=ReceiverId eq '" + global.User.UserId() + "'&$orderby=DateReceived desc&$expand=Sender", "GET", {}, function (data) {
           
            self.Message(data.value);
            SetMsgEvents();
        });

    }


    this.ShowMessage = function (currentItem, selectedItem) {
       
        self.SelectedItem(selectedItem);

    }
}
var msg = new MessageViewModel();
ko.applyBindings(msg, document.getElementById("messageInbox"));
msg.GetMessage();



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
    $('.nano').nanoScroller();



    // Disable links

    $('a').on('click', function (e) {
        e.preventDefault();
    });



    // Search box responsive stuff

    $('.search-box input').on('focus', function () {
        if ($(window).width() <= 1360) {
            cols.hideMessage();
        }
    });
}
jQuery(document).ready(function ($) {
});



