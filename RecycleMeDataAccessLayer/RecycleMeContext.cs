﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecycleMeDomainClasses;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity.Validation;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;
namespace RecycleMeDataAccessLayer
{

    public class RecycleMeContext : IdentityDbContext<AspNetUsers>
    {
        public RecycleMeContext() : base("RecycleMeContext", throwIfV1Schema: false) { }

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


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Entity<UserComment>()
           .HasOptional(b => b.Commenter)
           .WithMany(a => a.UserCommenter)
           .HasForeignKey(k => k.CommenterId)
           .WillCascadeOnDelete(false);

            modelBuilder.Entity<UserComment>()
            .HasOptional(b => b.CommentedUser)
            .WithMany(a => a.UserCommented)
            .HasForeignKey(k => k.CommentedUserId)
            .WillCascadeOnDelete(false);

            modelBuilder.Entity<UserFollower>()
            .HasOptional(b => b.Follower)
            .WithMany(a => a.UserFollowers)
            .HasForeignKey(k => k.FollowerId)
            .WillCascadeOnDelete(false);

            modelBuilder.Entity<UserFollower>()
            .HasOptional(b => b.FollowedUser)
            .WithMany(a => a.UserFollowerUsers)
            .HasForeignKey(k => k.FollowedUserId)
            .WillCascadeOnDelete(false);
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserFollowing>()
            .HasOptional(b => b.Following)
            .WithMany(a => a.UserFollowing)
            .HasForeignKey(k => k.FollowingId)
            .WillCascadeOnDelete(false);

            modelBuilder.Entity<UserFollowing>()
           .HasOptional(b => b.FollowingUser)
           .WithMany(a => a.UserFollowingUsers)
           .HasForeignKey(k => k.FollowingUserId)
           .WillCascadeOnDelete(false);


            modelBuilder.Entity<Item>()
            .HasOptional(b => b.Owner)
            .WithMany(a => a.Items)
            .HasForeignKey(k => k.OwnerId)
            .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Item>()
            //.HasRequired(t => t.Category)
            //.WithMany()
            //.Map(c => c.MapKey("ItemCategoryId"));

            modelBuilder.Entity<ItemComment>()
            .HasOptional(b => b.Commenter)
            .WithMany(a => a.UserItemCommenter)
            .HasForeignKey(k => k.CommenterId)
            .WillCascadeOnDelete(false);

            modelBuilder.Entity<ItemComment>()
           .HasRequired(b => b.CommentedItem)
           .WithMany(a => a.ItemCommented)
           .HasForeignKey(k => k.CommentedItemId)
           .WillCascadeOnDelete(false);


            modelBuilder.Entity<ItemFollowers>()
             .HasOptional(b => b.Follower)
             .WithMany(a => a.UserItemFollowers)
             .HasForeignKey(k => k.FollowerId)
             .WillCascadeOnDelete(false);


            modelBuilder.Entity<ItemFollowers>()
           .HasRequired(b => b.FollowedItem)
           .WithMany(a => a.ItemUserFollowers)
           .HasForeignKey(k => k.FollowedItemId)
           .WillCascadeOnDelete(false);
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<ItemImage>()
           .HasOptional(b => b.Item)
           .WithMany(a => a.ItemImages)
           .HasForeignKey(k => k.ItemId)
           .WillCascadeOnDelete(false);

            modelBuilder.Entity<Message>()
            .HasOptional(b => b.Sender)
            .WithMany(a => a.UserSender)
            .HasForeignKey(k => k.SenderId)
            .WillCascadeOnDelete(false);


            modelBuilder.Entity<Message>()
            .HasOptional(b => b.Receiver)
            .WithMany(a => a.UserReceiver)
            .HasForeignKey(k => k.ReceiverId)
            .WillCascadeOnDelete(false);


            modelBuilder.Entity<Message>()
           .HasOptional(b => b.Parent)
           .WithMany(a => a.SubMessage)
           .HasForeignKey(k => k.ParentId)
           .WillCascadeOnDelete(false);



            modelBuilder.Entity<Trade>()
           .HasOptional(b => b.Buyer)
           .WithMany(a => a.UserBuyer)
           .HasForeignKey(k => k.BuyerId)
           .WillCascadeOnDelete(false);



            modelBuilder.Entity<Trade>()
            .HasOptional(b => b.Seller)
            .WithMany(a => a.UserSeller)
            .HasForeignKey(k => k.SellerId)
            .WillCascadeOnDelete(false);



            modelBuilder.Entity<Trade>()
            .HasOptional(b => b.Item)
            .WithMany(a => a.ItemTrades)
            .HasForeignKey(k => k.ItemId)
            .WillCascadeOnDelete(false);


            modelBuilder.Entity<TradeBuyerItem>()
            .HasOptional(b => b.Trade)
            .WithMany(a => a.Trades)
            .HasForeignKey(k => k.TradeId)
            .WillCascadeOnDelete(false);


            modelBuilder.Entity<TradeComment>()
            .HasOptional(b => b.TradeCommenter)
            .WithMany(a => a.UserTrade)
            .HasForeignKey(k => k.TradeCommenterId)
            .WillCascadeOnDelete(false);

            modelBuilder.Entity<TradeComment>()
           .HasOptional(b => b.TradeItemCommented)
           .WithMany(a => a.TradeItem)
           .HasForeignKey(k => k.TradeId)
           .WillCascadeOnDelete(false);


            modelBuilder.Entity<Notification>()
            .HasOptional(b => b.Owner)
            .WithMany(a => a.Notifications)
            .HasForeignKey(k => k.OwnerId)
            .WillCascadeOnDelete(false);

            modelBuilder.Entity<Notification>()
            .HasOptional(b => b.Sender)
            .WithMany(a => a.NotificationsSender)
            .HasForeignKey(k => k.SenderId)
            .WillCascadeOnDelete(false);

        }

        public DbSet<User> Users { get; set; }

        public DbSet<Item> Items { get; set; }

        public DbSet<ItemImage> ItemImage { get; set; }

        public DbSet<ItemCategory> ItemCategory { get; set; }

        public DbSet<ItemComment> ItemComment { get; set; }

        public DbSet<ItemFollowers> ItemFollowers { get; set; }

        public DbSet<UserFollower> UserFollower { get; set; }

        public DbSet<UserFollowing> UserFollowing { get; set; }

        public DbSet<UserComment> UserComment { get; set; }

        public DbSet<Message> Message { get; set; }

        public DbSet<Trade> Trade { get; set; }

        public DbSet<TradeBuyerItem> TradeBuyerItem { get; set; }

        public DbSet<TradeComment> TradeComment { get; set; }

        public DbSet<Notification> Notification { get; set; }

    }

    public class StorageContext
    {
        private CloudStorageAccount _storageAccount;

        public StorageContext()
        {
            _storageAccount = CloudStorageAccount.Parse(System.Configuration.ConfigurationManager.ConnectionStrings["Azure"].ConnectionString);
        }

        public CloudBlobClient BlobClient
        {
            get { return _storageAccount.CreateCloudBlobClient(); }
        }

        public CloudTableClient TableClient
        {
            get { return _storageAccount.CreateCloudTableClient(); }
        }
    }
}
