using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using RecycleMeDataAccessLayer;
using RecycleMeDomainClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RecycleMeBusinessLogicLayer
{
    public class AspNetUser
    {
        public static IList<Claim> GetClaim(string UserId)
        {
            var usermanager = new UserManager<AspNetUsers>(new UserStore<AspNetUsers>(new RecycleMeContext()));
            return usermanager.GetClaims(UserId);
        }

        public static UserManager<AspNetUsers> GetUserManager()
        {
            return new UserManager<AspNetUsers>(new UserStore<AspNetUsers>(new RecycleMeContext()));
        }

    }
}
