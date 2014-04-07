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
    public class IdentityManager
    {
        public bool RoleExists(string Name)
        {
            var rm = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(new RecycleMeContext()));
            return rm.RoleExists(Name);
        }


        public bool CreateRole(string Name)
        {
            var rm = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(new RecycleMeContext()));
            var idResult = rm.Create(new IdentityRole(Name));
            return idResult.Succeeded;
        }


        public bool CreateUser(AspNetUsers User, string Password)
        {
            var um = new UserManager<AspNetUsers>(
                new UserStore<AspNetUsers>(new RecycleMeContext()));
            var idResult = um.Create(User, Password);
            return idResult.Succeeded;
        }


        public bool AddUserToRole(string UserId, string RoleName)
        {
            var um = new UserManager<AspNetUsers>(new UserStore<AspNetUsers>(new RecycleMeContext()));
            var idResult = um.AddToRole(UserId, RoleName);
            return idResult.Succeeded;
        }


        public void ClearUserRoles(string UserId)
        {
            var um = new UserManager<AspNetUsers>(
                new UserStore<AspNetUsers>(new RecycleMeContext()));
            var user = um.FindById(UserId);
            var currentRoles = new List<IdentityUserRole>();
            currentRoles.AddRange(user.Roles);
            foreach (var role in currentRoles)
            {
                um.RemoveFromRole(UserId,role.RoleId);
            }
        }
    }
}
