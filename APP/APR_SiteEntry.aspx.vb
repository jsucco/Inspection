Imports System.Web.Script.Serialization

Namespace core

    Partial Class APP_Menu_APR_SiteEntry
        Inherits core.APRWebApp

        Public CID As String
        Public UserID As String
        Public NavPerms As String = "[0]"
        Private CIDclass As New CID
        Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
            Dim UserParse As Object = Request.UserAgent
            Dim jser As New JavaScriptSerializer

            If IsNothing(Me.Session("APRISMOBILE")) = False Then
                If Me.Session("APRISMOBILE") = "True" Then
                    Response.Redirect("~/Mobile/APREntry.aspx")
                End If
            End If
            If Page.IsPostBack = False Then
                CID = Request.QueryString("CID")
                UserID = Request.QueryString("UserID")
                If CID = "" And UserID = "" Then
                    Dim corplist As New List(Of CID)
                    If IsNothing(Me.Session("CID_Info")) = False Then
                        corplist = Me.Session("CID_Info")
                        Dim corparray = corplist.ToArray()
                        CID = corparray(0).CID_Print
                        UserID = corparray(0).UserID
                        Response.Redirect("~/APP/APR_SiteEntry.aspx?CID=" + corparray(0).CID_Print + "&UserID=" + corparray(0).UserID)
                        'Request.QueryString("CID") = corparray(0).CID_Print
                        'Request.QueryString("UserID") = corparray(0).UserID
                    End If
                End If

                Dim bmap_nav As New BMappers(Of NavigationPermissions)
                Dim nplist As New List(Of NavigationPermissions)

                nplist = bmap_nav.GetCtxMangObject("SELECT APRPM_Enabled, APRLoom_Enabled, APRUtility_Enabled, APRInspection_Enabled, APRSPC_Enabled FROM  Corporate WHERE (CID = N'" & CID & "')")
                NavPerms = jser.Serialize(nplist)
            End If
        End Sub
    End Class

End Namespace
