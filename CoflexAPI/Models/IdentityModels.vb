Imports System.Data.Entity
Imports System.Security.Claims
Imports System.Threading.Tasks
Imports Microsoft.AspNet.Identity
Imports Microsoft.AspNet.Identity.EntityFramework

' Para agregar datos de perfil al usuario, agregue más propiedades a la clase ApplicationUser. Para obtener más información, visite http://go.microsoft.com/fwlink/?LinkID=317594.
Public Class ApplicationUser
    Inherits IdentityUser

    Public Property Name As String
    Public Property PaternalSurname As String
    Public Property MaternalSurname As String
    Public Property RegisterDate As DateTime
    Public Property ActiveUser As Boolean
    Public Property Leader As String

    Public Async Function GenerateUserIdentityAsync(manager As UserManager(Of ApplicationUser), authenticationType As String) As Task(Of ClaimsIdentity)
        ' Tenga en cuenta que el valor de authenticationType debe coincidir con el definido en CookieAuthenticationOptions.AuthenticationType
        Dim userIdentity = Await manager.CreateIdentityAsync(Me, authenticationType)
        ' Agregar aquí notificaciones personalizadas de usuario
        Return userIdentity
    End Function
End Class

Public Class ApplicationDbContext
    Inherits IdentityDbContext(Of ApplicationUser)
    Public Sub New()
        MyBase.New("DefaultConnection", throwIfV1Schema:=False)
    End Sub

    Public Shared Function Create() As ApplicationDbContext
        Return New ApplicationDbContext
    End Function

    Public Property Quotations As System.Data.Entity.DbSet(Of Quotations)
    Public Property QuotationVersions As System.Data.Entity.DbSet(Of QuotationVersions)
    Public Property Items As System.Data.Entity.DbSet(Of Items)
    Public Property ItemsComponents As System.Data.Entity.DbSet(Of ItemsComponents)
End Class
