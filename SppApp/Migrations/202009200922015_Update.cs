namespace SppApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PROJEKTI", "Poslano", c => c.Boolean());
            AddColumn("dbo.PROJEKTI", "UpisanoSPUR", c => c.Boolean());
            DropColumn("dbo.PROJEKTI", "Upisano");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PROJEKTI", "Upisano", c => c.Boolean());
            DropColumn("dbo.PROJEKTI", "UpisanoSPUR");
            DropColumn("dbo.PROJEKTI", "Poslano");
        }
    }
}
