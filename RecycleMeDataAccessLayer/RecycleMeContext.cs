using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecycleMeDomainClasses;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity.Validation;
namespace RecycleMeDataAccessLayer
{


    public class RecycleMeContext : IdentityDbContext<AspNetUsers>
    {
        public RecycleMeContext() : base("RecycleMeContext") { }

        public override int SaveChanges()
        {
            try
            {
                foreach (var entry in this.ChangeTracker.Entries()
                    .Where(e => e.Entity is ILogInfo &&
                        ((e.State == EntityState.Added) || (e.State == EntityState.Modified))))
                {
                    ((ILogInfo)entry.Entity).ModifiedDate = DateTime.Now;
                }


                return base.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
        }


        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<IdentityUser>().Ignore(t => t.Claims);
        //    base.OnModelCreating(modelBuilder);
        //}

        public DbSet<User> Users { get; set; }

        public DbSet<Review> Review { get; set; }

        public DbSet<Item> Items { get; set; }
       
    }
}
