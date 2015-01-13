InvokeHub(getCookie("RecycleAccessToken"));
var global = new GlobalViewModel();
global.User("@User.Identity.GetUserId()", "@User.Identity.GetUserName()")
ko.applyBindings(global, document.getElementById("loginPartialDiv"));
     global.GetMessage();
global.GetNotifications();
global.Profile();