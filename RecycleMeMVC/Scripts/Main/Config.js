
var uri = window.location.host.indexOf("localhost") !== 0 ? "http://recyclemeodata.azurewebsites.net/odata/" : "http://localhost:53480/odata/"

var ODataApi = {
    User: uri + "User/",
    UserFollow: uri + "UserFollower/",
    ItemCategory: uri + "ItemCategory/",
    Item: uri + "Item/",
    ItemImage: uri + "ItemImage/",
    ItemComment: uri + "ItemComment"

}
