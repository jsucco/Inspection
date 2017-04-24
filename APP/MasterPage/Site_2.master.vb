Imports System.Web.Security
Imports System.Security.Principal
Imports System.Threading
Imports core.Environment
Imports System.Reflection
Imports System.Net.NetworkInformation
Imports System.DirectoryServices.AccountManagement

Namespace core



    Partial Class Site
        Inherits BaseMasterPage

        Public Property Util As New Utilities
        Public Shared companyname As String
        Public Shared Username As String

        Private Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init




        End Sub

        Private Sub on_load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MainContent.Load
            'Dim load As String
            'load = Convert.ToString(Me.MainContent.Page.GetType())
            'Dim value As New List(Of MenuItem)
            Dim path As String
            Dim sitepaths As String()
            'Dim CID As New CID

            path = HttpContext.Current.Request.Url.AbsolutePath
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
            sitepaths = path.Split(New Char() {"/"c})
            SetSessionVariables()

            Dim authCookie = Request.Cookies(FormsAuthentication.FormsCookieName)
            Dim ticket As FormsAuthenticationTicket
            If Not authCookie Is Nothing Then
                ticket = FormsAuthentication.Decrypt(authCookie.Value)
            End If

            If IsNothing(ticket) = True Then
                SignOut.Text = "Log In"
            End If
            'Dim upUser As UserPrincipal = UserPrincipal.FindByIdentity(context)



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
            Response.Redirect("~\Login.aspx?UC=" + LocationCookie("APRKeepMeIn"))

        End Sub

        Private Function GetCookie(ByVal CookieName As String, ByVal SubKey As String) As Dictionary(Of String, String)
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