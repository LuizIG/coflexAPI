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
    Public Class ItemsController
        Inherits System.Web.Http.ApiController

        Private db As New CoflexDBEntities1

        ' GET: api/Items
        Function GetItems() As IQueryable(Of Items)
            Return db.Items
        End Function

        ' GET: api/Items/5
        <ResponseType(GetType(Items))>
        Async Function GetItems(ByVal id As Integer) As Task(Of IHttpActionResult)
            Dim items As Items = Await db.Items.FindAsync(id)
            If IsNothing(items) Then
                Return NotFound()
            End If

            Return Ok(items)
        End Function

        ' PUT: api/Items/5
        <ResponseType(GetType(Void))>
        Async Function PutItems(ByVal id As Integer, ByVal items As Items) As Task(Of IHttpActionResult)
            If Not ModelState.IsValid Then
                Return BadRequest(ModelState)
            End If

            If Not id = items.Id Then
                Return BadRequest()
            End If

            db.Entry(items).State = EntityState.Modified

            Try
                Await db.SaveChangesAsync()
            Catch ex As DbUpdateConcurrencyException
                If Not (ItemsExists(id)) Then
                    Return NotFound()
                Else
                    Throw
                End If
            End Try

            Return StatusCode(HttpStatusCode.NoContent)
        End Function

        ' POST: api/Items
        <ResponseType(GetType(Items))>
        Async Function PostItems(ByVal items As Items) As Task(Of IHttpActionResult)
            If Not ModelState.IsValid Then
                Return BadRequest(ModelState)
            End If

            db.Items.Add(items)
            Await db.SaveChangesAsync()

            Return CreatedAtRoute("DefaultApi", New With {.id = items.Id}, items)
        End Function

        ' DELETE: api/Items/5
        <ResponseType(GetType(Items))>
        Async Function DeleteItems(ByVal id As Integer) As Task(Of IHttpActionResult)
            Dim items As Items = Await db.Items.FindAsync(id)
            If IsNothing(items) Then
                Return NotFound()
            End If

            db.Items.Remove(items)
            Await db.SaveChangesAsync()

            Return Ok(items)
        End Function

        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If (disposing) Then
                db.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub

        Private Function ItemsExists(ByVal id As Integer) As Boolean
            Return db.Items.Count(Function(e) e.Id = id) > 0
        End Function
    End Class
End Namespace