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

Partial Public Class Items
    Public Property Id As Integer
    Public Property QuotationVersionsId As Integer
    Public Property ItemNumber As String
    Public Property Sku As String
    Public Property ItemDescription As String
    Public Property Quantity As Decimal
    Public Property UM As String
    Public Property Status As String

    Public Overridable Property ItemsComponents As ICollection(Of ItemsComponents) = New HashSet(Of ItemsComponents)

End Class
