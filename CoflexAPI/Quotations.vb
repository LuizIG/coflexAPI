'------------------------------------------------------------------------------
' <auto-generated>
'     Este código se generó a partir de una plantilla.
'
'     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
'     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
' </auto-generated>
'------------------------------------------------------------------------------

Imports System
Imports System.Collections.Generic

Partial Public Class Quotations
    Public Property Id As Integer
    Public Property AspNetUsersId As String
    Public Property ClientId As String
    Public Property ClientName As String
    Public Property [Date] As Date
    Public Property Status As Integer
    Public Property CoflexId As String
    Public Property ProspectId As Nullable(Of Integer)

    Public Overridable Property QuotationVersions As ICollection(Of QuotationVersions) = New HashSet(Of QuotationVersions)
    Public Overridable Property AspNetUsersView As AspNetUsersView

End Class
