Imports System
Imports System.Data.Entity.Migrations
Imports Microsoft.VisualBasic

Namespace Migrations
    Public Partial Class AddPostAbstract
        Inherits DbMigration
    
        Public Overrides Sub Up()
            DropForeignKey("dbo.Items", "QuotationVersionId", "dbo.QuotationVersions")
            DropForeignKey("dbo.QuotationVersions", "QuotationId", "dbo.Quotations")
            DropIndex("dbo.QuotationVersions", New String() { "QuotationId" })
            DropIndex("dbo.Items", New String() { "QuotationVersionId" })
            AddColumn("dbo.QuotationVersions", "Quotations_Id", Function(c) c.Int())
            AddColumn("dbo.Items", "QuotationVersions_Id", Function(c) c.Int())
            CreateIndex("dbo.Items", "QuotationVersions_Id")
            CreateIndex("dbo.QuotationVersions", "Quotations_Id")
            AddForeignKey("dbo.Items", "QuotationVersions_Id", "dbo.QuotationVersions", "Id")
            AddForeignKey("dbo.QuotationVersions", "Quotations_Id", "dbo.Quotations", "Id")
        End Sub
        
        Public Overrides Sub Down()
            DropForeignKey("dbo.QuotationVersions", "Quotations_Id", "dbo.Quotations")
            DropForeignKey("dbo.Items", "QuotationVersions_Id", "dbo.QuotationVersions")
            DropIndex("dbo.QuotationVersions", New String() { "Quotations_Id" })
            DropIndex("dbo.Items", New String() { "QuotationVersions_Id" })
            DropColumn("dbo.Items", "QuotationVersions_Id")
            DropColumn("dbo.QuotationVersions", "Quotations_Id")
            CreateIndex("dbo.Items", "QuotationVersionId")
            CreateIndex("dbo.QuotationVersions", "QuotationId")
            AddForeignKey("dbo.QuotationVersions", "QuotationId", "dbo.Quotations", "Id", cascadeDelete := True)
            AddForeignKey("dbo.Items", "QuotationVersionId", "dbo.QuotationVersions", "Id", cascadeDelete := True)
        End Sub
    End Class
End Namespace
