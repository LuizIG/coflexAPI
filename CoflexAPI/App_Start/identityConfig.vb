Imports System.Threading.Tasks
Imports Microsoft.AspNet.Identity
Imports Microsoft.AspNet.Identity.EntityFramework
Imports Microsoft.AspNet.Identity.Owin
Imports Microsoft.Owin

' Configure el administrador de usuarios de aplicación que se usa en esta aplicación. UserManager se define en ASP.NET Identity y se usa en la aplicación.
Public Class ApplicationUserManager
    Inherits UserManager(Of ApplicationUser)

    Public Sub New(store As IUserStore(Of ApplicationUser))
        MyBase.New(store)
    End Sub

    Public Shared Function Create(options As IdentityFactoryOptions(Of ApplicationUserManager), context As IOwinContext) As ApplicationUserManager
        Dim manager = New ApplicationUserManager(New UserStore(Of ApplicationUser)(context.Get(Of ApplicationDbContext)()))

        ' Configure la lógica de validación de nombres de usuario
        manager.UserValidator = New UserValidator(Of ApplicationUser)(manager) With {
            .AllowOnlyAlphanumericUserNames = False,
            .RequireUniqueEmail = True
        }

        ' Configure la lógica de validación de contraseñas
        manager.PasswordValidator = New PasswordValidator With {
            .RequiredLength = 6,
            .RequireNonLetterOrDigit = False,
            .RequireDigit = False,
            .RequireLowercase = False,
            .RequireUppercase = False
        }

        Dim dataProtectionProvider = options.DataProtectionProvider
        If (dataProtectionProvider IsNot Nothing) Then
            manager.UserTokenProvider = New DataProtectorTokenProvider(Of ApplicationUser)(dataProtectionProvider.Create("ASP.NET Identity"))
        End If

        Return manager
    End Function
End Class
