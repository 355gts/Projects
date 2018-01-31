namespace JoelScottFitness.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Addresses",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        AddressLine1 = c.String(nullable: false),
                        AddressLine2 = c.String(),
                        AddressLine3 = c.String(),
                        City = c.String(nullable: false),
                        Region = c.String(nullable: false),
                        PostCode = c.String(nullable: false),
                        Country = c.String(nullable: false),
                        CountryCode = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.BlogImages",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        BlogId = c.Long(nullable: false),
                        ImagePath = c.String(nullable: false),
                        CaptionTitle = c.String(),
                        Caption = c.String(),
                        CaptionColour = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Blogs", t => t.BlogId, cascadeDelete: true)
                .Index(t => t.BlogId);
            
            CreateTable(
                "dbo.Blogs",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        SubHeader = c.String(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                        Content = c.String(nullable: false),
                        ImagePath = c.String(nullable: false),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AuthClaim",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Long(nullable: false),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AuthUser", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Firstname = c.String(nullable: false),
                        Surname = c.String(nullable: false),
                        EmailAddress = c.String(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                        BillingAddressId = c.Long(nullable: false),
                        UserId = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Addresses", t => t.BillingAddressId, cascadeDelete: true)
                .ForeignKey("dbo.AuthUser", t => t.UserId)
                .Index(t => t.BillingAddressId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Purchases",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        PurchaseDate = c.DateTime(nullable: false),
                        TotalAmount = c.Double(nullable: false),
                        PayPalReference = c.String(nullable: false),
                        TransactionId = c.String(nullable: false),
                        DiscountCodeId = c.Long(),
                        CustomerId = c.Guid(nullable: false),
                        Status = c.Int(nullable: false),
                        QuestionnareId = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.CustomerId, cascadeDelete: true)
                .ForeignKey("dbo.DiscountCodes", t => t.DiscountCodeId)
                .ForeignKey("dbo.Questionnaires", t => t.QuestionnareId)
                .Index(t => t.DiscountCodeId)
                .Index(t => t.CustomerId)
                .Index(t => t.QuestionnareId);
            
            CreateTable(
                "dbo.DiscountCodes",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Code = c.String(nullable: false),
                        PercentDiscount = c.Int(nullable: false),
                        ValidFrom = c.DateTime(nullable: false),
                        ValidTo = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PurchasedItems",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ItemId = c.Long(nullable: false),
                        Quantity = c.Long(nullable: false),
                        PurchaseId = c.Long(nullable: false),
                        PlanPath = c.String(),
                        MemberOfHallOfFame = c.Boolean(nullable: false),
                        BeforeImage = c.String(),
                        AfterImage = c.String(),
                        Comment = c.String(),
                        HallOfFameDate = c.DateTime(),
                        HallOfFameEnabled = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Items", t => t.ItemId, cascadeDelete: true)
                .ForeignKey("dbo.Purchases", t => t.PurchaseId, cascadeDelete: true)
                .Index(t => t.ItemId)
                .Index(t => t.PurchaseId);
            
            CreateTable(
                "dbo.Items",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Description = c.String(nullable: false),
                        Price = c.Double(nullable: false),
                        ItemType = c.Int(nullable: false),
                        Duration = c.Long(),
                        PlanId = c.Long(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Plans", t => t.PlanId, cascadeDelete: true)
                .Index(t => t.PlanId);
            
            CreateTable(
                "dbo.Plans",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Description = c.String(nullable: false),
                        BannerHeader = c.String(nullable: false),
                        BannerColour = c.Int(nullable: false),
                        ImagePathMedium = c.String(nullable: false),
                        ImagePathLarge = c.String(nullable: false),
                        TargetGender = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Questionnaires",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Age = c.Int(nullable: false),
                        Weight = c.Int(nullable: false),
                        Height = c.Int(nullable: false),
                        IsMemberOfGym = c.Boolean(nullable: false),
                        CurrentGym = c.String(),
                        WorkoutTypeId = c.Long(nullable: false),
                        WorkoutDescription = c.String(),
                        DietTypeId = c.Long(nullable: false),
                        DietDetails = c.String(),
                        TrainingGoals = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AuthUser",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Email = c.String(maxLength: 256),
                        UserName = c.String(nullable: false, maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AuthLogin",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AuthUser", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AuthUserRole",
                c => new
                    {
                        UserId = c.Long(nullable: false),
                        RoleId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AuthUser", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AuthRole", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.ImageConfigurations",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SplashImageId = c.Long(nullable: false),
                        SectionImage1Id = c.Long(nullable: false),
                        SectionImage2Id = c.Long(nullable: false),
                        SectionImage3Id = c.Long(nullable: false),
                        Randomize = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Images",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ImagePath = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MailingList",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Email = c.String(nullable: false),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AuthRole",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AuthUserRole", "RoleId", "dbo.AuthRole");
            DropForeignKey("dbo.Customers", "UserId", "dbo.AuthUser");
            DropForeignKey("dbo.AuthUserRole", "UserId", "dbo.AuthUser");
            DropForeignKey("dbo.AuthLogin", "UserId", "dbo.AuthUser");
            DropForeignKey("dbo.AuthClaim", "UserId", "dbo.AuthUser");
            DropForeignKey("dbo.Purchases", "QuestionnareId", "dbo.Questionnaires");
            DropForeignKey("dbo.PurchasedItems", "PurchaseId", "dbo.Purchases");
            DropForeignKey("dbo.PurchasedItems", "ItemId", "dbo.Items");
            DropForeignKey("dbo.Items", "PlanId", "dbo.Plans");
            DropForeignKey("dbo.Purchases", "DiscountCodeId", "dbo.DiscountCodes");
            DropForeignKey("dbo.Purchases", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.Customers", "BillingAddressId", "dbo.Addresses");
            DropForeignKey("dbo.BlogImages", "BlogId", "dbo.Blogs");
            DropIndex("dbo.AuthRole", "RoleNameIndex");
            DropIndex("dbo.AuthUserRole", new[] { "RoleId" });
            DropIndex("dbo.AuthUserRole", new[] { "UserId" });
            DropIndex("dbo.AuthLogin", new[] { "UserId" });
            DropIndex("dbo.AuthUser", "UserNameIndex");
            DropIndex("dbo.Items", new[] { "PlanId" });
            DropIndex("dbo.PurchasedItems", new[] { "PurchaseId" });
            DropIndex("dbo.PurchasedItems", new[] { "ItemId" });
            DropIndex("dbo.Purchases", new[] { "QuestionnareId" });
            DropIndex("dbo.Purchases", new[] { "CustomerId" });
            DropIndex("dbo.Purchases", new[] { "DiscountCodeId" });
            DropIndex("dbo.Customers", new[] { "UserId" });
            DropIndex("dbo.Customers", new[] { "BillingAddressId" });
            DropIndex("dbo.AuthClaim", new[] { "UserId" });
            DropIndex("dbo.BlogImages", new[] { "BlogId" });
            DropTable("dbo.AuthRole");
            DropTable("dbo.MailingList");
            DropTable("dbo.Images");
            DropTable("dbo.ImageConfigurations");
            DropTable("dbo.AuthUserRole");
            DropTable("dbo.AuthLogin");
            DropTable("dbo.AuthUser");
            DropTable("dbo.Questionnaires");
            DropTable("dbo.Plans");
            DropTable("dbo.Items");
            DropTable("dbo.PurchasedItems");
            DropTable("dbo.DiscountCodes");
            DropTable("dbo.Purchases");
            DropTable("dbo.Customers");
            DropTable("dbo.AuthClaim");
            DropTable("dbo.Blogs");
            DropTable("dbo.BlogImages");
            DropTable("dbo.Addresses");
        }
    }
}
