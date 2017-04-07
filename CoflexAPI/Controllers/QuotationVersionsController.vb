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
    Public Class QuotationVersionsController
        Inherits System.Web.Http.ApiController

        Private db As New ApplicationDbContext

        ' GET: api/QuotationVersions
        Function GetQuotationVersions() As IQueryable(Of QuotationVersions)
            Return db.QuotationVersions
        End Function

        ' GET: api/QuotationVersions/5
        <ResponseType(GetType(QuotationVersions))>
        Async Function GetQuotationVersions(ByVal id As Integer) As Task(Of IHttpActionResult)
            Dim quotationVersions As QuotationVersions = Await db.QuotationVersions.FindAsync(id)
            If IsNothing(quotationVersions) Then
                Return NotFound()
            End If

            Return Ok(quotationVersions)
        End Function

        ' PUT: api/QuotationVersions/5
        <ResponseType(GetType(Void))>
        Async Function PutQuotationVersions(ByVal id As Integer, ByVal quotationVersions As QuotationVersions) As Task(Of IHttpActionResult)
            If Not ModelState.IsValid Then
                Return BadRequest(ModelState)
            End If

            If Not id = quotationVersions.Id Then
                Return BadRequest()
            End If

            db.Entry(quotationVersions).State = EntityState.Modified

            Try
                Await db.SaveChangesAsync()
            Catch ex As DbUpdateConcurrencyException
                If Not (QuotationVersionsExists(id)) Then
                    Return NotFound()
                Else
                    Throw
                End If
            End Try

            Return StatusCode(HttpStatusCode.NoContent)
        End Function

        ' POST: api/QuotationVersions
        <ResponseType(GetType(QuotationVersions))>
        Async Function PostQuotationVersions(ByVal quotationVersions As QuotationVersions) As Task(Of IHttpActionResult)
            If Not ModelState.IsValid Then
                Return BadRequest(ModelState)
            End If

            db.QuotationVersions.Add(quotationVersions)
            Await db.SaveChangesAsync()

            Return CreatedAtRoute("DefaultApi", New With {.id = quotationVersions.Id}, quotationVersions)
        End Function

        ' DELETE: api/QuotationVersions/5
        <ResponseType(GetType(QuotationVersions))>
        Async Function DeleteQuotationVersions(ByVal id As Integer) As Task(Of IHttpActionResult)
            Dim quotationVersions As QuotationVersions = Await db.QuotationVersions.FindAsync(id)
            If IsNothing(quotationVersions) Then
                Return NotFound()
            End If

            db.QuotationVersions.Remove(quotationVersions)
            Await db.SaveChangesAsync()

            Return Ok(quotationVersions)
        End Function

        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If (disposing) Then
                db.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub

        Private Function QuotationVersionsExists(ByVal id As Integer) As Boolean
            Return db.QuotationVersions.Count(Function(e) e.Id = id) > 0
        End Function
    End Class
End Namespace