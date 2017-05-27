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
Imports CoflexAPI.QuotationsBindingModel
Imports Microsoft.AspNet.Identity
Imports Microsoft.AspNet.Identity.EntityFramework
Imports Microsoft.Owin.Security

Namespace Controllers
    Public Class QuotationsController
        Inherits System.Web.Http.ApiController

        Private db As New CoflexDBEntities1

        ' GET: api/Quotations
        <Authorize>
        Function GetQuotations() As IQueryable(Of Quotations)
            Return db.Quotations.OrderBy(Function(x) x.Status).ThenByDescending(Function(x) x.Date)
        End Function

        ' GET: api/Quotations/5
        <Authorize>
        <ResponseType(GetType(Quotations))>
        Async Function GetQuotations(ByVal id As Integer) As Task(Of IHttpActionResult)
            db.Configuration.LazyLoadingEnabled = False
            Dim quotations As Quotations = Await db.Quotations.FindAsync(id)
            If IsNothing(quotations) Then
                Return NotFound()
            End If

            Return Ok(quotations)
        End Function

        ' PUT: api/Quotations/5
        <Authorize>
        <ResponseType(GetType(Void))>
        Async Function PutQuotations(ByVal id As Integer, ByVal quotations As Quotations) As Task(Of IHttpActionResult)
            If Not ModelState.IsValid Then
                Return BadRequest(ModelState)
            End If

            If Not id = quotations.Id Then
                Return BadRequest()
            End If

            db.Entry(quotations).State = EntityState.Modified

            Try
                Await db.SaveChangesAsync()
            Catch ex As DbUpdateConcurrencyException
                If Not (QuotationsExists(id)) Then
                    Return NotFound()
                Else
                    Throw
                End If
            End Try

            Return StatusCode(HttpStatusCode.NoContent)
        End Function

        ' PUT: api/Quotations/5
        <Authorize>
        <ResponseType(GetType(Void))>
        Async Function PutQuotations(ByVal id As Integer, ByVal UserId As String) As Task(Of IHttpActionResult)
            If Not ModelState.IsValid Then
                Return BadRequest(ModelState)
            End If

            Dim Quotation As Quotations = Await db.Quotations.FindAsync(id)
            Quotation.AspNetUsersId = UserId

            db.Entry(Quotation).State = EntityState.Modified

            Try
                Await db.SaveChangesAsync()
            Catch ex As DbUpdateConcurrencyException
                If Not (QuotationsExists(id)) Then
                    Return NotFound()
                Else
                    Throw
                End If
            End Try

            Return StatusCode(HttpStatusCode.NoContent)
        End Function

        ' POST: api/Quotations
        <Authorize>
        <ResponseType(GetType(Quotations))>
        Async Function PostQuotations(ByVal model As QuotationBindingModel) As Task(Of IHttpActionResult)
            If Not ModelState.IsValid Then
                'Return BadRequest(ModelState)
            End If

            Dim idUser As String = HttpContext.Current.User.Identity.GetUserId()

            Dim items As ICollection(Of Items)
            Dim modelItems = model.QuotationVersions().ItemsBindingModel
            Dim listItems As New List(Of Items)()
            Dim x As Integer = 0
            For Each item In modelItems

                Dim itemsComponests As ICollection(Of ItemsComponents)
                Dim listItemComponents As New List(Of ItemsComponents)()
                Dim modelItemComponets = item.ItemsComponents

                For Each itemComponent In modelItemComponets

                    If itemComponent.FinalCost = Nothing Then
                        itemComponent.FinalCost = 0.0
                    End If

                    If itemComponent.RBCost = Nothing Then
                        itemComponent.RBCost = 0
                    End If

                    If itemComponent.RACost = Nothing Then
                        itemComponent.RACost = 0
                    End If

                    If itemComponent.Shipping = Nothing Then
                        itemComponent.Shipping = 0
                    End If

                    listItemComponents.Add(New ItemsComponents With {
                        .ItemDescription = itemComponent.ItemDescription,
                        .CurrCost = itemComponent.CurrCost,
                        .Lvl1 = itemComponent.Lvl1,
                        .Lvl2 = itemComponent.Lvl2,
                        .Lvl3 = itemComponent.Lvl3,
                        .Quantity = itemComponent.Quantity,
                        .Result = itemComponent.Result,
                        .SkuComponent = itemComponent.SkuComponent,
                        .UM = itemComponent.UM,
                        .StndCost = itemComponent.StndCost,
                        .FinalCost = itemComponent.FinalCost,
                        .RACost = itemComponent.RACost,
                        .RBCost = itemComponent.RBCost,
                        .Shipping = itemComponent.Shipping
                    })
                Next
                itemsComponests = listItemComponents

                listItems.Add(New Items With {
                    .ItemDescription = item.ItemDescription,
                    .ItemNumber = x + 1,
                    .Quantity = item.Quantity,
                    .Sku = item.Sku,
                    .UM = item.UM,
                    .Status = item.Status,
                    .ItemsComponents = itemsComponests,
                    .ProfitMargin = item.ProfitMargin
                })
                x = x + 1
            Next
            items = listItems

            Dim quotationVersion As ICollection(Of QuotationVersions)
            Dim qv As New QuotationVersions With {
                .Date = DateTime.Now,
                .Status = 0,
                .LastModificationDate = Date.Now,
                .ExchangeRate = model.QuotationVersions.ExchangeRate,
                .UseStndCost = model.QuotationVersions.UseStndCost,
                .VersionNumber = 1,
                .Items = items
            }


            Dim list As New List(Of QuotationVersions)()
            list.Add(qv)
            quotationVersion = list

            Dim count = db.Quotations.Where(Function(y) y.ClientId = model.ClientId).Count + 1
            Dim CoflexId = model.ClientName.Substring(0, 3) & "-" & Format(count, "#000")

            Dim q As New Quotations With {
                .Date = DateTime.Now,
                .ClientId = model.ClientId,
                .ClientName = model.ClientName,
                .Status = 0,
                .AspNetUsersId = idUser,
                .QuotationVersions = quotationVersion,
                .CoflexId = CoflexId,
                .ProspectId = model.ProspectId
            }
            db.Quotations.Add(q)
            Await db.SaveChangesAsync()

            Return CreatedAtRoute("DefaultApi", New With {.id = q.Id}, q)
        End Function

        ' DELETE: api/Quotations/5
        <ResponseType(GetType(Quotations))>
        Async Function DeleteQuotations(ByVal id As Integer) As Task(Of IHttpActionResult)
            Dim quotations As Quotations = Await db.Quotations.FindAsync(id)
            If IsNothing(quotations) Then
                Return NotFound()
            End If

            db.Quotations.Remove(quotations)
            Await db.SaveChangesAsync()

            Return Ok(quotations)
        End Function

        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If (disposing) Then
                db.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub

        Private Function QuotationsExists(ByVal id As Integer) As Boolean
            Return db.Quotations.Count(Function(e) e.Id = id) > 0
        End Function
    End Class
End Namespace