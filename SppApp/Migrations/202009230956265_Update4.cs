namespace SppApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.USKLADJENOSTI", "XmlId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.USKLADJENOSTI", "XmlId");
        }
    }
}
