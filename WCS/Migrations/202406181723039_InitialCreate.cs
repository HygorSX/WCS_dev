namespace WCS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Lexmarks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Serial = c.String(),
                        DeviceManufacturer = c.String(),
                        DeviceModel = c.String(),
                        QuantidadeImpressaoTotal = c.Int(nullable: false),
                        PorcentagemKitManutenção = c.Int(nullable: false),
                        PorcentagemBlack = c.Int(nullable: false),
                        PorcentagemCyan = c.Int(nullable: false),
                        PorcentagemYellow = c.Int(nullable: false),
                        PorcentagemMagenta = c.Int(nullable: false),
                        PorcentagemFusor = c.Int(nullable: false),
                        PorcentagemBelt = c.Int(nullable: false),
                        PrinterStatus = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Lexmarks");
        }
    }
}
