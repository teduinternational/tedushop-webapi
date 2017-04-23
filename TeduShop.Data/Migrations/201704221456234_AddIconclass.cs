namespace TeduShop.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIconclass : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Functions", "IconCss", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Functions", "IconCss");
        }
    }
}
