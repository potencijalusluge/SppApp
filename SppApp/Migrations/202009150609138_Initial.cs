namespace SppApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.KONTAKTI", "Prezime", c => c.String(maxLength: 150));
            AlterColumn("dbo.PROJEKTI", "IzvorFinanciranja", c => c.String(maxLength: 250));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PROJEKTI", "IzvorFinanciranja", c => c.String(nullable: false, maxLength: 250));
            AlterColumn("dbo.KONTAKTI", "Prezime", c => c.String(nullable: false, maxLength: 150));
        }
    }
}
