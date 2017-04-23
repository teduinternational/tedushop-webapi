namespace TeduShop.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ParentIdFunction : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Functions", new[] { "ParentId" });
            AlterColumn("dbo.Functions", "ParentId", c => c.Int());
            CreateIndex("dbo.Functions", "ParentId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Functions", new[] { "ParentId" });
            AlterColumn("dbo.Functions", "ParentId", c => c.Int(nullable: false));
            CreateIndex("dbo.Functions", "ParentId");
        }
    }
}
