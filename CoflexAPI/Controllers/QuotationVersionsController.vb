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
        Function GetQuotationVersions() As IQueryable(Of QuotationVersion)
            Return db.QuotationVersions
        End Function

        ' GET: api/QuotationVersions/5
        <ResponseType(GetType(QuotationVersion))>
        Async Function GetQuotationVersion(ByVal id As Integer) As Task(Of IHttpActionResult)
            Dim quotationVersion As QuotationVersion = Await db.QuotationVersions.FindAsync(id)
            If IsNothing(quotationVersion) Then
                Return NotFound()
            End If

            Return Ok(quotationVersion)
        End Function

        ' PUT: api/QuotationVersions/5
        <ResponseType(GetType(Void))>
        Async Function PutQuotationVersion(ByVal id As Integer, ByVal quotationVersion As QuotationVersion) As Task(Of IHttpActionResult)
            If Not ModelState.IsValid Then
                Return BadRequest(ModelState)
            End If

            If Not id = quotationVersion.Id Then
                Return BadRequest()
            End If

            db.Entry(quotationVersion).State = EntityState.Modified

            Try
                Await db.SaveChangesAsync()
            Catch ex As DbUpdateConcurrencyException
                If Not (QuotationVersionExists(id)) Then
                    Return NotFound()
                Else
                    Throw
                End If
            End Try

            Return StatusCode(HttpStatusCode.NoContent)
        End Function

        ' POST: api/QuotationVersions
        <ResponseType(GetType(QuotationVersion))>
        Async Function PostQuotationVersion(ByVal quotationVersion As QuotationVersion) As Task(Of IHttpActionResult)
            If Not ModelState.IsValid Then
                Return BadRequest(ModelState)
            End If

            db.QuotationVersions.Add(quotationVersion)
            Await db.SaveChangesAsync()

            Return CreatedAtRoute("DefaultApi", New With {.id = quotationVersion.Id}, quotationVersion)
        End Function

        ' DELETE: api/QuotationVersions/5
        <ResponseType(GetType(QuotationVersion))>
        Async Function DeleteQuotationVersion(ByVal id As Integer) As Task(Of IHttpActionResult)
            Dim quotationVersion As QuotationVersion = Await db.QuotationVersions.FindAsync(id)
            If IsNothing(quotationVersion) Then
                Return NotFound()
            End If

            db.QuotationVersions.Remove(quotationVersion)
            Await db.SaveChangesAsync()

            Return Ok(quotationVersion)
        End Function

        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If (disposing) Then
                db.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub

        Private Function QuotationVersionExists(ByVal id As Integer) As Boolean
            Return db.QuotationVersions.Count(Function(e) e.Id = id) > 0
        End Function
    End Class
End Namespace