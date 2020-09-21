namespace SppApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update3 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AKTIVNOSTI", "Vrsta", c => c.String(maxLength: 50));
            AlterColumn("dbo.DIONICI", "Vrsta", c => c.String(maxLength: 50));
            AlterColumn("dbo.FINANCIRANJA", "IzvorFinanciranja", c => c.String(maxLength: 50));
            AlterColumn("dbo.FINANCIRANJA", "IzvorSufinanciranja", c => c.String(maxLength: 50));
            AlterColumn("dbo.JAVNE_NABAVE", "VrstaPostupka", c => c.String(maxLength: 250));
            AlterColumn("dbo.RIZICI", "Vrsta", c => c.String(maxLength: 50));
            AlterColumn("dbo.RIZICI", "Vjerojatnost", c => c.String(maxLength: 50));
            AlterColumn("dbo.USKLADJENOSTI", "Naziv", c => c.String(maxLength: 500));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.USKLADJENOSTI", "Naziv", c => c.String(maxLength: 250));
            AlterColumn("dbo.RIZICI", "Vjerojatnost", c => c.String(maxLength: 25));
            AlterColumn("dbo.RIZICI", "Vrsta", c => c.String(maxLength: 25));
            AlterColumn("dbo.JAVNE_NABAVE", "VrstaPostupka", c => c.String(maxLength: 50));
            AlterColumn("dbo.FINANCIRANJA", "IzvorSufinanciranja", c => c.String(maxLength: 150));
            AlterColumn("dbo.FINANCIRANJA", "IzvorFinanciranja", c => c.String(maxLength: 150));
            AlterColumn("dbo.DIONICI", "Vrsta", c => c.String(maxLength: 25));
            AlterColumn("dbo.AKTIVNOSTI", "Vrsta", c => c.String(maxLength: 25));
        }
    }
}
