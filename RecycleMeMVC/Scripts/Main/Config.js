

var uri = window.location.host.indexOf("localhost") !== 0 ? "http://recyclemeodata.azurewebsites.net/odata/" : "http://localhost:53480/odata/"

var ODataApi = {
    User: uri + "User/",
    UserFollow: uri + "UserFollower/"
}

var Categories = {
    "List": [
        {
            "Id": "Beauty Products",
            "Name": "Beauty Products"
        },
        {
            "Id": "Photography",
            "Name": "Photography"
        },
        {
            "Id": "Games & Toys",
            "Name": "Games & Toys"
        }
    ]
}