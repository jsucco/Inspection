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
            Dim test As Single
            Dim test2 As Single

            chartdates = dtaker.getdatestable(1)
            maxDate = chartdates.Item2
            minDate = chartdates.Item1

            If dtaker.barchartsdrawcnt > dtaker.barchartsdrawcnt2 Then
                If TxtDateFrom.Date <> txtfromdate Then
                    TxtDateFrom.Date = txtfromdate
                End If
                If TxtDateTo.Date <> txttodate Then
                    TxtDateTo.Date = txttodate
                End If
                dtaker.barchartsdrawcnt2 += 1
            End If



        End Sub

        'Protected Sub BtnUpdateRange_Click(sender As Object, e As System.EventArgs) Handles BtnUpdateRange.Click
        '    Dim Xaxismin As Int64
        '    Dim Xaxismax As Int64
        '    Dim datefrom As Int64
        '    Dim selectdatefrom As DateTime
        '    Dim selectdateto As DateTime



        '    Xaxismin = C1WebChart1.Axis.X.Min
        '    Xaxismax = C1WebChart1.Axis.X.Max

        '    C1WebChart1.Axis.X.Min = Xaxismin + 1000000

        'End Sub

        Protected Sub Button1_Click(sender As Object, e As System.EventArgs) Handles Button1.Click
            Dim dtaker As New dtaker

            dtaker.barchartsdrawcnt += 1
            txtfromdate = TxtDateFrom.Date
            txttodate = TxtDateTo.Date
            Response.Redirect("~/UTILITIES/BarCharts.aspx")

        End Sub

        Protected Sub Button2_Click(sender As Object, e As System.EventArgs) Handles Button2.Click
            Response.Redirect("~/UTILITIES/Charts.aspx")
        End Sub
    End Class


End Namespace