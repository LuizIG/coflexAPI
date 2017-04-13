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
    Public Class ComponentsController
        Inherits System.Web.Http.ApiController

        Private db As New GP_desarrolloEntities

        ' GET: api/Components
        Function GetComponentsViews() As IQueryable(Of ComponentsView)
            Return db.ComponentsViews
        End Function

        ' GET: api/Components/5
        <ResponseType(GetType(ComponentsView))>
        Async Function GetComponentsView(ByVal id As String) As Task(Of IHttpActionResult)
            Dim componentsView As ComponentsView = Await db.ComponentsViews.FindAsync(id)
            If IsNothing(componentsView) Then
                Return NotFound()
            End If

            Return Ok(componentsView)
        End Function

        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If (disposing) Then
                db.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub

        Private Function ComponentsViewExists(ByVal id As String) As Boolean
            Return db.ComponentsViews.Count(Function(e) e.ID = id) > 0
        End Function
    End Class
End Namespace