namespace JoelScottFitness.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedHallOfFameEnabledflag : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PurchasedItems", "HallOfFameEnabled", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PurchasedItems", "HallOfFameEnabled");
        }
    }
}
