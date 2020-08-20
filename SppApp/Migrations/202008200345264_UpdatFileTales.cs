namespace SppApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatFileTales : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GRADJEVINSKE_DOZVOLE", "Naziv", c => c.String(maxLength: 500));
            AddColumn("dbo.OSTALA_DOKUMENTACIJA", "Naziv", c => c.String(maxLength: 500));
            AlterColumn("dbo.GRADJEVINSKE_DOZVOLE", "Putanja", c => c.String());
            AlterColumn("dbo.OSTALA_DOKUMENTACIJA", "Putanja", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.OSTALA_DOKUMENTACIJA", "Putanja", c => c.String(maxLength: 500));
            AlterColumn("dbo.GRADJEVINSKE_DOZVOLE", "Putanja", c => c.String(maxLength: 500));
            DropColumn("dbo.OSTALA_DOKUMENTACIJA", "Naziv");
            DropColumn("dbo.GRADJEVINSKE_DOZVOLE", "Naziv");
        }
    }
}
