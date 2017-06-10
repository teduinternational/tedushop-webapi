namespace TeduShop.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addreferencetouser : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Announcements", "UserId");
            RenameColumn(table: "dbo.Announcements", name: "AppUser_Id", newName: "UserId");
            RenameIndex(table: "dbo.Announcements", name: "IX_AppUser_Id", newName: "IX_UserId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Announcements", name: "IX_UserId", newName: "IX_AppUser_Id");
            RenameColumn(table: "dbo.Announcements", name: "UserId", newName: "AppUser_Id");
            AddColumn("dbo.Announcements", "UserId", c => c.String(maxLength: 128));
        }
    }
}
