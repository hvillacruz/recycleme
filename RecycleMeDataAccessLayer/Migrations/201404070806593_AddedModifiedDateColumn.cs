namespace RecycleMeDataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedModifiedDateColumn : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.UserComments", "ModifiedDate", c => c.DateTime(defaultValueSql: "GETDATE()"));
        }

        public override void Down()
        {
            AlterColumn("dbo.UserComments", "ModifiedDate", c => c.DateTime(defaultValueSql: "GETDATE()"));
        }
    }
}
