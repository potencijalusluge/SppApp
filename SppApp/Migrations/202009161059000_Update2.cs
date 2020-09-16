namespace SppApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ORGANIZACIJE", "OIB", c => c.String(nullable: false, maxLength: 15));
            AlterColumn("dbo.ORGANIZACIJE", "BrojTelefona", c => c.String(nullable: false, maxLength: 25));
            AlterColumn("dbo.ORGANIZACIJE", "Email", c => c.String(nullable: false, maxLength: 250));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ORGANIZACIJE", "Email", c => c.String(maxLength: 250));
            AlterColumn("dbo.ORGANIZACIJE", "BrojTelefona", c => c.String(maxLength: 25));
            AlterColumn("dbo.ORGANIZACIJE", "OIB", c => c.String(maxLength: 15));
        }
    }
}
