namespace RecycleMeDataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ItemCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ItemComments",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        CommenterId = c.String(maxLength: 128),
                        CommentedItemId = c.Long(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        Comment = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Items", t => t.CommentedItemId)
                .ForeignKey("dbo.Users", t => t.CommenterId)
                .Index(t => t.CommenterId)
                .Index(t => t.CommentedItemId);
            
            CreateTable(
                "dbo.Items",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        OwnerId = c.String(maxLength: 128),
                        Name = c.String(),
                        ImagePath = c.String(),
                        Description = c.String(),
                        TradeTag = c.String(),
                        ExchangeTag = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        ItemCategoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ItemCategories", t => t.ItemCategoryId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.OwnerId)
                .Index(t => t.OwnerId)
                .Index(t => t.ItemCategoryId);
            
            CreateTable(
                "dbo.ItemImages",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ItemId = c.Long(),
                        Name = c.String(),
                        Path = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Items", t => t.ItemId)
                .Index(t => t.ItemId);
            
            CreateTable(
                "dbo.ItemFollowers",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        FollowerId = c.String(maxLength: 128),
                        FollowedItemId = c.Long(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Items", t => t.FollowedItemId)
                .ForeignKey("dbo.Users", t => t.FollowerId)
                .Index(t => t.FollowerId)
                .Index(t => t.FollowedItemId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        ExternalId = c.String(),
                        ExternalUserName = c.String(),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Email = c.String(),
                        Address = c.String(),
                        BirthDate = c.DateTime(),
                        Mobile = c.String(),
                        Avatar = c.String(),
                        BgPic = c.String(),
                        ProfileStatus = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        LastActivity = c.DateTime(),
                        ModifiedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.UserComments",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        CommenterId = c.String(maxLength: 128),
                        CommentedUserId = c.String(maxLength: 128),
                        ModifiedDate = c.DateTime(nullable: false),
                        Comment = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CommentedUserId)
                .ForeignKey("dbo.Users", t => t.CommenterId)
                .Index(t => t.CommenterId)
                .Index(t => t.CommentedUserId);
            
            CreateTable(
                "dbo.UserFollowers",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        FollowerId = c.String(maxLength: 128),
                        FollowedUserId = c.String(maxLength: 128),
                        ModifiedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.FollowedUserId)
                .ForeignKey("dbo.Users", t => t.FollowerId)
                .Index(t => t.FollowerId)
                .Index(t => t.FollowedUserId);
            
            CreateTable(
                "dbo.UserFollowings",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        FollowingId = c.String(maxLength: 128),
                        FollowingUserId = c.String(maxLength: 128),
                        ModifiedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.FollowingId)
                .ForeignKey("dbo.Users", t => t.FollowingUserId)
                .Index(t => t.FollowingId)
                .Index(t => t.FollowingUserId);
            
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SenderId = c.String(maxLength: 128),
                        ReceiverId = c.String(maxLength: 128),
                        Subject = c.String(),
                        Body = c.String(),
                        DateSent = c.DateTime(),
                        DateReceived = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.ReceiverId)
                .ForeignKey("dbo.Users", t => t.SenderId)
                .Index(t => t.SenderId)
                .Index(t => t.ReceiverId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            AlterColumn("dbo.UserComments", "ModifiedDate", c => c.DateTime(defaultValueSql: "GETDATE()"));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.ItemComments", "CommenterId", "dbo.Users");
            DropForeignKey("dbo.ItemComments", "CommentedItemId", "dbo.Items");
            DropForeignKey("dbo.Items", "OwnerId", "dbo.Users");
            DropForeignKey("dbo.ItemFollowers", "FollowerId", "dbo.Users");
            DropForeignKey("dbo.Messages", "SenderId", "dbo.Users");
            DropForeignKey("dbo.Messages", "ReceiverId", "dbo.Users");
            DropForeignKey("dbo.UserFollowings", "FollowingUserId", "dbo.Users");
            DropForeignKey("dbo.UserFollowings", "FollowingId", "dbo.Users");
            DropForeignKey("dbo.UserFollowers", "FollowerId", "dbo.Users");
            DropForeignKey("dbo.UserFollowers", "FollowedUserId", "dbo.Users");
            DropForeignKey("dbo.UserComments", "CommenterId", "dbo.Users");
            DropForeignKey("dbo.UserComments", "CommentedUserId", "dbo.Users");
            DropForeignKey("dbo.ItemFollowers", "FollowedItemId", "dbo.Items");
            DropForeignKey("dbo.ItemImages", "ItemId", "dbo.Items");
            DropForeignKey("dbo.Items", "ItemCategoryId", "dbo.ItemCategories");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Messages", new[] { "ReceiverId" });
            DropIndex("dbo.Messages", new[] { "SenderId" });
            DropIndex("dbo.UserFollowings", new[] { "FollowingUserId" });
            DropIndex("dbo.UserFollowings", new[] { "FollowingId" });
            DropIndex("dbo.UserFollowers", new[] { "FollowedUserId" });
            DropIndex("dbo.UserFollowers", new[] { "FollowerId" });
            DropIndex("dbo.UserComments", new[] { "CommentedUserId" });
            DropIndex("dbo.UserComments", new[] { "CommenterId" });
            DropIndex("dbo.ItemFollowers", new[] { "FollowedItemId" });
            DropIndex("dbo.ItemFollowers", new[] { "FollowerId" });
            DropIndex("dbo.ItemImages", new[] { "ItemId" });
            DropIndex("dbo.Items", new[] { "ItemCategoryId" });
            DropIndex("dbo.Items", new[] { "OwnerId" });
            DropIndex("dbo.ItemComments", new[] { "CommentedItemId" });
            DropIndex("dbo.ItemComments", new[] { "CommenterId" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Messages");
            DropTable("dbo.UserFollowings");
            DropTable("dbo.UserFollowers");
            DropTable("dbo.UserComments");
            DropTable("dbo.Users");
            DropTable("dbo.ItemFollowers");
            DropTable("dbo.ItemImages");
            DropTable("dbo.Items");
            DropTable("dbo.ItemComments");
            DropTable("dbo.ItemCategories");
            AlterColumn("dbo.UserComments", "ModifiedDate", c => c.DateTime(defaultValueSql: "GETDATE()"));
        }
    }
}
