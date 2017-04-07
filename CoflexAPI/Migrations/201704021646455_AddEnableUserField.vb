Imports System
Imports System.Data.Entity.Migrations
Imports Microsoft.VisualBasic

Namespace Migrations
    Public Partial Class AddEnableUserField
        Inherits DbMigration
    
        Public Overrides Sub Up()
            'AddColumn("dbo.AspNetUsers", "ActiveUser", Function(c) c.Boolean(nullable := False))
        End Sub
        
        Public Overrides Sub Down()
            'DropColumn("dbo.AspNetUsers", "ActiveUser")
        End Sub
    End Class
End Namespace
