namespace TeduShop.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addcolortodetail : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderDetails", "ColorId", c => c.Int(nullable: false));
            AddColumn("dbo.OrderDetails", "SizeId", c => c.Int(nullable: false));
            CreateIndex("dbo.OrderDetails", "ColorId");
            CreateIndex("dbo.OrderDetails", "SizeId");
            AddForeignKey("dbo.OrderDetails", "ColorId", "dbo.Colors", "ID", cascadeDelete: true);
            AddForeignKey("dbo.OrderDetails", "SizeId", "dbo.Sizes", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderDetails", "SizeId", "dbo.Sizes");
            DropForeignKey("dbo.OrderDetails", "ColorId", "dbo.Colors");
            DropIndex("dbo.OrderDetails", new[] { "SizeId" });
            DropIndex("dbo.OrderDetails", new[] { "ColorId" });
            DropColumn("dbo.OrderDetails", "SizeId");
            DropColumn("dbo.OrderDetails", "ColorId");
        }
    }
}
