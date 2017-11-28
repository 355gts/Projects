namespace JoelScottFitness.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addedhalloffamefields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PurchasedItems", "MemberOfHallOfFame", c => c.Boolean(nullable: false));
            AddColumn("dbo.PurchasedItems", "BeforeImage", c => c.String());
            AddColumn("dbo.PurchasedItems", "AfterImage", c => c.String());
            AddColumn("dbo.PurchasedItems", "Comment", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PurchasedItems", "Comment");
            DropColumn("dbo.PurchasedItems", "AfterImage");
            DropColumn("dbo.PurchasedItems", "BeforeImage");
            DropColumn("dbo.PurchasedItems", "MemberOfHallOfFame");
        }
    }
}
