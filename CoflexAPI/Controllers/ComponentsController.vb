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
        Function GetVistaComponents() As IQueryable(Of VistaComponents)
            Return db.VistaComponents
        End Function

        ' GET: api/Components/5
        <ResponseType(GetType(VistaComponents))>
        Async Function GetVistaComponents(ByVal id As String) As Task(Of IHttpActionResult)
            Dim vistaComponents As VistaComponents = Await db.VistaComponents.FindAsync(id)
            If IsNothing(vistaComponents) Then
                Return NotFound()
            End If

            Return Ok(vistaComponents)
        End Function

        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If (disposing) Then
                db.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub

        Private Function VistaComponentsExists(ByVal id As String) As Boolean
            Return db.VistaComponents.Count(Function(e) e.PPN_I = id) > 0
        End Function
    End Class
End Namespace