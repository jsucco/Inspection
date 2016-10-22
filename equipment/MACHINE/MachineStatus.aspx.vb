
Imports System.Data.SqlClient
Imports System.Web.Security
Imports System.Security.Principal
Imports System.Threading


Namespace core


    Partial Class EQUIPMENT_COMMERCIAL_WASHING_commwash
        Inherits System.Web.UI.Page


        Public Shared mgroups As New mgroups


        Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

            Dim threadPrincipal As IPrincipal = Thread.CurrentPrincipal
            Dim IsAutho As String = threadPrincipal.Identity.IsAuthenticated
            Dim UserName As String = Context.User.Identity.Name
            Dim imageid As Integer
            machinetitle_label.Text = mgroups.machineselector

            imageid = mgroups.Machineselectorid
            Image1.ImageUrl = "ImageVB.aspx?ImageID=" + imageid.ToString()

        End Sub

        Protected Sub commwash_training_Click(sender As Object, e As System.EventArgs) Handles commwash_training.Click
            Response.Redirect("~/Training/TrainingVideo.aspx")
        End Sub
    End Class


End Namespace
