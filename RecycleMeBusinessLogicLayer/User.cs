using RecycleMeDataAccessLayer;
using RecycleMeDomainClasses;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecycleMeBusinessLogicLayer
{
    public class Users
    {
        public static void Create(UserViewModel user)
        {

            using (RecycleMeContext context = new RecycleMeContext())
            {
                context.Users.Add(new User()
                {
                    ExternalId = user.ExternalId,
                    ExternalUserName = user.ExternalUserName,
                    UserId = user.UserId,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Address = user.Address,
                    Avatar = user.Avatar,
                    BirthDate = user.BirthDate,
                    Email = user.Email,
                    IsActive = true,
                    LastActivity = DateTime.Now
                });

                context.SaveChanges();

            }
        }


        public static User Get(string id)
        {
            using (RecycleMeContext context = new RecycleMeContext())
            {
                return context.Users.Where(a => a.UserId == id).FirstOrDefault();

            }
        }
    }
}
