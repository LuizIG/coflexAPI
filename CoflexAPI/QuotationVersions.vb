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

Partial Public Class QuotationVersions
    Public Property Id As Integer
    Public Property QuotationsId As Integer
    Public Property VersionNumber As Integer
    Public Property ExchangeRate As Double
    Public Property [Date] As Date
    Public Property Status As Integer
    Public Property UseStndCost As Boolean

    Public Overridable Property Items As ICollection(Of Items) = New HashSet(Of Items)

End Class
