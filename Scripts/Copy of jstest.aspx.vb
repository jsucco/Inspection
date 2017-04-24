
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Data.SqlClient
Imports System.Text
Imports System.Threading.Tasks
Imports System.Web.Script.Serialization

Namespace core


    Partial Class UTILITIES_chartmaster
        Inherits System.Web.UI.Page

        Public Shared maxDate As DateTime
        Public Shared minDate As DateTime

        Sub Page_load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

            Dim dtaker = New dtaker
            Dim chartdates As Tuple(Of DateTime, DateTime)


            chartdates = dtaker.getdatestable(1)
            maxDate = chartdates.Item2
            minDate = chartdates.Item1

            'For Each item In chartdates

            '    Page.ClientScript.RegisterArrayDeclaration("dates", chartdates(i))
            '    i += 1
            'Next
            'Dim serializer As New JavaScriptSerializer()
            'Dim serialres As String = serializer.Serialize(chartdates)

            'Page.ClientScript.RegisterArrayDeclaration("dates", serialres)

        End Sub


    End Class


End Namespace