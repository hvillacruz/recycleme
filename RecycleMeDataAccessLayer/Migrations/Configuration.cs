namespace RecycleMeDataAccessLayer.Migrations
{

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using RecycleMeDomainClasses;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<RecycleMeDataAccessLayer.RecycleMeContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }



        protected override void Seed(RecycleMeDataAccessLayer.RecycleMeContext Context)
        {
            this.AddUserAndRoles();
            this.AddCategories(Context);
        }

        void AddCategories(RecycleMeContext context)
        {
            context.ItemCategory.AddOrUpdate(
            new ItemCategory() { Name = "For Him", },
            new ItemCategory() { Name = "For Her", }
        );
        }

        bool AddUserAndRoles()
        {
            bool success = false;

            var idManager = new IdentityManager();
            success = idManager.CreateRole("Admin");
            if (!success == true) return success;

            success = idManager.CreateRole("User");
            if (!success) return success;


            var newUser = new AspNetUsers()
            {
                UserName = "Admin",
            };


            success = idManager.CreateUser(newUser, "Password@123");
            if (!success) return success;

            success = idManager.AddUserToRole(newUser.Id, "Admin");
            if (!success) return success;

            return success;
        }


    }

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


        public bool CreateUser(AspNetUsers user, string password)
        {
            var um = new UserManager<AspNetUsers>(
                new UserStore<AspNetUsers>(new RecycleMeContext()));
            var idResult = um.Create(user, password);
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
                um.RemoveFromRole(UserId, role.RoleId);
            }
        }
    }
}
