namespace JoelScottFitness.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifiedDateonPlannullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Plans", "ModifiedDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Plans", "ModifiedDate", c => c.DateTime(nullable: false));
        }
    }
}
