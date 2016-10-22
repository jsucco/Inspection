Imports System.Web.Security
Imports System.Security.Principal
Imports System.Threading

Namespace core

    Partial Class UTILITIES_elecchart
        Inherits System.Web.UI.Page

        Public Shared txtfromdate As DateTime = "02/04/2014 00:00:00"
        Public Shared txttodate As DateTime = "02/09/2014 00:00:00"

        Protected Sub Button1_Click(sender As Object, e As System.EventArgs) Handles Button1.Click
            Dim dtaker As New dtaker

            dtaker.melecdrawcnt += 1
            txtfromdate = TxtDateFrom.Date
            txttodate = TxtDateTo.Date
            Response.Redirect("~/UTILITIES/gaschart.aspx")
        End Sub

        Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

            Dim threadPrincipal As IPrincipal = Thread.CurrentPrincipal
            Dim IsAutho As String = threadPrincipal.Identity.IsAuthenticated
            Dim UserName As String = Context.User.Identity.Name
            Dim dtaker = New dtaker


            If dtaker.melecdrawcnt > dtaker.melecdrawcnt2 Then
                If TxtDateFrom.Date <> txtfromdate Then
                    TxtDateFrom.Date = txtfromdate
                End If
                If TxtDateTo.Date <> txttodate Then
                    TxtDateTo.Date = txttodate
                End If
                dtaker.melecdrawcnt2 += 1
            End If
        End Sub
    End Class

End Namespace