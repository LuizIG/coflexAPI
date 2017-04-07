Imports System
Imports System.Data.Entity.Migrations
Imports Microsoft.VisualBasic

Namespace Migrations
    Public Partial Class AddNewModelForQuotations
        Inherits DbMigration
    
        Public Overrides Sub Up()
            CreateTable(
                "dbo.QuotationVersions",
                Function(c) New With
                    {
                        .Id = c.Int(nullable := False, identity := True),
                        .QuotationId = c.Int(nullable := False),
                        .VersionNumber = c.String(),
                        .ExchangeRate = c.Double(nullable := False),
                        ._Date = c.DateTime(name := "Date", nullable := False),
                        .Status = c.Int(nullable := False),
                        .UseStndCost = c.Boolean(nullable := False)
                    }) _
                .PrimaryKey(Function(t) t.Id) _
                .ForeignKey("dbo.Quotations", Function(t) t.QuotationId, cascadeDelete := True) _
                .Index(Function(t) t.QuotationId)
            
            CreateTable(
                "dbo.Items",
                Function(c) New With
                    {
                        .Id = c.Int(nullable := False, identity := True),
                        .QuotationVersionId = c.Int(nullable := False),
                        .ItemNumber = c.String(),
                        .Sku = c.String(),
                        .ItemDescription = c.String(),
                        .Quantity = c.Short(nullable := False),
                        .UM = c.String(),
                        .Status = c.String()
                    }) _
                .PrimaryKey(Function(t) t.Id) _
                .ForeignKey("dbo.QuotationVersions", Function(t) t.QuotationVersionId, cascadeDelete := True) _
                .Index(Function(t) t.QuotationVersionId)
            
            CreateTable(
                "dbo.ItemsComponents",
                Function(c) New With
                    {
                        .Id = c.Int(nullable := False, identity := True),
                        .ItemsId = c.Int(nullable := False),
                        .SkuComponent = c.String(),
                        .ItemDescription = c.String(),
                        .Quantity = c.Int(nullable := False),
                        .UM = c.String(),
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
            
            AddColumn("dbo.Quotations", "AspNetUsersId", Function(c) c.String())
            AddColumn("dbo.Quotations", "ClientId", Function(c) c.String())
            AddColumn("dbo.Quotations", "Date", Function(c) c.DateTime(nullable := False))
            AddColumn("dbo.Quotations", "Status", Function(c) c.Int(nullable := False))
        End Sub
        
        Public Overrides Sub Down()
            DropForeignKey("dbo.QuotationVersions", "QuotationId", "dbo.Quotations")
            DropForeignKey("dbo.Items", "QuotationVersionId", "dbo.QuotationVersions")
            DropForeignKey("dbo.ItemsComponents", "ItemsId", "dbo.Items")
            DropIndex("dbo.ItemsComponents", New String() { "ItemsId" })
            DropIndex("dbo.Items", New String() { "QuotationVersionId" })
            DropIndex("dbo.QuotationVersions", New String() { "QuotationId" })
            DropColumn("dbo.Quotations", "Status")
            DropColumn("dbo.Quotations", "Date")
            DropColumn("dbo.Quotations", "ClientId")
            DropColumn("dbo.Quotations", "AspNetUsersId")
            DropTable("dbo.ItemsComponents")
            DropTable("dbo.Items")
            DropTable("dbo.QuotationVersions")
        End Sub
    End Class
End Namespace
