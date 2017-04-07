Imports System.Net.Http
Imports System.Security.Claims
Imports System.Security.Cryptography
Imports System.Threading.Tasks
Imports System.Web.Http
Imports System.Web.Http.Description
Imports Microsoft.AspNet.Identity
Imports Microsoft.AspNet.Identity.EntityFramework
Imports Microsoft.AspNet.Identity.Owin
Imports Microsoft.Owin.Security
Imports Microsoft.Owin.Security.Cookies

<Authorize>
<RoutePrefix("api/Account")>
Public Class AccountController
    Inherits ApiController
    Private Const LocalLoginProvider As String = "Local"

    Private _userManager As ApplicationUserManager
    Private m_AccessTokenFormat As ISecureDataFormat(Of AuthenticationTicket)

    Public Sub New()
    End Sub

    Public Sub New(userMan As ApplicationUserManager, accessTokenFormatType As ISecureDataFormat(Of AuthenticationTicket))
        Me.UserManager = userMan
        Me.AccessTokenFormat = accessTokenFormatType
    End Sub

    Public Property UserManager() As ApplicationUserManager
        Get
            Return If(_userManager, Request.GetOwinContext().GetUserManager(Of ApplicationUserManager)())
        End Get
        Private Set
            _userManager = Value
        End Set
    End Property

    Public Property AccessTokenFormat() As ISecureDataFormat(Of AuthenticationTicket)
        Get
            Return m_AccessTokenFormat
        End Get
        Private Set
            m_AccessTokenFormat = Value
        End Set
    End Property


    ' POST api/Account/Logout
    ''' <summary>
    ''' Termina la sesión
    ''' </summary>
    ''' <returns></returns>
    <Route("Logout")>
    <HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)>
    Public Function Logout() As IHttpActionResult
        Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType)
        Return Ok()
    End Function

    ' POST api/Account/ChangePassword
    ''' <summary>
    ''' Cambia el password
    ''' </summary>
    ''' <param name="model"></param>
    ''' <returns></returns>
    <Route("ChangePassword")>
    <HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)>
    Public Async Function ChangePassword(model As ChangePasswordBindingModel) As Task(Of IHttpActionResult)
        If Not ModelState.IsValid Then
            Return BadRequest(ModelState)
        End If
        Dim result As IdentityResult = Await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword)
        If Not result.Succeeded Then
            Return GetErrorResult(result)
        End If

        Return Ok()
    End Function

    ' POST api/Account/SetPassword
    ''' <summary>
    ''' Establece el password
    ''' </summary>
    ''' <param name="model"></param>
    ''' <returns></returns>
    <Route("SetPassword")>
    <Authorize(Roles:="Administrador")>
    <HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)>
    <ResponseType(GetType(Void))>
    Public Async Function SetPassword(model As SetPasswordBindingModel) As Task(Of IHttpActionResult)
        If Not ModelState.IsValid Then
            Return BadRequest(ModelState)
        End If
        Dim userItem = Await UserManager.FindByIdAsync(model.UserId)
        Dim remove As IdentityResult = Await UserManager.RemovePasswordAsync(model.UserId)
        Dim result As IdentityResult = Await UserManager.AddPasswordAsync(model.UserId, model.NewPassword)
        'Dim result As IdentityResult = Await UserManager.AddPasswordAsync(model.UserId, model.NewPassword)
        If Not result.Succeeded Then
            Return GetErrorResult(result)
        End If
        Return Ok()
    End Function

    ' POST api/Account/Register
    ''' <summary>
    ''' Alta de usuarios
    ''' </summary>
    ''' <param name="model"></param>
    ''' <returns></returns>
    <AllowAnonymous>
    <Route("Register")>
    <Authorize(Roles:="Administrador")>
    <HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)>
    Public Async Function Register(model As RegisterBindingModel) As Task(Of IHttpActionResult)
        If Not ModelState.IsValid Then
            Return BadRequest(ModelState)
        End If

        Dim user = New ApplicationUser() With {
            .UserName = model.Email,
            .Email = model.Email,
            .Name = model.Name,
            .PaternalSurname = model.PaternalSurname,
            .MaternalSurname = model.MaternalSurname,
            .RegisterDate = DateTime.Now,
            .ActiveUser = True
        }
        Dim result As IdentityResult = Await UserManager.CreateAsync(user, model.Password)
        Dim userCreated = UserManager.FindByEmail(model.Email)
        For Each role In model.Roles
            Await UserManager.AddToRoleAsync(userCreated.Id, role.Name)
        Next
        If Not result.Succeeded Then
            Return GetErrorResult(result)
        End If
        Return Ok()
    End Function

    ' DELETE api/Account/Enable
    ''' <summary>
    ''' Baja de usuarios
    ''' </summary>
    ''' <param name="model">Id del usuario y estado</param>
    ''' <returns></returns>
    <AllowAnonymous>
    <Route("Enable")>
    <Authorize(Roles:="Administrador")>
    <HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)>
    <HttpPut>
    Public Async Function Disable(model As DisableUsuerAccountBindingModel) As Task(Of IHttpActionResult)
        If Not ModelState.IsValid Then
            Return BadRequest(ModelState)
        End If
        Dim userItem = Await UserManager.FindByIdAsync(model.UserId)
        userItem.ActiveUser = model.Enable
        Dim result As IdentityResult = Await UserManager.UpdateAsync(userItem)
        If Not result.Succeeded Then
            Return GetErrorResult(result)
        End If
        Return Ok()
    End Function


    ' POST api/Account/Roles
    ''' <summary>
    ''' Agrega rol a un usuario
    ''' </summary>
    ''' <param name="model"></param>
    ''' <returns></returns>
    <HttpPost>
    <Authorize(Roles:="Administrador")>
    <HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)>
    <Route("Roles/Add")>
    Public Async Function addRoles(model As AddRoleBindingModel) As Task(Of IHttpActionResult)
        If Not ModelState.IsValid Then
            Return BadRequest(ModelState)
        End If
        Dim result = Await UserManager.AddToRoleAsync(model.UserId, model.RoleName)
        If Not result.Succeeded Then
            Return GetErrorResult(result)
        End If
        Return Ok()
    End Function

    ' PUT api/Account/Roles
    ''' <summary>
    ''' Elimina rol de un usuario
    ''' </summary>
    ''' <param name="model"></param>
    ''' <returns></returns>
    <HttpPost>
    <Authorize(Roles:="Administrador")>
    <HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)>
    <Route("Roles/Remove")>
    Public Async Function deleteRoles(model As AddRoleBindingModel) As Task(Of IHttpActionResult)
        If Not ModelState.IsValid Then
            Return BadRequest(ModelState)
        End If
        Dim result = Await UserManager.RemoveFromRoleAsync(model.UserId, model.RoleName)
        If Not result.Succeeded Then
            Return GetErrorResult(result)
        End If
        Return Ok()
    End Function


    ' GET api/Account/Users
    ''' <summary>
    ''' Obtiene una lista de todos los usuarios activos
    ''' </summary>
    ''' <returns></returns>
    <Authorize(Roles:="Administrador")>
    <HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)>
    <HttpGet>
    Public Async Function GetUsersList() As Task(Of List(Of UserViewModelWithRole))
        Dim usersList = UserManager.Users.ToList
        Dim usersRoleModelList As New List(Of UserViewModelWithRole)
        Dim RoleManager = New RoleManager(Of IdentityRole)(New RoleStore(Of IdentityRole)())
        For Each userItem In usersList
            Dim userWRole = New UserViewModelWithRole With {
                .Id = userItem.Id,
                .Name = userItem.Name,
                .PaternalSurname = userItem.PaternalSurname,
                .MaternalSurname = userItem.MaternalSurname,
                .Email = userItem.Email,
                .Enable = userItem.ActiveUser
            }
            userWRole.Roles = New List(Of UserRoleViewModel)
            For Each roleItem In userItem.Roles
                Dim roleresult = Await RoleManager.FindByIdAsync(roleItem.RoleId)
                Dim roleInUser As New UserRoleViewModel With {
                    .Name = roleresult.Name
                }
                userWRole.Roles.Add(roleInUser)
            Next
            usersRoleModelList.Add(userWRole)
        Next
        Return usersRoleModelList
    End Function


    ' GET api/Account/Users
    ''' <summary>
    ''' Obtiene un usuario especifico
    ''' </summary>
    ''' <param name="id">Id del usuario</param>
    ''' <returns></returns>
    <Authorize(Roles:="Administrador")>
    <HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)>
    <HttpGet>
    Public Async Function GetUser(ByVal id As String) As Task(Of IHttpActionResult)
        Dim userItem = Await UserManager.FindByIdAsync(id)

        If (userItem Is Nothing) Then
            ModelState.AddModelError("Message", "El usuario no existe")
            Return BadRequest(ModelState)
        End If

        Dim RoleManager = New RoleManager(Of IdentityRole)(New RoleStore(Of IdentityRole)())

        Dim userWRole = New UserViewModelWithRole With {
                .Id = userItem.Id,
                .Name = userItem.Name,
                .PaternalSurname = userItem.PaternalSurname,
                .MaternalSurname = userItem.MaternalSurname,
                .Email = userItem.Email
            }
        userWRole.Roles = New List(Of UserRoleViewModel)
        For Each roleItem In userItem.Roles
            Dim roleresult = Await RoleManager.FindByIdAsync(roleItem.RoleId)
            Dim roleInUser As New UserRoleViewModel With {
                    .Name = roleresult.Name
                }
            userWRole.Roles.Add(roleInUser)
        Next
        Return Ok(userWRole)
    End Function

    ' PUT api/Account/Users
    ''' <summary>
    ''' Actualiza la informacion de un usuarios
    ''' </summary>
    ''' <param name="model"></param>
    ''' <returns></returns>
    <Authorize(Roles:="Administrador")>
    <HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)>
    <HttpPost>
    <Route("Edit")>
    Public Async Function GetUser(ByVal model As UpdateUserBindingModel) As Task(Of IHttpActionResult)
        Dim userItem = Await UserManager.FindByIdAsync(model.IdUsuario)

        If (userItem Is Nothing) Then
            ModelState.AddModelError("Message", "El usuario no existe")
            Return BadRequest(ModelState)
        End If

        Dim RoleManager = New RoleManager(Of IdentityRole)(New RoleStore(Of IdentityRole)())
        With userItem
            .Email = model.Email
            .Name = model.Name
            .PaternalSurname = model.PaternalSurname
            .MaternalSurname = model.MaternalSurname
        End With

        Dim result As IdentityResult = Await UserManager.UpdateAsync(userItem)
        If Not result.Succeeded Then
            Return GetErrorResult(result)
        End If
        Return Ok()
    End Function


    Protected Overrides Sub Dispose(disposing As Boolean)
        If disposing AndAlso _userManager IsNot Nothing Then
            _userManager.Dispose()
            _userManager = Nothing
        End If

        MyBase.Dispose(disposing)
    End Sub

#Region "Helpers"

    Private ReadOnly Property Authentication() As IAuthenticationManager
        Get
            Return Request.GetOwinContext().Authentication
        End Get
    End Property

    Private Function GetErrorResult(result As IdentityResult) As IHttpActionResult
        If result Is Nothing Then
            Return InternalServerError()
        End If

        If Not result.Succeeded Then
            If result.Errors IsNot Nothing Then
                For Each [error] As String In result.Errors
                    ModelState.AddModelError("", [error])
                Next
            End If

            If ModelState.IsValid Then
                ' No hay disponibles errores ModelState para enviar, por lo que simplemente devuelva un BadRequest vacío.
                Return BadRequest()
            End If

            Return BadRequest(ModelState)
        End If

        Return Nothing
    End Function

    Private Class ExternalLoginData
        Public Property LoginProvider As String
        Public Property ProviderKey As String
        Public Property UserName As String

        Public Function GetClaims() As IList(Of Claim)
            Dim claims As IList(Of Claim) = New List(Of Claim)()
            claims.Add(New Claim(ClaimTypes.NameIdentifier, ProviderKey, Nothing, LoginProvider))

            If UserName IsNot Nothing Then
                claims.Add(New Claim(ClaimTypes.Name, UserName, Nothing, LoginProvider))
            End If

            Return claims
        End Function

        Public Shared Function FromIdentity(identity As ClaimsIdentity) As ExternalLoginData
            If identity Is Nothing Then
                Return Nothing
            End If

            Dim providerKeyClaim As Claim = identity.FindFirst(ClaimTypes.NameIdentifier)

            If providerKeyClaim Is Nothing OrElse [String].IsNullOrEmpty(providerKeyClaim.Issuer) OrElse [String].IsNullOrEmpty(providerKeyClaim.Value) Then
                Return Nothing
            End If

            If providerKeyClaim.Issuer = ClaimsIdentity.DefaultIssuer Then
                Return Nothing
            End If

            Return New ExternalLoginData() With {
                .LoginProvider = providerKeyClaim.Issuer,
                .ProviderKey = providerKeyClaim.Value,
                .UserName = identity.FindFirstValue(ClaimTypes.Name)
            }
        End Function
    End Class

    Private NotInheritable Class RandomOAuthStateGenerator
        Private Sub New()
        End Sub
        Private Shared _random As RandomNumberGenerator = New RNGCryptoServiceProvider()

        Public Shared Function Generate(strengthInBits As Integer) As String
            Const bitsPerByte As Integer = 8

            If strengthInBits Mod bitsPerByte <> 0 Then
                Throw New ArgumentException("strengthInBits debe ser uniformemente divisible por 8.", "strengthInBits")
            End If

            Dim strengthInBytes As Integer = strengthInBits \ bitsPerByte

            Dim data As Byte() = New Byte(strengthInBytes - 1) {}
            _random.GetBytes(data)
            Return HttpServerUtility.UrlTokenEncode(data)
        End Function
    End Class
#End Region
End Class
