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

        Private db As New GP_desarrolloEntities

        ' GET: api/Items
        Function GetVistaItems() As IQueryable(Of VistaItem)
            Return db.VistaItems
        End Function

        ' GET: api/Items/5
        <ResponseType(GetType(VistaItem))>
        Async Function GetVistaItem(ByVal id As String) As Task(Of IHttpActionResult)
            Dim vistaItem As VistaItem = Await db.VistaItems.FindAsync(id)
            If IsNothing(vistaItem) Then
                Return NotFound()
            End If

            Return Ok(vistaItem)
        End Function

        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If (disposing) Then
                db.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub

        Private Function VistaItemExists(ByVal id As String) As Boolean
            Return db.VistaItems.Count(Function(e) e.ID = id) > 0
        End Function
    End Class
End Namespace