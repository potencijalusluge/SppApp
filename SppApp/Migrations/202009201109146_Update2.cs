namespace SppApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PROJEKTI", "Doraditi", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PROJEKTI", "Doraditi");
        }
    }
}
