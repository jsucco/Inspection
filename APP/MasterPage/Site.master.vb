Imports System.Web.Security
Imports System.Security.Principal
Imports System.Threading
Imports core.Environment
Imports System.Reflection
Imports System.Net.NetworkInformation


Namespace core

    Partial Class Site
        Inherits BaseMasterPage

        Public Property Util As New Utilities
        Public Shared companyname As String
        Public MenuFlag As Integer = 0

        Private Sub on_load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MainContent.Load
            Dim load As String
            load = Convert.ToString(Me.MainContent.Page.GetType())

            SetBaseUri()
            CheckCorporateName()
            SetSessionVariables()
            SetMenuText()

        End Sub
        Private Sub SetMenuText()
            Dim mitem As Menu
            Dim value As New List(Of MenuItem)
            Dim count As Integer
            Dim sitepaths As String()
            Dim stringpath As String
            Dim adminrole As MenuItem
            Dim path As String
            path = HttpContext.Current.Request.Url.AbsolutePath
            sitepaths = path.Split(New Char() {"/"c})

            MenuFlag = (From site In sitepaths Where site = "APR_SiteEntry.aspx" Select site).Count

            mitem = NavigationMenu

            For i = 0 To NavigationMenu.Items.Count() - 1
                value.Add(NavigationMenu.Items.Item(i))
            Next
            For Each adminrole In value.ToList()
                count = 0
                Dim sitepathcount As Integer = sitepaths.Length
                For Each stringpath In sitepaths
                    count = count + 1
                    If adminrole.Text = stringpath Then
                        GoTo 101
                    ElseIf adminrole.Text = "MAIN MENU" Then
                        '  If MenuFlag = 0 Then
                        GoTo 101
                        ' End If
                    ElseIf adminrole.Text = "QUALITY ASSURANCE" And stringpath = "QA" Then
                        GoTo 101
                    ElseIf adminrole.Text = "COMMERCIAL WASHING" And stringpath = "COMMERCIAL%20WASHING" Then
                        GoTo 101
                    ElseIf (adminrole.Text = "INSPECTION TOOL" And stringpath = "SPCinspectionInput.aspx") Or (adminrole.Text = "INSPECTION TOOL" And stringpath = "SPCInspectionUtility.aspx") Or (adminrole.Text = "INSPECTION TOOL" And stringpath = "SPCInspectionReporter.aspx") Then
                        GoTo 101
                    ElseIf count = sitepathcount Then
                        NavigationMenu.Items.Remove(adminrole)
                        GoTo 101
                    End If
                Next
                NavigationMenu.Items.Remove(adminrole)
101:
            Next
        End Sub
        Private Sub SetBaseUri()
            Dim fullurl As String = HttpContext.Current.Request.Url.AbsoluteUri
            If Not fullurl Is Nothing Then
                Dim fullurlarray = fullurl.Split("/")
                If fullurlarray.Length > 3 Then
                    If fullurlarray(3).ToString() = "APRDEV" Then
                        MasterSessionDAO.BaseUri = fullurlarray(0).ToString() + "//" + fullurlarray(2).ToString() + "/" + fullurlarray(3).ToString()
                    Else
                        MasterSessionDAO.BaseUri = fullurlarray(0).ToString() + "//" + fullurlarray(2).ToString()
                    End If
                End If
            End If
        End Sub
        Private Sub CheckCorporateName()
            CorporateName.Value = "GLOBAL STC"
            Dim QueryCID = Request.QueryString("CID")
            If IsNothing(QueryCID) = False Then
                SetCorporateName(QueryCID)
            Else
                Dim CookieCId = Request.Cookies("APRKeepMeIn")
                If IsNothing(CookieCId) = False Then
                    Try
                        SetCorporateName(CookieCId("CID_Print"))
                    Catch ex As Exception
                        Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
                    End Try
                End If
            End If
        End Sub
        Private Sub SetCorporateName(ByVal CID As String)
            If IsNothing(CID) = False And CID.Length > 0 Then
                Using _db As New AprManager_Entities
                    Dim CorpName = (From x In _db.LocationMasters Where x.CID = CID Select x).ToArray()
                    If IsNothing(CorpName) = False And CorpName.Length > 0 Then
                        CorporateName.Value = CorpName(0).Name.Trim()
                    End If
                End Using
            End If
        End Sub
        Private Sub SetSessionVariables()
            Dim MasterType As Type = GetType(MasterSessionDAO)
            Dim MasterFields = MasterType.GetFields(BindingFlags.Public Or BindingFlags.Static)
            Dim tst1 As Object
            For Each mfield As FieldInfo In MasterFields
                tst1 = mfield.GetValue(Nothing)
                If tst1 Is Nothing Then
                    Session(mfield.Name.ToString()) = "NOTSET"
                Else
                    Session(mfield.Name.ToString()) = Convert.ToString(tst1)
                End If
            Next
        End Sub
        Protected Sub SignOut_Click(sender As Object, e As EventArgs) Handles SignOut.Click
            Dim LocationCookie = GetCookie("APRKeepMeIn", "CID_Print")

            FormsAuthentication.SignOut()
            Session.Abandon()
            Response.Cookies("APRKeepMeIn").Expires = DateTime.Now.AddDays(-1)
            Dim newCookie As New HttpCookie(FormsAuthentication.FormsCookieName, "")
            newCookie.Expires = DateTime.Now.AddYears(-1)
            Response.Cookies.Add(newCookie)
            If LocationCookie.ContainsKey("APRKeepMeIn") Then
                Response.Redirect("~\Login.aspx?UC=" + LocationCookie("APRKeepMeIn"))
            Else
                Response.Redirect("~\Login.aspx")
            End If


        End Sub

        Public Function GetCookie(ByVal CookieName As String, ByVal SubKey As String) As Dictionary(Of String, String)
            Dim dictionary As New Dictionary(Of String, String)

            If Not HttpContext.Current Is Nothing And Not HttpContext.Current.Handler Is Nothing Then
                If Not HttpContext.Current.Request.Cookies(CookieName) Is Nothing Then
                    If Request.Cookies.AllKeys.Contains(CookieName) Then
                        Dim ThisCookie As String = Server.HtmlEncode(HttpContext.Current.Request.Cookies(CookieName)(SubKey)).ToString()
                        Dim lastVisitCookie As String = Server.HtmlEncode(HttpContext.Current.Request.Cookies(CookieName)("lastVisit")).ToString()
                        If ThisCookie <> "" Then
                            dictionary.Add(CookieName, ThisCookie)
                            dictionary.Add("lastVisit", lastVisitCookie)
                        End If
                    End If
                End If
            End If
            Return dictionary
        End Function

    End Class

End Namespace