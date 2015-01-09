using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using RecycleMeMVC.Models;
using Facebook;
using System.Configuration;
using System.Net;
using System.IO;
using RecycleMeDataAccessLayer;
using RecycleMeBusinessLogicLayer;
using RecycleMeDomainClasses;
using Microsoft.AspNet.Identity.EntityFramework;
namespace RecycleMeMVC.Controllers
{
    [RoutePrefix("Profile")]
    [Authorize]
    public class ProfileController : Controller
    {

        //
        // GET: /Profile/
        [Route("Dashboard/{id?}")]
        public ActionResult Dashboard(string id)
        {

            ViewBag.UserId = id == null ? User.Identity.GetUserId() : id;
            ViewBag.IsUser = User.Identity.GetUserId() == id ? 1 : 0;
            ViewBag.LoginUserId = User.Identity.GetUserId();

            return View();
        }

        [Route("Dashboard")]
        public ActionResult Dashboard()
        {

            ViewBag.UserId = User.Identity.GetUserId();
            ViewBag.LoginUserId = User.Identity.GetUserId();
            return View();
        }



        [Route("Notification")]
        public ActionResult Notification()
        {

            ViewBag.UserId = User.Identity.GetUserId();
            ViewBag.LoginUserId = User.Identity.GetUserId();
            return View();
        }




        [Route("Bid/{id?}")]
        public ActionResult Bid(string id)
        {

            ViewBag.UserId = id == null ? User.Identity.GetUserId() : id;
            ViewBag.IsUser = User.Identity.GetUserId() == id ? 1 : 0;
            ViewBag.LoginUserId = User.Identity.GetUserId();
            ViewBag.ItemId = id;
            return View();
        }

        [Route("Bid")]
        public ActionResult Bid()
        {

            ViewBag.UserId = User.Identity.GetUserId();
            return View();
        }


        [Route("Message")]
        public ActionResult Message()
        {

            ViewBag.UserId = User.Identity.GetUserId();
            return View();
        }


        [Route("Message/{id?}")]
        public ActionResult Message(string id)
        {

            ViewBag.UserId = User.Identity.GetUserId();
            return View();
        }


        [Route("Following/{id?}")]
        public ActionResult Following(string id)
        {

            ViewBag.UserId = id == null ? User.Identity.GetUserId() : id;
            return View();
        }


        [Route("Trade")]
        public ActionResult Trade()
        {
            
            return View();
        }


        [Route("Trade/{id?}")]
        public ActionResult Trade(string id)
        {
            ViewBag.ItemId = id == null ? String.Empty : id;
            return View();
        }


        [Route("Exchange")]
        public ActionResult Exchange()
        {

            return View();
        }

        [Route("Exchange/{id?}")]
        public ActionResult Exchange(string id)
        {
            ViewBag.ItemId = id == null ? String.Empty : id;
            return View();
        }


      
        [Route("Followers/{id?}")]
        public ActionResult Followers(string id)
        {

            ViewBag.UserId = id == null ? User.Identity.GetUserId() : id;
            return View();
        }


        private static long ItemId { get; set; }
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

        public ActionResult Twitter(string id)
        {


            var usermanager = new UserManager<AspNetUsers>(new UserStore<AspNetUsers>(new RecycleMeContext()));
            var claimsforUser = usermanager.GetClaims(User.Identity.GetUserId());
            var access_token = claimsforUser.FirstOrDefault(x => x.Type == "urn:twitter:access_token").Value;
            var access_token_secret = claimsforUser.FirstOrDefault(x => x.Type == "urn:twitter:access_token_secret").Value;

            Twitter twitter = new Twitter(ConfigurationManager.AppSettings["TweetConsumerKey"],
                                            ConfigurationManager.AppSettings["TweetConsumerSecret"], access_token, access_token_secret);

            twitter.PostStatusUpdate("hello", 1, 2);
            return null;
        }

        public ActionResult Facebook(string id)
        {
            ItemId = long.Parse(id);
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


        public System.Drawing.Image DownloadImageFromUrl(string imageUrl)
        {
            System.Drawing.Image image = null;

            try
            {
                System.Net.HttpWebRequest webRequest = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(imageUrl);
                webRequest.AllowWriteStreamBuffering = true;
                webRequest.Timeout = 30000;

                System.Net.WebResponse webResponse = webRequest.GetResponse();

                System.IO.Stream stream = webResponse.GetResponseStream();

                image = System.Drawing.Image.FromStream(stream);

                webResponse.Close();
            }
            catch (Exception ex)
            {
                return null;
            }

            return image;
        }

        public static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        public ActionResult FacebookCallback(string code)
        {
            var fb = new FacebookClient();
            dynamic result = fb.Post("oauth/access_token", new
            {
                client_id = ConfigurationManager.AppSettings["FbAppId"],
                client_secret = ConfigurationManager.AppSettings["FbAppSecret"],
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
            //dynamic res = fb.Post("me/feed", new
            //{
            //    message = "My first wall post using Facebook SDK for .NETs"
            //});
            //var newPostId = res.id;

            RecycleMeContext db = new RecycleMeContext();
            var itemImage = db.Items.Where(a => a.Id == ItemId).FirstOrDefault();
            var image = db.ItemImage.Where(a => a.ItemId == ItemId).FirstOrDefault();


            WebRequest req = WebRequest.Create(image.Path);
            WebResponse response = req.GetResponse();
            Stream stream = response.GetResponseStream();




            var media = new FacebookMediaObject
            {
                FileName = "henry",
                ContentType = "image/jpeg"
            };
            byte[] img = ReadFully(stream);
            media.SetValue(img);
            var postparameters = new Dictionary<string, object>();

            postparameters["source"] = media;
            postparameters["message"] = "Think Recycle Me - " + itemImage.Name;
            postparameters["link"] = "http://recycleme.azurewebsites.net/";
            var res = fb.Post("/me/photos", postparameters);
            // Set the auth cookie
            //FormsAuthentication.SetAuthCookie(email, false);

            return RedirectToAction("Dashboard", "Profile");
        }
    }


}