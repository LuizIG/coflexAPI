﻿Imports System.Data
Imports System.Data.Entity
Imports System.Data.Entity.Infrastructure
Imports System.Linq
Imports System.Net
Imports System.Net.Http
Imports System.Threading.Tasks
Imports System.Web.Http
Imports System.Web.Http.Description
Imports CoflexAPI
Imports Microsoft.AspNet.Identity

Namespace Controllers
    <Authorize>
    Public Class NewComponentsController
        Inherits System.Web.Http.ApiController

        Private db As New CoflexDBEntities1

        ' GET: api/NewComponents
        ''' <summary>
        ''' Obiene la lista de los componentes custom creados por usuarios
        ''' </summary>
        ''' <returns></returns>
        <HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)>
        Function GetNewComponents() As IQueryable(Of NewComponentsView)
            Return db.NewComponentsView
        End Function

        ' GET: api/NewComponents/5
        ''' <summary>
        ''' Obtiene el detalle de componentes custom
        ''' </summary>
        ''' <param name="id">Id del componente</param>
        ''' <returns></returns>
        <ResponseType(GetType(NewComponentsView))>
        <HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)>
        Async Function GetNewComponents(ByVal id As Integer) As Task(Of IHttpActionResult)
            Dim newComponents As NewComponentsView = Await db.NewComponentsView.FindAsync(id)
            If IsNothing(newComponents) Then
                Return NotFound()
            End If

            Return Ok(newComponents)
        End Function

        ' POST: api/NewComponents
        ''' <summary>
        ''' Alta de componentes custom
        ''' </summary>
        ''' <param name="newComponentModel">Modelo de componentes</param>
        ''' <returns></returns>
        <ResponseType(GetType(NewComponents))>
        <HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)>
        Async Function PostNewComponents(ByVal newComponentModel As NewComponentsBindingModel) As Task(Of IHttpActionResult)
            If Not ModelState.IsValid Then
                Return BadRequest(ModelState)
            End If

            Dim newComponent As New NewComponents With {
                .CurrntCost = newComponentModel.StndCost,
                .IdQuotation = newComponentModel.IdQuotation,
                .Uofm = newComponentModel.Uofm,
                .ItemDesc = newComponentModel.ItemDesc,
                .Result = newComponentModel.StndCost,
                .StndCost = newComponentModel.StndCost,
                .SkuComponente = newComponentModel.SkuComponente
            }

            db.NewComponents.Add(newComponent)
            Await db.SaveChangesAsync()
            Return CreatedAtRoute("DefaultApi", New With {.id = newComponent.Id}, newComponent)
        End Function

        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If (disposing) Then
                db.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub

        Private Function NewComponentsExists(ByVal id As Integer) As Boolean
            Return db.NewComponents.Count(Function(e) e.Id = id) > 0
        End Function
    End Class
End Namespace