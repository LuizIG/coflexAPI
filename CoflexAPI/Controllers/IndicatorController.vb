Imports System.Data.SqlClient
Imports System.Net
Imports System.Web.Http
Imports System.Web.Http.Description

Namespace Controllers
    Public Class IndicatorController
        Inherits ApiController
        Private db As New CoflexDBEntities1

        <Authorize>
        Public Function getIndicator(ByVal min As String, ByVal max As String, Optional ByVal estatus As String = "", Optional ByVal estatusV As String = "", Optional ByVal vendedor As String = "", Optional ByVal cliente As String = "") As IEnumerable(Of Indicator_Result)

            Dim empDetails As IEnumerable(Of Indicator_Result) =
                db.Database.SqlQuery(Of Indicator_Result)("exec QuotationsIndicatorProcedure @mindate, @maxdate, @estatus ,@estatusV ,@vendedor, @cliente",
    New SqlParameter("mindate", min),
    New SqlParameter("maxdate", max),
    New SqlParameter("estatus", estatus),
    New SqlParameter("estatusV", estatusV),
    New SqlParameter("vendedor", vendedor),
    New SqlParameter("cliente", cliente)).ToList()
            Return empDetails
        End Function

    End Class
End Namespace