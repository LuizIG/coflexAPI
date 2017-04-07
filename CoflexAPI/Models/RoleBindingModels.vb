Imports System.ComponentModel.DataAnnotations
Imports Newtonsoft.Json

' Modelos usados como parámetros para las acciones de AccountController.
Public Class RoleAddBindingModel
    <Required>
    <Display(Name:="Rol")>
    Public Property Name As String
End Class
