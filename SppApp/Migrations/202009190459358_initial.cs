namespace SppApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PROJEKTI", "DatumIzmjene", c => c.DateTime());
            DropColumn("dbo.PROJEKTI", "UvrstenNSR");
            DropColumn("dbo.PROJEKTI", "OdobrenNSR");
            DropColumn("dbo.PROJEKTI", "OdobrenjaJPP");
            DropColumn("dbo.PROJEKTI", "DatumUgovoraJPP");
            DropColumn("dbo.PROJEKTI", "PrijavljenEPP");
            DropColumn("dbo.PROJEKTI", "ObjavljenEPP");
            DropColumn("dbo.PROJEKTI", "PrijavljenEIB");
            DropColumn("dbo.PROJEKTI", "PredodobrenEIB");
            DropColumn("dbo.PROJEKTI", "DatumPrijaveVelikog");
            DropColumn("dbo.PROJEKTI", "OdobrenjeVelikog");
            DropColumn("dbo.PROJEKTI", "PotvrdaJASPERS");
            DropColumn("dbo.PROJEKTI", "VlasnickaDokumentacija");
            DropColumn("dbo.PROJEKTI", "StudijaIzvodivosti");
            DropColumn("dbo.PROJEKTI", "InvesticijskaStudija");
            DropColumn("dbo.PROJEKTI", "IdejnoRjesenje");
            DropColumn("dbo.PROJEKTI", "LokacijskaDozvola");
            DropColumn("dbo.PROJEKTI", "Sektor");
            DropColumn("dbo.PROJEKTI", "IzvorFinanciranja");
            DropColumn("dbo.PROJEKTI", "Ispravno");
            DropColumn("dbo.FINANCIRANJA", "IznosEUR");
            DropColumn("dbo.ORGANIZACIJE", "Mjesto");
            DropColumn("dbo.ORGANIZACIJE", "Drzava");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ORGANIZACIJE", "Drzava", c => c.String(maxLength: 50));
            AddColumn("dbo.ORGANIZACIJE", "Mjesto", c => c.String(nullable: false, maxLength: 150));
            AddColumn("dbo.FINANCIRANJA", "IznosEUR", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.PROJEKTI", "Ispravno", c => c.Boolean());
            AddColumn("dbo.PROJEKTI", "IzvorFinanciranja", c => c.String(maxLength: 250));
            AddColumn("dbo.PROJEKTI", "Sektor", c => c.String(maxLength: 150));
            AddColumn("dbo.PROJEKTI", "LokacijskaDozvola", c => c.String(maxLength: 25));
            AddColumn("dbo.PROJEKTI", "IdejnoRjesenje", c => c.String(maxLength: 25));
            AddColumn("dbo.PROJEKTI", "InvesticijskaStudija", c => c.String(maxLength: 25));
            AddColumn("dbo.PROJEKTI", "StudijaIzvodivosti", c => c.String(maxLength: 25));
            AddColumn("dbo.PROJEKTI", "VlasnickaDokumentacija", c => c.String(maxLength: 25));
            AddColumn("dbo.PROJEKTI", "PotvrdaJASPERS", c => c.Boolean());
            AddColumn("dbo.PROJEKTI", "OdobrenjeVelikog", c => c.DateTime());
            AddColumn("dbo.PROJEKTI", "DatumPrijaveVelikog", c => c.DateTime());
            AddColumn("dbo.PROJEKTI", "PredodobrenEIB", c => c.Boolean());
            AddColumn("dbo.PROJEKTI", "PrijavljenEIB", c => c.Boolean());
            AddColumn("dbo.PROJEKTI", "ObjavljenEPP", c => c.Boolean());
            AddColumn("dbo.PROJEKTI", "PrijavljenEPP", c => c.Boolean());
            AddColumn("dbo.PROJEKTI", "DatumUgovoraJPP", c => c.DateTime());
            AddColumn("dbo.PROJEKTI", "OdobrenjaJPP", c => c.DateTime());
            AddColumn("dbo.PROJEKTI", "OdobrenNSR", c => c.Boolean());
            AddColumn("dbo.PROJEKTI", "UvrstenNSR", c => c.Boolean());
            DropColumn("dbo.PROJEKTI", "DatumIzmjene");
        }
    }
}
