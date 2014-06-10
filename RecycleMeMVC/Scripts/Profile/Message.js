var MessageViewModel = function () {

    var self = this;
    this.SelectedItem = ko.observableArray();
    this.Message = ko.observableArray();
    this.Subject = ko.observable("");
    this.Body = ko.observable("");
    this.Recipient = ko.observable("");
    this.RecipientList = ko.observableArray();
    this.SelectedChoice = ko.observable();
    this.SendMessage = function (item, selectedImage)
    {


        var data = {

            SenderId: global.User.UserId(),
            ReceiverId: global.User.UserId(),
            Subject: item.Subject(),
            Body:item.Body(),
            DateSent: Helper.time()

        }

        AjaxNinja.Invoke(ODataApi.Message, "POST", JSON.stringify(data), function (result) {
            self.GetMessage();
            $('#myMessageModal').modal('hide')
        });

       
    }

    this.GetMessage = function () {
       


        AjaxNinja.Invoke(ODataApi.Message + "?$filter=ReceiverId eq '" + global.User.UserId() + "'&$orderby=DateReceived desc&$expand=Sender", "GET", {}, function (data) {
           
            self.Message(data.value);
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

function rescale() {
    var size = { width: $(window).width(), height: $(window).height() }
    /*CALCULATE SIZE*/
    var offset = 20;
    var offsetBody = 150;
    $('#myModal').css('height', size.height - offset);
    $('.modal-body').css('height', (size.height/2) - (offset + offsetBody));
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



