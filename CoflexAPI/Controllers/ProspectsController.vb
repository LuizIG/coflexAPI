Imports System.Data
Imports System.Data.Entity
Imports System.Data.Entity.Infrastructure
Imports System.Linq
Imports System.Net
Imports System.Net.Http
Imports System.Threading.Tasks
Imports System.Web.Http
Imports System.Web.Http.Description
Imports CoflexAPI
Imports Microsoft.AspNet.Identity

Namespace Controllers

    <Authorize>
    Public Class ProspectsController
        Inherits System.Web.Http.ApiController

        Private db As New CoflexDBEntities1

        ' GET: api/Prospects
        ''' <summary>
        ''' Obtiene la lista de prospectos
        ''' </summary>
        ''' <returns></returns>
        <HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)>
        Function GetProspect() As IQueryable(Of Prospect)
            Return db.Prospect
        End Function

        ' GET: api/Prospects/5
        ''' <summary>
        ''' Obtiene el detalle de un prospecto por Id
        ''' </summary>
        ''' <param name="id">Id del prospecto</param>
        ''' <returns></returns>
        <ResponseType(GetType(Prospect))>
        <HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)>
        Async Function GetProspect(ByVal id As Integer) As Task(Of IHttpActionResult)
            Dim prospect As Prospect = Await db.Prospect.FindAsync(id)
            If IsNothing(prospect) Then
                Return NotFound()
            End If

            Return Ok(prospect)
        End Function

        ' PUT: api/Prospects/5
        ''' <summary>
        ''' Actualiza la informacion de un prospecto
        ''' </summary>
        ''' <param name="id">Id del prospecto</param>
        ''' <param name="prospect">Modelo del prospecto</param>
        ''' <returns></returns>
        <ResponseType(GetType(Void))>
        <HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)>
        Async Function PutProspect(ByVal id As Integer, ByVal prospect As Prospect) As Task(Of IHttpActionResult)
            If Not ModelState.IsValid Then
                Return BadRequest(ModelState)
            End If

            db.Entry(prospect).State = EntityState.Modified

            Try
                Await db.SaveChangesAsync()
            Catch ex As DbUpdateConcurrencyException
                If Not (ProspectExists(id)) Then
                    Return NotFound()
                Else
                    Throw
                End If
            End Try

            Return StatusCode(HttpStatusCode.NoContent)
        End Function

        ' POST: api/Prospects
        ''' <summary>
        ''' Alta de prospectos
        ''' </summary>
        ''' <param name="prospect">Modelo de Prospecto</param>
        ''' <returns></returns>
        <ResponseType(GetType(Prospect))>
        <HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)>
        Async Function PostProspect(ByVal prospect As Prospect) As Task(Of IHttpActionResult)
            If Not ModelState.IsValid Then
                Return BadRequest(ModelState)
            End If

            db.Prospect.Add(prospect)
            Await db.SaveChangesAsync()

            Return CreatedAtRoute("DefaultApi", New With {.id = prospect.Id}, prospect)
        End Function

        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If (disposing) Then
                db.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub

        Private Function ProspectExists(ByVal id As Integer) As Boolean
            Return db.Prospect.Count(Function(e) e.Id = id) > 0
        End Function
    End Class
End Namespace