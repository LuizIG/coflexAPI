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
Imports Microsoft.AspNet.Identity

Namespace Controllers

    <Authorize>
    Public Class QuotationSummaryController
        Inherits System.Web.Http.ApiController

        Private db As New CoflexDBEntities1

        ' GET: api/QuotationSummary
        ''' <summary>
        ''' Obiene el resumen de las cotizaciones realizadas
        ''' </summary>
        ''' <returns></returns>
        <HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)>
        Function GetQuotationSummaryView() As IQueryable(Of QuotationSummaryView)
            Return db.QuotationSummaryView
        End Function

        ' GET: api/QuotationSummary/5
        ''' <summary>
        ''' Obiene el resumen de las cotizaciones realizadas por id
        ''' </summary>
        ''' <param name="id">Id de la cotizacion</param>
        ''' <returns></returns>
        <ResponseType(GetType(QuotationSummaryView))>
        <HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)>
        Async Function GetQuotationSummaryView(ByVal id As Integer) As Task(Of IHttpActionResult)
            Dim quotationSummaryView As QuotationSummaryView = Await db.QuotationSummaryView.FindAsync(id)
            If IsNothing(quotationSummaryView) Then
                Return NotFound()
            End If

            Return Ok(quotationSummaryView)
        End Function

        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If (disposing) Then
                db.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub

        Private Function QuotationSummaryViewExists(ByVal id As Integer) As Boolean
            Return db.QuotationSummaryView.Count(Function(e) e.Id = id) > 0
        End Function
    End Class
End Namespace