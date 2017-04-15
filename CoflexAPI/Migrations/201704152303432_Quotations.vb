Imports System
Imports System.Data.Entity.Migrations
Imports Microsoft.VisualBasic

Namespace Migrations
    Public Partial Class Quotations
        Inherits DbMigration
    
        Public Overrides Sub Up()
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
                .PrimaryKey(Function(t) t.Id) _
                .ForeignKey("dbo.Quotations", Function(t) t.QuotationsId, cascadeDelete := True) _
                .Index(Function(t) t.QuotationsId)
            
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
                .PrimaryKey(Function(t) t.Id) _
                .ForeignKey("dbo.QuotationVersions", Function(t) t.QuotationVersionsId, cascadeDelete := True) _
                .Index(Function(t) t.QuotationVersionsId)
            
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
                .PrimaryKey(Function(t) t.Id) _
                .ForeignKey("dbo.Items", Function(t) t.ItemsId, cascadeDelete := True) _
                .Index(Function(t) t.ItemsId)
            
        End Sub
        
        Public Overrides Sub Down()
            DropForeignKey("dbo.QuotationVersions", "QuotationsId", "dbo.Quotations")
            DropForeignKey("dbo.Items", "QuotationVersionsId", "dbo.QuotationVersions")
            DropForeignKey("dbo.ItemsComponents", "ItemsId", "dbo.Items")
            DropIndex("dbo.ItemsComponents", New String() { "ItemsId" })
            DropIndex("dbo.Items", New String() { "QuotationVersionsId" })
            DropIndex("dbo.QuotationVersions", New String() { "QuotationsId" })
            DropTable("dbo.ItemsComponents")
            DropTable("dbo.Items")
            DropTable("dbo.QuotationVersions")
            DropTable("dbo.Quotations")
        End Sub
    End Class
End Namespace
