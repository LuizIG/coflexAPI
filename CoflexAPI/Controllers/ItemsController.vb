Imports System.Threading.Tasks
Imports System.Web.Http
Imports System.Web.Http.Description
Imports CoflexAPI

Namespace Controllers
    Public Class ItemsController
        Inherits System.Web.Http.ApiController

        Private db As New GP_desarrolloEntities

        ' GET: api/Items
        Function GetVistaItems() As IQueryable(Of VistaItems)
            Return db.VistaItems
        End Function

        ' GET: api/Items/5
        <ResponseType(GetType(VistaItems))>
        Async Function GetVistaItems(ByVal id As String) As Task(Of IHttpActionResult)
            Dim vistaItems As VistaItems = Await db.VistaItems.FindAsync(id)
            If IsNothing(vistaItems) Then
                Return NotFound()
            End If

            Return Ok(vistaItems)
        End Function

        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If (disposing) Then
                db.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub

        Private Function VistaItemsExists(ByVal id As String) As Boolean
            Return db.VistaItems.Count(Function(e) e.PPN_I = id) > 0
        End Function
    End Class
End Namespace