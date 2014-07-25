using Facebook;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using RecycleMeDataAccessLayer;
using RecycleMeDomainClasses;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecycleMeBusinessLogicLayer
{
    public class FB
    {

        public static UserViewModel UserInfo(string UserId)
        {

            var usermanager = new UserManager<AspNetUsers>(new UserStore<AspNetUsers>(new RecycleMeContext()));
            var claimsforUser = usermanager.GetClaims(UserId);
            var access_token = claimsforUser.FirstOrDefault(x => x.Type == "FacebookAccessToken").Value;
            var fb = new FacebookClient(access_token);

            dynamic info = fb.Get("me");

            UserViewModel user = new UserViewModel();

            user = new UserViewModel()
              {
                  UserId = UserId,
                  ExternalId = GetValidValue(info.id),
                  ExternalUserName = GetValidValue(info.username),
                  FirstName = GetValidValue(info.first_name),
                  Email = GetValidValue(info.email),
                  LastName = GetValidValue(info.last_name),
                  BirthDate = DateTime.ParseExact(info.birthday, "MM/dd/yyyy", null),
                  Address = info.location == null ? "" : info.location.name,
                  Avatar = @"https://graph.facebook.com/" + GetValidValue(info.id) + "/picture?type=large"
              };


            return user;
        }

        private static string GetValidValue(string value)
        {
            return value == null ? "" : value;
        }


        public static string FbAccess()
        {

            var facebookClient = new FacebookClient();
            facebookClient.AppId = ConfigurationManager.AppSettings["FbAppId"];
            facebookClient.AppSecret = ConfigurationManager.AppSettings["FbAppSecret"];

            try
            {
                dynamic result = facebookClient.Get("oauth/access_token", new
                {
                    client_id = ConfigurationManager.AppSettings["FbAppId"],
                    client_secret = ConfigurationManager.AppSettings["FbAppSecret"],
                    grant_type = "authorization_code"
                });
                return result.access_token;
            }
            catch (Exception ex)
            {

                return ex.Message;
            }


        }


        public void Post(string UserId)
        {

            var usermanager = new UserManager<AspNetUsers>(new UserStore<AspNetUsers>(new RecycleMeContext()));
            var claimsforUser = usermanager.GetClaims(UserId);
            var access_token = claimsforUser.FirstOrDefault(x => x.Type == "FacebookAccessToken").Value;
            var client = new FacebookClient(access_token);

            ////dynamic me = client.Get("me");
            ////var id = me.id;

            ////dynamic parameters = new ExpandoObject();
            ////parameters.title = "test title";
            ////parameters.message = "test message";
            ////try
            ////{
            ////    var result = client.Post(id + "/feed", parameters);
            ////}
            //catch (Exception ex)
            //{
            //    Console.Write(ex.Message);
            //}

            dynamic result = client.Post("me/feed", new
            {
                message = "My first wall post using Facebook SDK for .NET"
            });
            var newPostId = result.id;

        }

    }
}
