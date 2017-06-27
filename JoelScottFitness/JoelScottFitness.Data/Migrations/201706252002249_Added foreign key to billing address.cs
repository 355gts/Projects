namespace JoelScottFitness.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addedforeignkeytobillingaddress : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Customers", name: "BillingAddress_Id", newName: "BillingAddressId");
            RenameIndex(table: "dbo.Customers", name: "IX_BillingAddress_Id", newName: "IX_BillingAddressId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Customers", name: "IX_BillingAddressId", newName: "IX_BillingAddress_Id");
            RenameColumn(table: "dbo.Customers", name: "BillingAddressId", newName: "BillingAddress_Id");
        }
    }
}
