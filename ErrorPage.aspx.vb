
Partial Class ErrorPage
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Try
            Dim UC As String = Session("UC")
            Label1.Text = UC
        Catch ex As Exception
            Label1.Text = "It Didnt Work"
        End Try
    End Sub
End Class
