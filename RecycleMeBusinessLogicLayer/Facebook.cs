using Facebook;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using RecycleMeDataAccessLayer;
using RecycleMeDomainClasses;
using System;
using System.Collections.Generic;
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
                      ExternalId = info.id,
                      ExternalUserName = info.username,
                      FirstName = info.first_name,
                      Email = info.email,
                      LastName = info.last_name,
                      BirthDate = DateTime.ParseExact(info.birthday, "MM/dd/yyyy", null),
                      Address = info.location.name,
                      Avatar = @"https://graph.facebook.com/" + info.id + "/picture?type=large"
                  };
           

            return user;
        }


    }
}
