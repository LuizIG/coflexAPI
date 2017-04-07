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

Namespace Controllers
    Public Class QuotationItemsComponentsController
        Inherits System.Web.Http.ApiController

        Private db As New ApplicationDbContext

        ' GET: api/QuotationItemsComponents
        Function GetItemsComponentsSet() As IQueryable(Of ItemsComponents)
            Return db.ItemsComponentsSet
        End Function

        ' GET: api/QuotationItemsComponents/5
        <ResponseType(GetType(ItemsComponents))>
        Async Function GetItemsComponents(ByVal id As Integer) As Task(Of IHttpActionResult)
            Dim itemsComponents As ItemsComponents = Await db.ItemsComponentsSet.FindAsync(id)
            If IsNothing(itemsComponents) Then
                Return NotFound()
            End If

            Return Ok(itemsComponents)
        End Function

        ' PUT: api/QuotationItemsComponents/5
        <ResponseType(GetType(Void))>
        Async Function PutItemsComponents(ByVal id As Integer, ByVal itemsComponents As ItemsComponents) As Task(Of IHttpActionResult)
            If Not ModelState.IsValid Then
                Return BadRequest(ModelState)
            End If

            If Not id = itemsComponents.Id Then
                Return BadRequest()
            End If

            db.Entry(itemsComponents).State = EntityState.Modified

            Try
                Await db.SaveChangesAsync()
            Catch ex As DbUpdateConcurrencyException
                If Not (ItemsComponentsExists(id)) Then
                    Return NotFound()
                Else
                    Throw
                End If
            End Try

            Return StatusCode(HttpStatusCode.NoContent)
        End Function

        ' POST: api/QuotationItemsComponents
        <ResponseType(GetType(ItemsComponents))>
        Async Function PostItemsComponents(ByVal itemsComponents As ItemsComponents) As Task(Of IHttpActionResult)
            If Not ModelState.IsValid Then
                Return BadRequest(ModelState)
            End If

            db.ItemsComponentsSet.Add(itemsComponents)
            Await db.SaveChangesAsync()

            Return CreatedAtRoute("DefaultApi", New With {.id = itemsComponents.Id}, itemsComponents)
        End Function

        ' DELETE: api/QuotationItemsComponents/5
        <ResponseType(GetType(ItemsComponents))>
        Async Function DeleteItemsComponents(ByVal id As Integer) As Task(Of IHttpActionResult)
            Dim itemsComponents As ItemsComponents = Await db.ItemsComponentsSet.FindAsync(id)
            If IsNothing(itemsComponents) Then
                Return NotFound()
            End If

            db.ItemsComponentsSet.Remove(itemsComponents)
            Await db.SaveChangesAsync()

            Return Ok(itemsComponents)
        End Function

        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If (disposing) Then
                db.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub

        Private Function ItemsComponentsExists(ByVal id As Integer) As Boolean
            Return db.ItemsComponentsSet.Count(Function(e) e.Id = id) > 0
        End Function
    End Class
End Namespace