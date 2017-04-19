Imports System.ComponentModel.DataAnnotations

Public Class NewComponentsBindingModel

    <Required>
    <Display(Name:="Sku Componente")>
    Public Property SkuComponente As String
    <Required>
    <Display(Name:="Id Cotizacion")>
    Public Property IdQuotation As Integer
    <Required>
    <Display(Name:="Unidad")>
    Public Property Uofm As String
    <Required>
    <Display(Name:="Costo Estandar")>
    Public Property StndCost As Decimal

End Class
