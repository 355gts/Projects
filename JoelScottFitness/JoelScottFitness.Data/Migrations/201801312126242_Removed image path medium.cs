namespace JoelScottFitness.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Removedimagepathmedium : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Plans", "ImagePathMedium");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Plans", "ImagePathMedium", c => c.String(nullable: false));
        }
    }
}
