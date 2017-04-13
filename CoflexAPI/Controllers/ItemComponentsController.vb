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
        Function GetVistaItemComponents() As IQueryable(Of VistaItemComponent)
            Return db.VistaItemComponents
        End Function

        ' GET: api/ItemComponents/5
        <ResponseType(GetType(IQueryable(Of VistaItemComponent)))>
        Async Function GetVistaItemComponent(ByVal id As String) As Task(Of IHttpActionResult)
            'Dim vistaItemComponent As VistaItemComponent = Await db.VistaItemComponents.FindAsync(id)
            Dim v = Await db.VistaItemComponents.Where(Function(x) x.SkuArticulo.Equals(id)).OrderBy(Function(x) x.Nivel1).ThenBy(Function(x) x.Nivel2).ThenBy(Function(x) x.Nivel3).ToListAsync
            If IsNothing(v) Then
                Return NotFound()
            End If

            Return Ok(v)
        End Function

        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If (disposing) Then
                db.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub

        Private Function VistaItemComponentExists(ByVal id As Decimal) As Boolean
            Return db.VistaItemComponents.Count(Function(e) e.STNDCOST = id) > 0
        End Function
    End Class
End Namespace