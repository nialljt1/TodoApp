namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Bookings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OrganiserForename = c.String(nullable: false, maxLength: 50),
                        OrganiserSurname = c.String(nullable: false, maxLength: 50),
                        OrganiserTelephoneNumber = c.String(nullable: false, maxLength: 50),
                        NumberOfDiners = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Bookings");
        }
    }
}
