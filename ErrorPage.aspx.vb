
Partial Class ErrorPage
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Try
            Dim UC As String = Request.QueryString("UC")
            If UC.Trim.Length > 0 Then
                Label1.Text = UC
            End If
        Catch ex As Exception
            Label1.Text = "It Didnt Work"
        End Try
    End Sub
End Class
