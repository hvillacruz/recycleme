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
        public bool RoleExists(string name)
        {
            var rm = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(new RecycleMeContext()));
            return rm.RoleExists(name);
        }


        public bool CreateRole(string name )
        {
            var rm = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(new RecycleMeContext()));
            var idResult = rm.Create(new IdentityRole(name));
            return idResult.Succeeded;
        }


        public bool CreateUser(AspNetUsers user, string password)
        {
            var um = new UserManager<AspNetUsers>(
                new UserStore<AspNetUsers>(new RecycleMeContext()));
            var idResult = um.Create(user, password);
            return idResult.Succeeded;
        }


        public bool AddUserToRole(string userId, string roleName)
        {
            var um = new UserManager<AspNetUsers>( new UserStore<AspNetUsers>(new RecycleMeContext()));
            var idResult = um.AddToRole(userId, roleName);
            return idResult.Succeeded;
        }


        public void ClearUserRoles(string userId)
        {
            var um = new UserManager<AspNetUsers>(
                new UserStore<AspNetUsers>(new RecycleMeContext()));
            var user = um.FindById(userId);
            var currentRoles = new List<IdentityUserRole>();
            currentRoles.AddRange(user.Roles);
            foreach (var role in currentRoles)
            {
                um.RemoveFromRole(userId, role.Role.Name);
            }
        }
    }
}
