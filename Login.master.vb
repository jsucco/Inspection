
Namespace core


    Partial Class Site
        Inherits System.Web.UI.MasterPage

        Private util As New Utilities

        Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

            Dim UserParse As Object = Request.UserAgent
            If util.DetectDeviceType(UserParse) = True Then
                Me.Session("APRISMOBILE") = "True"
                Dim CIDList As List(Of CID) = Me.Session("CID_Info")
                Dim CIDarray = CIDList.ToArray()

                'Response.Redirect("~/Mobile/Default.aspx?UC=" + CIDarray(0).CID.ToString())
            End If

        End Sub



    End Class

End Namespace