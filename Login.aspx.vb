Imports core.Environment
Imports System.Web.Security
Imports System.Security.Principal
Imports System.Threading
Imports System.Reflection
Imports System.DirectoryServices.AccountManagement
Imports System.Web.Configuration

Partial Class LoginTest
    Inherits System.Web.UI.Page
    Public credsError = "false"
    Private Sub on_load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


    End Sub
    Sub OnAuthenticate(ByVal sender As Object, ByVal e As EventArgs) Handles btnSubmit.Click
        Dim Authenticated As Boolean
        Dim User = AdjustUserNameForm(txtUserName.Value)
        If User.Length = 0 Or txtPassword.Value.Length = 0 Then
            Exit Sub
        End If
        Using pc As New PrincipalContext(ContextType.Domain, "standardtextile.com", Nothing, ContextOptions.Negotiate)
            Dim perm As String = pc.ValidateCredentials(User, txtPassword.Value, ContextOptions.SimpleBind).ToString()

            If pc.ValidateCredentials(User, txtPassword.Value, ContextOptions.SimpleBind) Then
                AddAuthCookie(User)
                AddKeyForChildApps(User)
                Response.Redirect("~/APP/APR_SiteEntry.aspx?perm=" + perm)
            Else
                Dim UDAO As New core.userDAO
                If UDAO.AutenticateUser(txtUserName.Value, txtPassword.Value, 578) = True Then
                    SetCookie("APR_UserActivityLog", "PrimaryKey", UDAO.LogUserActivity(User, "WebApp_CTX", 578))
                    SetCookie("APR_Username", "Username", User)
                    AddAuthCookie(User)
                    AddKeyForChildApps(User)
                    Response.Redirect("~/APP/APR_SiteEntry.aspx")
                End If
            End If
            credsError = "true"
        End Using


    End Sub
    Private Function AdjustUserNameForm(ByVal username As String) As String
        Dim usersplit = username.Split("\")
        If usersplit.Length < 2 Then
            username = "textile\" + username
        Else
            username = username
        End If
        Return username
    End Function
    Public Sub SetCookie(ByVal CookieName As String, ByVal SubKey As String, ByVal Value As Object)
        If Not Request.Cookies(CookieName) Is Nothing Then
            Dim CurrentCookie As String = Server.HtmlEncode(Request.Cookies(CookieName)(SubKey)).ToString()

            If Value <> CurrentCookie Then
                Response.Cookies(CookieName)(SubKey) = Value
                Response.Cookies(CookieName)("lastVisit") = DateTime.Now.ToString()
                Response.Cookies(CookieName).Expires = DateTime.Now.AddDays(365)
            End If

        Else
            Response.Cookies(CookieName)(SubKey) = Value
            Response.Cookies(CookieName)("lastVisit") = DateTime.Now.ToString()
            Response.Cookies(CookieName).Expires = DateTime.Now.AddDays(365)
        End If

    End Sub
    Private Sub AddAuthCookie(ByVal username As String)
        FormsAuthentication.Initialize()
        Dim expires As DateTime = DateTime.Now.AddDays(365)
        Dim ticket As FormsAuthenticationTicket = New FormsAuthenticationTicket(1,
                                username,
                                DateTime.Now,
                                expires,
                                True,
                                "APRSTCAUTHCOOKIE",
                                FormsAuthentication.FormsCookiePath)
        Dim encryptedTicket = FormsAuthentication.Encrypt(ticket)
        Dim authCookie As HttpCookie = New HttpCookie(
                                               FormsAuthentication.FormsCookieName,
                                               encryptedTicket)
        authCookie.Expires = expires

        Response.Cookies.Add(authCookie)
    End Sub
    Private Sub AddKeyForChildApps(ByVal user As String)
        Dim KeepMeIn As HttpCookie = Request.Cookies("APRKeepMeIn")
        Dim cypher As New core.cypher
        Dim encryptKey = System.Web.Configuration.WebConfigurationManager.AppSettings("key64")
        Dim aprkey = System.Web.Configuration.WebConfigurationManager.AppSettings("AprKey")
        Dim QueryCID = Me.Request.QueryString("CID")
        If Not aprkey Is Nothing Then
            Response.Cookies("APRKeepMeIn")("MachineKey") = cypher.MD5Encrypt(aprkey, encryptKey)
            Response.Cookies("APRKeepMeIn")("IPAddress") = GetIP()
            Response.Cookies("APRKeepMeIn")("CID_Print") = getCID()
            Response.Cookies("APRKeepMeIn")("lastVisit") = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString()
            Response.Cookies("APRKeepMeIn")("User") = user
            Response.Cookies("APRKeepMeIn").Expires = DateTime.Now.AddYears(10)
        End If
    End Sub
    Private Function GetIP() As String
        Dim KeepMeIn As HttpCookie = Request.Cookies("APRKeepMeIn")
        If Not KeepMeIn Is Nothing Then
            If Not KeepMeIn("IPAddress") Is Nothing Then
                Return KeepMeIn("IPAddress")
                Exit Function
            End If
        End If
        Return core.APRWebApp.GetIPAddress()

    End Function
    Private Function getCID() As String
        Dim KeepMeIn As HttpCookie = Request.Cookies("APRKeepMeIn")
        Dim CID As String = ""
        Try
            If Not KeepMeIn Is Nothing Then
                If KeepMeIn("CID_Print").Length > 0 Then
                    Return KeepMeIn("CID_Print")
                    Exit Function
                End If
            End If
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
        End Try

        Dim LastCID As HttpCookie = Request.Cookies("CID")
        Try
            If Not LastCID Is Nothing Then
                If LastCID("CID_val").Length > 0 Then
                    Return LastCID("CID_val")
                    Exit Function
                End If
            End If
        Catch ex As Exception

        End Try

    End Function
End Class
