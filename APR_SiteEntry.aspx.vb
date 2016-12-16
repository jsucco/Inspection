
Partial Class APR_SiteEntry
    Inherits System.Web.UI.Page

    Public Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Response.Redirect("~/APP/APR_SiteEntry.aspx")
    End Sub

End Class
