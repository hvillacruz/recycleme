

//var uri = window.location.host.indexOf("localhost") !== 0 ? "http://recyclemeodata.azurewebsites.net/odata/" : "http://localhost:53480/odata/"
//if (window.location.host.indexOf("localhost") >= 0)
    var uri = "http://localhost:53480/odata/";
//else
    //var uri = "http://recyclemeodata.azurewebsites.net/odata/";
//alert(window.location.host.indexOf("localhost"));
var ODataApi = {
    User: uri + "User/",
    UserFollow: uri + "UserFollower/"
}