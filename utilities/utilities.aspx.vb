Imports System.Web.Security
Imports System.Security.Principal
Imports System.Threading


Namespace core



    Partial Class UTILITIES_utilities
        Inherits System.Web.UI.Page

        Protected Sub water_but_Click(sender As Object, e As System.EventArgs) Handles water_but.Click

            Dim dtaker As New dtaker

            'dtaker.chartselector = "mainwater"
            dtaker.chartsdrawcnt = 0
            dtaker.chartsdrawcnt2 = 0
            dtaker.barchartsdrawcnt = 0
            dtaker.barchartsdrawcnt2 = 0
            Response.Redirect("~/UTILITIES/Charts.aspx")
        End Sub

        Protected Sub elec_but_Click(sender As Object, e As System.EventArgs) Handles elec_but.Click
            Dim dtaker As New dtaker

            'dtaker.chartselector = "electric"
            dtaker.melecdrawcnt = 0
            dtaker.melecdrawcnt2 = 0
            Response.Redirect("~/UTILITIES/elecchart.aspx")
        End Sub

        Protected Sub gas_BUT_Click(sender As Object, e As System.EventArgs) Handles gas_BUT.Click
            Dim dtaker As New dtaker

            'dtaker.chartselector = "maingas"
            dtaker.mgasdrawcnt = 0
            dtaker.mgasdrawcnt2 = 0
            Response.Redirect("~/UTILITIES/gaschart.aspx")
        End Sub

        Protected Sub spec_chart_Click(sender As Object, e As System.EventArgs) Handles spec_chart.Click
            Dim dtaker As New dtaker

            dtaker.chartselector = 0
            dtaker.schselector = 1
            dtaker.specdrawcnt = 0
            dtaker.specdrawcnt2 = 0
            Response.Redirect("~/UTILITIES/chartpage.aspx")
        End Sub

        Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load



            Dim threadPrincipal As IPrincipal = Thread.CurrentPrincipal
            Dim IsAutho As String = threadPrincipal.Identity.IsAuthenticated
            Dim UserName As String = Context.User.Identity.Name






        End Sub
    End Class



End Namespace