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
    Public Class CommentsController
        Inherits System.Web.Http.ApiController

        Private db As New CoflexDBEntities1

        ' GET: api/Comments
        Function GetQuoteComments() As IQueryable(Of QuoteComments)
            Return db.QuoteComments
        End Function

        ' GET: api/Comments/5
        <ResponseType(GetType(QuoteComments))>
        Async Function GetQuoteComments(ByVal id As Integer) As Task(Of IHttpActionResult)
            Dim quoteComments = Await db.QuoteComments.Where(Function(x) x.QuotationsVersionsId = id).ToArrayAsync
            If quoteComments.Length = 0 Then
                Return NotFound()
            End If

            If IsNothing(quoteComments(0)) Then
                Return NotFound()
            End If


            Return Ok(quoteComments(0))
        End Function

        ' PUT: api/Comments/5
        <ResponseType(GetType(Void))>
        Async Function PutQuoteComments(ByVal id As Integer, ByVal quoteComments As QuoteComments) As Task(Of IHttpActionResult)
            If Not ModelState.IsValid Then
                Return BadRequest(ModelState)
            End If

            Dim quoteCommentsList = Await db.QuoteComments.Where(Function(x) x.QuotationsVersionsId = id).ToArrayAsync
            If quoteCommentsList.Length = 0 Then
                'INSERTAR
                quoteComments.QuotationsVersionsId = id
                db.QuoteComments.Add(quoteComments)
                Await db.SaveChangesAsync()
                Return CreatedAtRoute("DefaultApi", New With {.id = quoteComments.Id}, quoteComments)
            End If
            quoteCommentsList(0).OrderDeliery = quoteComments.OrderDeliery
            quoteCommentsList(0).ValidOffer = quoteComments.ValidOffer
            quoteCommentsList(0).Atention = quoteComments.Atention
            quoteCommentsList(0).Configuration = quoteComments.Configuration
            quoteCommentsList(0).DeliveryTerms = quoteComments.DeliveryTerms

            db.Entry(quoteCommentsList(0)).State = EntityState.Modified

            Try
                Await db.SaveChangesAsync()
            Catch ex As DbUpdateConcurrencyException
                If Not (QuoteCommentsExists(id)) Then
                    Return NotFound()
                Else
                    Throw
                End If
            End Try

            Return StatusCode(HttpStatusCode.NoContent)
        End Function


        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If (disposing) Then
                db.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub

        Private Function QuoteCommentsExists(ByVal id As Integer) As Boolean
            Return db.QuoteComments.Count(Function(e) e.Id = id) > 0
        End Function
    End Class
End Namespace