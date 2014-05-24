using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using RecycleMeMVC.Models;
using Facebook;
using System.Configuration;
namespace RecycleMeMVC.Controllers
{
    [RoutePrefix("Profile")]
    public class ProfileController : Controller
    {
        //
        // GET: /Profile/
        [Route("Dashboard/{id?}")]
        public ActionResult Dashboard(string id)
        {

            ViewBag.UserId = id == null ? User.Identity.GetUserId() : id;
            return View();
        }


        private Uri RedirectUri
        {
            get
            {
                var uriBuilder = new UriBuilder(Request.Url);
                uriBuilder.Query = null;
                uriBuilder.Fragment = null;
                uriBuilder.Path = Url.Action("FacebookCallback");
                return uriBuilder.Uri;
            }
        }

        public ActionResult Facebook(string id)
        {
            var fb = new FacebookClient();
            var loginUrl = fb.GetLoginUrl(new
            {
                client_id = ConfigurationManager.AppSettings["FbAppId"],
                client_secret = ConfigurationManager.AppSettings["FbAppSecret"],
                redirect_uri = RedirectUri.AbsoluteUri,
                 grant_type = "authorization_code",
                response_type = "code",
                scope = "email" // Add other permissions as needed
            });

            return Redirect(loginUrl.AbsoluteUri);
        }



        public ActionResult FacebookCallback(string code)
        {
            var fb = new FacebookClient();
            dynamic result = fb.Post("oauth/access_token", new
            {
                client_id =  ConfigurationManager.AppSettings["FbAppId"],
                client_secret =  ConfigurationManager.AppSettings["FbAppSecret"],
                redirect_uri = RedirectUri.AbsoluteUri,
                grant_type = "authorization_code",
                code = code
            });




            var accessToken = result.access_token;

            // Store the access token in the session
            Session["AccessToken"] = accessToken;

            // update the facebook client with the access token so 
            // we can make requests on behalf of the user
            fb.AccessToken = accessToken;

            // Get the user's information
            dynamic me = fb.Get("me?fields=first_name,last_name,id,email");
            string email = me.email;
            dynamic res = fb.Post("me/feed", new
            {
                message = "My first wall post using Facebook SDK for .NETs"
            });
            var newPostId = res.id;

            // Set the auth cookie
            //FormsAuthentication.SetAuthCookie(email, false);

            return RedirectToAction("Dashboard", "Profile");
        }
    }
}