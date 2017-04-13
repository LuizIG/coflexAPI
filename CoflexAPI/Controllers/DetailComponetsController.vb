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
    Public Class DetailComponetsController
        Inherits System.Web.Http.ApiController

        Private db As New GP_desarrolloEntities

        ' GET: api/DetailComponets
        Function GetDetailComponetsViews() As IQueryable(Of DetailComponetsView)
            Return db.DetailComponetsViews
        End Function

        ' GET: api/DetailComponets/5
        <ResponseType(GetType(DetailComponetsView))>
        Async Function GetDetailComponetsView(ByVal id As String) As Task(Of IHttpActionResult)
            Dim detailComponetsView = Await db.DetailComponetsViews.FindAsync(id) '.Where(Function(x) x.SkuArticulo = id).ToArrayAsync
            If IsNothing(detailComponetsView) Then
                Return NotFound()
            End If

            Return Ok(detailComponetsView)
        End Function
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If (disposing) Then
                db.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub

        Private Function DetailComponetsViewExists(ByVal id As String) As Boolean
            Return db.DetailComponetsViews.Count(Function(e) e.SkuArticulo = id) > 0
        End Function
    End Class
End Namespace