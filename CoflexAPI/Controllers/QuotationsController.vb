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
    Public Class QuotationsController
        Inherits System.Web.Http.ApiController

        Private db As New CoflexDBEntities1

        ' GET: api/Quotations
        Function GetQuotations() As IQueryable(Of Quotations)
            Return db.Quotations
        End Function

        ' GET: api/Quotations/5
        <ResponseType(GetType(Quotations))>
        Async Function GetQuotations(ByVal id As Integer) As Task(Of IHttpActionResult)
            Dim quotations As Quotations = Await db.Quotations.FindAsync(id)
            If IsNothing(quotations) Then
                Return NotFound()
            End If

            Return Ok(quotations)
        End Function

        ' PUT: api/Quotations/5
        <ResponseType(GetType(Void))>
        Async Function PutQuotations(ByVal id As Integer, ByVal quotations As Quotations) As Task(Of IHttpActionResult)
            If Not ModelState.IsValid Then
                Return BadRequest(ModelState)
            End If

            If Not id = quotations.Id Then
                Return BadRequest()
            End If

            db.Entry(quotations).State = EntityState.Modified

            Try
                Await db.SaveChangesAsync()
            Catch ex As DbUpdateConcurrencyException
                If Not (QuotationsExists(id)) Then
                    Return NotFound()
                Else
                    Throw
                End If
            End Try

            Return StatusCode(HttpStatusCode.NoContent)
        End Function

        ' POST: api/Quotations
        <ResponseType(GetType(Quotations))>
        Async Function PostQuotations(ByVal quotations As Quotations) As Task(Of IHttpActionResult)
            If Not ModelState.IsValid Then
                Return BadRequest(ModelState)
            End If

            db.Quotations.Add(quotations)
            Await db.SaveChangesAsync()

            Return CreatedAtRoute("DefaultApi", New With {.id = quotations.Id}, quotations)
        End Function

        ' DELETE: api/Quotations/5
        <ResponseType(GetType(Quotations))>
        Async Function DeleteQuotations(ByVal id As Integer) As Task(Of IHttpActionResult)
            Dim quotations As Quotations = Await db.Quotations.FindAsync(id)
            If IsNothing(quotations) Then
                Return NotFound()
            End If

            db.Quotations.Remove(quotations)
            Await db.SaveChangesAsync()

            Return Ok(quotations)
        End Function

        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If (disposing) Then
                db.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub

        Private Function QuotationsExists(ByVal id As Integer) As Boolean
            Return db.Quotations.Count(Function(e) e.Id = id) > 0
        End Function
    End Class
End Namespace