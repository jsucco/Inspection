Imports core.Environment
Imports System.Web.Security
Imports System.Security.Principal
Imports System.Threading
Imports System.Reflection



Namespace core



    Partial Class login
        Inherits System.Web.UI.Page

        Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

            Dim istablet As Boolean = Convert.ToBoolean(Request.Browser("IsTablet"))




        End Sub

        Private util As New Utilities

        Protected Sub btnLogin_Click(sender As Object, e As System.EventArgs) Handles btnLogin.Click

            Dim userDAO As New userDAO
            Dim corp As New corporate
            Dim cidclass As New CID
            'Dim strURLCUSTOMER As String = Me.Session("UC")
            'Session.Abandon()
            If IsNothing(userid.Value) Or IsNothing(password.Value) Then
                lblError.Text = "User Name or Password left Blank"
            Else
                If userDAO.AutenticateUser(userid.Value, password.Value, "000578") = True Then
                    'Session("CID") = strURLCUSTOMER
                    Session("Username") = userid.Value
                    userDAO.LogUserActivity(userid.Value, "Mobile")
                    If Not userid.Value Is Nothing And RememberChk.Checked = True Then

                        FormsAuthentication.SetAuthCookie(userid.Value, True)

                        Thread.CurrentPrincipal = New GenericPrincipal(New GenericIdentity(userid.Value), {"ADMIN"})

                        Dim threadPrincipal As IPrincipal = Thread.CurrentPrincipal

                    ElseIf Not userid.Value Is Nothing And RememberChk.Checked = False Then
                        'UserObjects.IsAutho = True

                    Else
                        Response.Redirect("~/ErrorPage.aspx")
                    End If

                    Try
                        Dim MFBID As Integer = corp.SetMFBPermissions(userid.Value)
                        Session("MFBID") = MFBID
                    Catch ex As Exception
                        'Response.Redirect("~/ErrorPage.aspx")
                    End Try
                    SetUserCookies(userid.Value)

                    Response.Redirect("~/Mobile/APREntry.aspx")

                Else
                    lblError.Text = ("Incorrect UserID or Password")
                End If
            End If


        End Sub

        Private Sub SetUserCookies(ByVal Username As String)
            If Not Request.Cookies("APRUserName") Is Nothing Then
                Dim MFBIDCookie As String = Server.HtmlEncode(Request.Cookies("APRUserName")("Username")).ToString()

                If Username <> MFBIDCookie Then
                    Response.Cookies("APRUserName")("Username") = Username
                    Response.Cookies("APRUserName")("lastVisit") = DateTime.Now.ToString()
                    Response.Cookies("APRUserName").Expires = DateTime.Now.AddDays(365)
                End If

            Else
                Response.Cookies("APRUserName")("Username") = Username
                Response.Cookies("APRUserName")("lastVisit") = DateTime.Now.ToString()
                Response.Cookies("APRUserName").Expires = DateTime.Now.AddDays(365)
            End If
        End Sub

    End Class


End Namespace