Imports System
Imports System.Data.Entity.Migrations
Imports Microsoft.VisualBasic

Namespace Migrations
    Public Partial Class ChangeModel
        Inherits DbMigration
    
        Public Overrides Sub Up()
            DropForeignKey("dbo.ItemsComponents", "ItemsId", "dbo.Items")
            DropForeignKey("dbo.Items", "QuotationVersionsId", "dbo.QuotationVersions")
            DropForeignKey("dbo.QuotationVersions", "QuotationsId", "dbo.Quotations")
            DropIndex("dbo.ItemsComponents", New String() { "ItemsId" })
            DropIndex("dbo.Items", New String() { "QuotationVersionsId" })
            DropIndex("dbo.QuotationVersions", New String() { "QuotationsId" })
            AddColumn("dbo.AspNetUsers", "Leader", Function(c) c.String())
            DropTable("dbo.ItemsComponents")
            DropTable("dbo.Items")
            DropTable("dbo.Quotations")
            DropTable("dbo.QuotationVersions")
        End Sub
        
        Public Overrides Sub Down()
            CreateTable(
                "dbo.QuotationVersions",
                Function(c) New With
                    {
                        .Id = c.Int(nullable := False, identity := True),
                        .QuotationsId = c.Int(nullable := False),
                        .VersionNumber = c.String(),
                        .ExchangeRate = c.Double(nullable := False),
                        ._Date = c.DateTime(name := "Date", nullable := False),
                        .Status = c.Int(nullable := False),
                        .UseStndCost = c.Boolean(nullable := False)
                    }) _
                .PrimaryKey(Function(t) t.Id)
            
            CreateTable(
                "dbo.Quotations",
                Function(c) New With
                    {
                        .Id = c.Int(nullable := False, identity := True),
                        .AspNetUsersId = c.String(),
                        .ClientId = c.String(),
                        .ClientName = c.String(),
                        ._Date = c.DateTime(name := "Date", nullable := False),
                        .Status = c.Int(nullable := False)
                    }) _
                .PrimaryKey(Function(t) t.Id)
            
            CreateTable(
                "dbo.Items",
                Function(c) New With
                    {
                        .Id = c.Int(nullable := False, identity := True),
                        .QuotationVersionsId = c.Int(nullable := False),
                        .ItemNumber = c.String(),
                        .Sku = c.String(),
                        .ItemDescription = c.String(),
                        .Quantity = c.Short(nullable := False),
                        .UM = c.String(),
                        .Status = c.String()
                    }) _
                .PrimaryKey(Function(t) t.Id)
            
            CreateTable(
                "dbo.ItemsComponents",
                Function(c) New With
                    {
                        .Id = c.Int(nullable := False, identity := True),
                        .SkuComponent = c.String(),
                        .ItemDescription = c.String(),
                        .Quantity = c.Int(nullable := False),
                        .UM = c.String(),
                        .ItemsId = c.Int(nullable := False),
                        .StndCost = c.Double(nullable := False),
                        .CurrCost = c.Double(nullable := False),
                        .Result = c.Double(nullable := False),
                        .Lvl1 = c.Int(nullable := False),
                        .Lvl2 = c.Int(nullable := False),
                        .Lvl3 = c.Int(nullable := False)
                    }) _
                .PrimaryKey(Function(t) t.Id)
            
            DropColumn("dbo.AspNetUsers", "Leader")
            CreateIndex("dbo.QuotationVersions", "QuotationsId")
            CreateIndex("dbo.Items", "QuotationVersionsId")
            CreateIndex("dbo.ItemsComponents", "ItemsId")
            AddForeignKey("dbo.QuotationVersions", "QuotationsId", "dbo.Quotations", "Id", cascadeDelete := True)
            AddForeignKey("dbo.Items", "QuotationVersionsId", "dbo.QuotationVersions", "Id", cascadeDelete := True)
            AddForeignKey("dbo.ItemsComponents", "ItemsId", "dbo.Items", "Id", cascadeDelete := True)
        End Sub
    End Class
End Namespace
