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
    Public Class ItemComponentsController
        Inherits System.Web.Http.ApiController

        Private db As New GP_desarrolloEntities

        ' GET: api/ItemComponents
        Function GetVistaItemComponents() As IQueryable(Of VistaItemComponents)
            Return db.VistaItemComponents
        End Function

        ' GET: api/ItemComponents/5
        <ResponseType(GetType(VistaItemComponents))>
        Async Function GetVistaItemComponents(ByVal id As String) As Task(Of IHttpActionResult)
            Dim vistaItemComponents = Await GetVistaItemComponents().Where(Function(x) x.SkuArticulo.Equals(id)).ToListAsync()
            If IsNothing(vistaItemComponents) Then
                Return NotFound()
            End If
            Return Ok(vistaItemComponents)
        End Function


        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If (disposing) Then
                db.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub

        Private Function VistaItemComponentsExists(ByVal id As String) As Boolean
            Return db.VistaItemComponents.Count(Function(e) e.ITEMDESC = id) > 0
        End Function
    End Class
End Namespace