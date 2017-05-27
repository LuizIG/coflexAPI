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

Namespace Controllers
    Public Class ProspectsController
        Inherits System.Web.Http.ApiController

        Private db As New CoflexDBEntities1

        ' GET: api/Prospects
        Function GetProspect() As IQueryable(Of Prospect)
            Return db.Prospect
        End Function

        ' GET: api/Prospects/5
        <ResponseType(GetType(Prospect))>
        Async Function GetProspect(ByVal id As Integer) As Task(Of IHttpActionResult)
            Dim prospect As Prospect = Await db.Prospect.FindAsync(id)
            If IsNothing(prospect) Then
                Return NotFound()
            End If

            Return Ok(prospect)
        End Function

        ' PUT: api/Prospects/5
        <ResponseType(GetType(Void))>
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
        <ResponseType(GetType(Prospect))>
        Async Function PostProspect(ByVal prospect As Prospect) As Task(Of IHttpActionResult)
            If Not ModelState.IsValid Then
                Return BadRequest(ModelState)
            End If

            db.Prospect.Add(prospect)
            Await db.SaveChangesAsync()

            Return CreatedAtRoute("DefaultApi", New With {.id = prospect.Id}, prospect)
        End Function

        ' DELETE: api/Prospects/5
        <ResponseType(GetType(Prospect))>
        Async Function DeleteProspect(ByVal id As Integer) As Task(Of IHttpActionResult)
            Dim prospect As Prospect = Await db.Prospect.FindAsync(id)
            If IsNothing(prospect) Then
                Return NotFound()
            End If

            db.Prospect.Remove(prospect)
            Await db.SaveChangesAsync()

            Return Ok(prospect)
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