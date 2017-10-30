Imports System.Web.Security
Imports System.Security.Principal
Imports System.Threading

Namespace core



    Partial Class UTILITIES_Charts
        Inherits System.Web.UI.Page


        Public Shared maxDate As DateTime
        Public Shared minDate As DateTime

        Public Shared txtfromdate As DateTime = "01/25/2014 00:00:00"
        Public Shared txttodate As DateTime = "01/28/2014 00:00:00"


        Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

            Dim threadPrincipal As IPrincipal = Thread.CurrentPrincipal
            Dim IsAutho As String = threadPrincipal.Identity.IsAuthenticated
            Dim UserName As String = Context.User.Identity.Name
            Dim dtaker = New dtaker
            Dim chartdates As Tuple(Of DateTime, DateTime)

            chartdates = dtaker.getdatestable(1)
            maxDate = chartdates.Item2
            minDate = chartdates.Item1

            If dtaker.chartsdrawcnt > dtaker.chartsdrawcnt2 Then
                If TxtDateFrom.Date <> txtfromdate Then
                    TxtDateFrom.Date = txtfromdate
                End If
                If TxtDateTo.Date <> txttodate Then
                    TxtDateTo.Date = txttodate
                End If
                dtaker.chartsdrawcnt2 += 1
            End If



        End Sub


        Protected Sub Button1_Click(sender As Object, e As System.EventArgs) Handles Button1.Click
            Dim dtaker As New dtaker

            dtaker.chartsdrawcnt += 1
            txtfromdate = TxtDateFrom.Date
            txttodate = TxtDateTo.Date
            Response.Redirect("~/UTILITIES/Charts.aspx")

        End Sub

        Protected Sub Button2_Click(sender As Object, e As System.EventArgs) Handles Button2.Click
            Response.Redirect("~/UTILITIES/BarCharts.aspx")
        End Sub
    End Class


End Namespace