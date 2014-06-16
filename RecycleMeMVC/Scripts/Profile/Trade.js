var TradeViewModel = function () {
    var self = this;
    this.Items = ko.observableArray();

    this.CurrentItems = function () {

        AjaxNinja.Invoke(ODataApi.User + "('" + global.User.UserId() + "')/Items?$orderby=ModifiedDate desc&$expand=Owner,ItemImages,Category,ItemCommented,ItemUserFollowers", "GET", {}, function (data) {

            var result = [];
            $(data.value).each(function (index, value) {
                var res = $.extend(value, { CommentText: "", ImageClass: "metro-" + value.ItemImages.length });

                result.push(res);
            });
            self.Items(result);

            new Sortable(multi, {
                draggable: '.tile',
                handle: '.tile__name'
            });


            [].forEach.call(multi.getElementsByClassName('tile__list'), function (el) {
                new Sortable(el, { group: 'photo' });
            });

        });

    }



    this.SelectedUser = function (item) {

        window.location.href = '/Profile/Dashboard/' + item.FollowedUserId;

    }

}

var items = new TradeViewModel();
ko.applyBindings(items, document.getElementById("tradeItemContainer"));
items.CurrentItems();
