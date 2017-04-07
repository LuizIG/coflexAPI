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

        Private db As New ApplicationDbContext

        ' GET: api/Quotations
        Function GetQuotationSet() As IQueryable(Of Quotation)
            Return db.QuotationSet
        End Function

        ' GET: api/Quotations/5
        <ResponseType(GetType(Quotation))>
        Async Function GetQuotation(ByVal id As Integer) As Task(Of IHttpActionResult)
            Dim quotation As Quotation = Await db.QuotationSet.FindAsync(id)
            If IsNothing(quotation) Then
                Return NotFound()
            End If

            Return Ok(quotation)
        End Function

        ' PUT: api/Quotations/5
        <ResponseType(GetType(Void))>
        Async Function PutQuotation(ByVal id As Integer, ByVal quotation As Quotation) As Task(Of IHttpActionResult)
            If Not ModelState.IsValid Then
                Return BadRequest(ModelState)
            End If

            If Not id = quotation.Id Then
                Return BadRequest()
            End If

            db.Entry(quotation).State = EntityState.Modified

            Try
                Await db.SaveChangesAsync()
            Catch ex As DbUpdateConcurrencyException
                If Not (QuotationExists(id)) Then
                    Return NotFound()
                Else
                    Throw
                End If
            End Try

            Return StatusCode(HttpStatusCode.NoContent)
        End Function

        ' POST: api/Quotations
        <ResponseType(GetType(Quotation))>
        Async Function PostQuotation(ByVal quotation As Quotation) As Task(Of IHttpActionResult)
            If Not ModelState.IsValid Then
                Return BadRequest(ModelState)
            End If

            db.QuotationSet.Add(quotation)
            Await db.SaveChangesAsync()

            Return CreatedAtRoute("DefaultApi", New With {.id = quotation.Id}, quotation)
        End Function

        ' DELETE: api/Quotations/5
        <ResponseType(GetType(Quotation))>
        Async Function DeleteQuotation(ByVal id As Integer) As Task(Of IHttpActionResult)
            Dim quotation As Quotation = Await db.QuotationSet.FindAsync(id)
            If IsNothing(quotation) Then
                Return NotFound()
            End If

            db.QuotationSet.Remove(quotation)
            Await db.SaveChangesAsync()

            Return Ok(quotation)
        End Function

        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If (disposing) Then
                db.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub

        Private Function QuotationExists(ByVal id As Integer) As Boolean
            Return db.QuotationSet.Count(Function(e) e.Id = id) > 0
        End Function
    End Class
End Namespace