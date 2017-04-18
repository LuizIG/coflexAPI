Imports System.ComponentModel.DataAnnotations

Public Class QuotationsBindingModel

    Public Class QuotationBindingModel
        <Required>
        <Display(Name:="Id del Cliente")>
        Public Property ClientId As String

        <Required>
        <Display(Name:="Nombre del Cliente")>
        Public Property ClientName As String

        <Required>
        <Display(Name:="Version")>
        Public Property QuotationVersions As QuotationVersionBindingModel
    End Class


    Public Class QuotationVersionBindingModel
        <Required>
        <Display(Name:="Tipo de Cambio")>
        Public Property ExchangeRate As Double

        <Required>
        <Display(Name:="Usar Costo Estandar")>
        Public Property UseStndCost As Boolean

        <Required>
        <Display(Name:="Items")>
        Public Property ItemsBindingModel As ICollection(Of ItemsBindingModel)


    End Class



    Public Class QuotationVersionIdBindingModel
        Inherits QuotationVersionBindingModel
        <Required>
        <Display(Name:="Tipo de Cambio")>
        Public Property IdQuotaions As Integer
    End Class



    Public Class ItemsBindingModel
        <Required>
        <Display(Name:="Sku")>
        Public Property Sku As String

        <Required>
        <Display(Name:="Descripcion")>
        Public Property ItemDescription As String

        <Required>
        <Display(Name:="Cantidad")>
        Public Property Quantity As Double

        <Required>
        <Display(Name:="UM")>
        Public Property UM As String

        <Required>
        <Display(Name:="Status")>
        Public Property Status As String

        <Required>
        <Display(Name:="ItemsComponents")>
        Public Property ItemsComponents As ICollection(Of ItemComponentsBindingModel)

    End Class


    Public Class ItemComponentsBindingModel
        <Required>
        <Display(Name:="SkuComponent")>
        Public Property SkuComponent As String

        <Required>
        <Display(Name:="Descripcion")>
        Public Property ItemDescription As String

        <Required>
        <Display(Name:="Cantidad")>
        Public Property Quantity As Double

        <Required>
        <Display(Name:="UM")>
        Public Property UM As String

        <Required>
        <Display(Name:="Costo Estandar")>
        Public Property StndCost As Double

        <Required>
        <Display(Name:="Costo Actual")>
        Public Property CurrCost As Double

        <Required>
        <Display(Name:="Resultado")>
        Public Property Result As Double

        <Required>
        <Display(Name:="Lvl1")>
        Public Property Lvl1 As Integer

        <Required>
        <Display(Name:="Lvl2")>
        Public Property Lvl2 As Integer

        <Required>
        <Display(Name:="Lvl3")>
        Public Property Lvl3 As Integer

    End Class


End Class
