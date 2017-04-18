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

Namespace Controllers
    Public Class QuotationVersionsController
        Inherits System.Web.Http.ApiController

        Private db As New CoflexDBEntities1

        ' GET: api/QuotationVersions
        Function GetQuotationVersions() As IQueryable(Of QuotationVersions)
            Return db.QuotationVersions
        End Function

        ' GET: api/QuotationVersions
        Function GetQuotationVersionsFiltrado(ByVal QuotationsId As Integer) As IQueryable(Of QuotationVersions)
            db.Configuration.LazyLoadingEnabled = False
            Return db.QuotationVersions.Where(Function(x) x.QuotationsId = QuotationsId)
        End Function

        ' GET: api/QuotationVersions/5
        <ResponseType(GetType(QuotationVersions))>
        Async Function GetQuotationVersions(ByVal id As Integer) As Task(Of IHttpActionResult)
            Dim quotationVersions As QuotationVersions = Await db.QuotationVersions.FindAsync(id)
            If IsNothing(quotationVersions) Then
                Return NotFound()
            End If

            Return Ok(quotationVersions)
        End Function

        ' PUT: api/QuotationVersions/5
        <ResponseType(GetType(Void))>
        Async Function PutQuotationVersions(ByVal id As Integer, ByVal quotationVersions As QuotationVersionIdBindingModel) As Task(Of IHttpActionResult)
            If Not ModelState.IsValid Then
                Return BadRequest(ModelState)
            End If

            Dim qVersion As QuotationVersions = Await db.QuotationVersions.FindAsync(id)


            'Borra los items anteriores :v
            For Each item In qVersion.Items
                db.Items.Remove(item)
                Await db.SaveChangesAsync()
            Next

            qVersion.Date = DateTime.Now
            qVersion.ExchangeRate = quotationVersions.ExchangeRate

            Dim modelItems = quotationVersions.ItemsBindingModel
            Dim listItems As New List(Of Items)()
            Dim x As Integer = 0
            For Each item In modelItems

                Dim itemsComponests As ICollection(Of ItemsComponents)
                Dim listItemComponents As New List(Of ItemsComponents)()
                Dim modelItemComponets = item.ItemsComponents

                For Each itemComponent In modelItemComponets
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
                        .StndCost = itemComponent.StndCost
                    })
                Next
                itemsComponests = listItemComponents


                db.Items.Add(New Items With {
                    .QuotationVersionsId = id,
                    .ItemDescription = item.ItemDescription,
                    .ItemNumber = x + 1,
                    .Quantity = item.Quantity,
                    .Sku = item.Sku,
                    .UM = item.UM,
                    .Status = item.Status,
                    .ItemsComponents = itemsComponests
                })
                x = x + 1
            Next

            db.Entry(qVersion).State = EntityState.Modified

            Try
                Await db.SaveChangesAsync()
            Catch ex As DbUpdateConcurrencyException
                If Not (QuotationVersionsExists(id)) Then
                    Return NotFound()
                Else
                    Throw
                End If
            End Try

            Return StatusCode(HttpStatusCode.NoContent)
        End Function

        ' POST: api/QuotationVersions
        <ResponseType(GetType(QuotationVersions))>
        Async Function PostQuotationVersions(ByVal quotationVersionsModel As QuotationVersionIdBindingModel) As Task(Of IHttpActionResult)
            If Not ModelState.IsValid Then
                Return BadRequest(ModelState)
            End If

            Dim items As ICollection(Of Items)
            Dim modelItems = quotationVersionsModel.ItemsBindingModel
            Dim listItems As New List(Of Items)()
            Dim x As Integer = 0
            For Each item In modelItems

                Dim itemsComponests As ICollection(Of ItemsComponents)
                Dim listItemComponents As New List(Of ItemsComponents)()
                Dim modelItemComponets = item.ItemsComponents

                For Each itemComponent In modelItemComponets
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
                        .StndCost = itemComponent.StndCost
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
                    .ItemsComponents = itemsComponests
                })
                x = x + 1
            Next
            items = listItems
            Dim count = db.QuotationVersions.Where(Function(QVERSION) QVERSION.QuotationsId = quotationVersionsModel.IdQuotaions).Count + 1
            Dim qv As New QuotationVersions With {
                .Date = DateTime.Now,
                .Status = 0,
                .ExchangeRate = quotationVersionsModel.ExchangeRate,
                .UseStndCost = quotationVersionsModel.UseStndCost,
                .VersionNumber = count,
                .Items = items,
                .QuotationsId = quotationVersionsModel.IdQuotaions
            }


            db.QuotationVersions.Add(qv)
            Await db.SaveChangesAsync()

            Return CreatedAtRoute("DefaultApi", New With {.id = qv.Id}, qv)
        End Function

        ' DELETE: api/QuotationVersions/5
        <ResponseType(GetType(QuotationVersions))>
        Async Function DeleteQuotationVersions(ByVal id As Integer) As Task(Of IHttpActionResult)
            Dim quotationVersions As QuotationVersions = Await db.QuotationVersions.FindAsync(id)
            If IsNothing(quotationVersions) Then
                Return NotFound()
            End If

            db.QuotationVersions.Remove(quotationVersions)
            Await db.SaveChangesAsync()

            Return Ok(quotationVersions)
        End Function

        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If (disposing) Then
                db.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub

        Private Function QuotationVersionsExists(ByVal id As Integer) As Boolean
            Return db.QuotationVersions.Count(Function(e) e.Id = id) > 0
        End Function
    End Class
End Namespace