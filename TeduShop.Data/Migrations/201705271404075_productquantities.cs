namespace TeduShop.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class productquantities : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ProductColors", "ColorId", "dbo.Colors");
            DropForeignKey("dbo.ProductColors", "ProductId", "dbo.Products");
            DropForeignKey("dbo.ProductSizes", "ProductId", "dbo.Products");
            DropForeignKey("dbo.ProductSizes", "SizeId", "dbo.Sizes");
            DropIndex("dbo.ProductColors", new[] { "ProductId" });
            DropIndex("dbo.ProductColors", new[] { "ColorId" });
            DropIndex("dbo.ProductSizes", new[] { "ProductId" });
            DropIndex("dbo.ProductSizes", new[] { "SizeId" });
            CreateTable(
                "dbo.ProductQuantities",
                c => new
                    {
                        ProductId = c.Int(nullable: false),
                        SizeId = c.Int(nullable: false),
                        ColorId = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ProductId, t.SizeId, t.ColorId })
                .ForeignKey("dbo.Colors", t => t.ColorId, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .ForeignKey("dbo.Sizes", t => t.SizeId, cascadeDelete: true)
                .Index(t => t.ProductId)
                .Index(t => t.SizeId)
                .Index(t => t.ColorId);
            
            DropTable("dbo.ProductColors");
            DropTable("dbo.ProductSizes");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ProductSizes",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ProductId = c.Int(nullable: false),
                        SizeId = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ProductColors",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ProductId = c.Int(nullable: false),
                        ColorId = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            DropForeignKey("dbo.ProductQuantities", "SizeId", "dbo.Sizes");
            DropForeignKey("dbo.ProductQuantities", "ProductId", "dbo.Products");
            DropForeignKey("dbo.ProductQuantities", "ColorId", "dbo.Colors");
            DropIndex("dbo.ProductQuantities", new[] { "ColorId" });
            DropIndex("dbo.ProductQuantities", new[] { "SizeId" });
            DropIndex("dbo.ProductQuantities", new[] { "ProductId" });
            DropTable("dbo.ProductQuantities");
            CreateIndex("dbo.ProductSizes", "SizeId");
            CreateIndex("dbo.ProductSizes", "ProductId");
            CreateIndex("dbo.ProductColors", "ColorId");
            CreateIndex("dbo.ProductColors", "ProductId");
            AddForeignKey("dbo.ProductSizes", "SizeId", "dbo.Sizes", "ID", cascadeDelete: true);
            AddForeignKey("dbo.ProductSizes", "ProductId", "dbo.Products", "ID", cascadeDelete: true);
            AddForeignKey("dbo.ProductColors", "ProductId", "dbo.Products", "ID", cascadeDelete: true);
            AddForeignKey("dbo.ProductColors", "ColorId", "dbo.Colors", "ID", cascadeDelete: true);
        }
    }
}
