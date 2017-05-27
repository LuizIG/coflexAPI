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
    <Authorize>
    Public Class CatUOFMController
        Inherits System.Web.Http.ApiController

        Private db As New CoflexDBEntities1

        ' GET: api/CatUOFM

        ''' <summary>
        ''' Catalogo de unidades de medida
        ''' </summary>
        ''' <returns></returns>
        Function GetCatUOFMView() As IQueryable(Of CatUOFMView)
            Return db.CatUOFMView
        End Function

        ' GET: api/CatUOFM/5
        <ResponseType(GetType(CatUOFMView))>
        Async Function GetCatUOFMView(ByVal id As String) As Task(Of IHttpActionResult)
            Dim catUOFMView As CatUOFMView = Await db.CatUOFMView.FindAsync(id)
            If IsNothing(catUOFMView) Then
                Return NotFound()
            End If

            Return Ok(catUOFMView)
        End Function

        ' PUT: api/CatUOFM/5
        <ResponseType(GetType(Void))>
        Async Function PutCatUOFMView(ByVal id As String, ByVal catUOFMView As CatUOFMView) As Task(Of IHttpActionResult)
            If Not ModelState.IsValid Then
                Return BadRequest(ModelState)
            End If

            If Not id = catUOFMView.Id Then
                Return BadRequest()
            End If

            db.Entry(catUOFMView).State = EntityState.Modified

            Try
                Await db.SaveChangesAsync()
            Catch ex As DbUpdateConcurrencyException
                If Not (CatUOFMViewExists(id)) Then
                    Return NotFound()
                Else
                    Throw
                End If
            End Try

            Return StatusCode(HttpStatusCode.NoContent)
        End Function

        ' POST: api/CatUOFM
        <ResponseType(GetType(CatUOFMView))>
        Async Function PostCatUOFMView(ByVal catUOFMView As CatUOFMView) As Task(Of IHttpActionResult)
            If Not ModelState.IsValid Then
                Return BadRequest(ModelState)
            End If

            db.CatUOFMView.Add(catUOFMView)

            Try
                Await db.SaveChangesAsync()
            Catch ex As DbUpdateException
                If (CatUOFMViewExists(catUOFMView.Id)) Then
                    Return Conflict()
                Else
                    Throw
                End If
            End Try

            Return CreatedAtRoute("DefaultApi", New With {.id = catUOFMView.Id}, catUOFMView)
        End Function

        ' DELETE: api/CatUOFM/5
        <ResponseType(GetType(CatUOFMView))>
        Async Function DeleteCatUOFMView(ByVal id As String) As Task(Of IHttpActionResult)
            Dim catUOFMView As CatUOFMView = Await db.CatUOFMView.FindAsync(id)
            If IsNothing(catUOFMView) Then
                Return NotFound()
            End If

            db.CatUOFMView.Remove(catUOFMView)
            Await db.SaveChangesAsync()

            Return Ok(catUOFMView)
        End Function

        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If (disposing) Then
                db.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub

        Private Function CatUOFMViewExists(ByVal id As String) As Boolean
            Return db.CatUOFMView.Count(Function(e) e.Id = id) > 0
        End Function
    End Class
End Namespace