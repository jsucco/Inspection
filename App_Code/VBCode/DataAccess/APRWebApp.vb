Imports Microsoft.VisualBasic
Imports System.Reflection

Namespace core

    Public Class APRWebApp
        Inherits System.Web.UI.Page

        Public Property cypher As New cypher

        Sub Page_MacroLoad(sender As Object, e As EventArgs) Handles Me.Load

            SetUserProfileObjs()

            Dim path As String
            Dim uristirng As String
            path = HttpContext.Current.Request.Url.AbsolutePath
            Dim fullurl As String = HttpContext.Current.Request.Url.AbsoluteUri

            'If Not fullurl Is Nothing Then
            '    Dim fullurlarray = fullurl.Split("/")
            '    If fullurlarray.Length > 3 Then
            '        If fullurlarray(3).ToString() = "APRDEV" Then
            '            uristirng = fullurlarray(0).ToString() + "//" + fullurlarray(2).ToString() + "/" + fullurlarray(3).ToString()
            '            Session("BaseUri") = fullurlarray(0).ToString() + "//" + fullurlarray(2).ToString() + "/" + fullurlarray(3).ToString()
            '        Else
            '            uristirng = fullurlarray(0).ToString() + "//" + fullurlarray(2).ToString()
            '            Session("BaseUri") = fullurlarray(0).ToString() + "//" + fullurlarray(2).ToString()
            '        End If
            '    End If
            'End If

        End Sub

        Public Shared Function GetIPAddress() As String
            Dim context As System.Web.HttpContext = System.Web.HttpContext.Current
            Dim sIPAddress As String = context.Request.ServerVariables("HTTP_X_FORWARDED_FOR")
            If String.IsNullOrEmpty(sIPAddress) Then
                Return context.Request.ServerVariables("REMOTE_ADDR")
            Else
                Dim ipArray As String() = sIPAddress.Split(New [Char]() {","c})
                Return ipArray(0)
            End If
        End Function

        Private Sub SetUserProfileObjs()
            Dim UserProfileType As Type = GetType(UserProfile)
            Dim Fields = UserProfileType.GetFields(BindingFlags.Public Or BindingFlags.Instance)
            For Each field As FieldInfo In Fields
                Dim sessionvalue As Object = Me.Session(field.Name.ToString())
                Dim CookieSubKey As String = field.Name.ToString() + "_val"
                Dim CookieCheck As Dictionary(Of String, String) = GetCookie(field.Name.ToString(), CookieSubKey)
                If Not sessionvalue Is Nothing Then
                    SetCookie(field.Name.ToString(), CookieSubKey, sessionvalue)
                ElseIf CookieCheck.Count > 0 Then
                    Me.Session(field.Name.ToString) = CookieCheck.Item(field.Name.ToString())
                Else
                    'Response.Redirect("~/Login.aspx")
                End If

            Next

        End Sub

        Public Sub SetCookie(ByVal CookieName As String, ByVal SubKey As String, ByVal Value As Object)
            If Not Request.Cookies(CookieName) Is Nothing Then
                Try
                    Dim CurrentCookie As String = Server.HtmlEncode(Request.Cookies(CookieName)(SubKey)).ToString()
                    Dim valuestring As String = Convert.ToString(Value)
                    If valuestring <> CurrentCookie Then
                        Response.Cookies(CookieName)(SubKey) = Value
                        Response.Cookies(CookieName)("lastVisit") = DateTime.Now.ToString()
                        Response.Cookies(CookieName).Expires = DateTime.Now.AddDays(365)
                    End If
                Catch ex As Exception
                    Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
                End Try

            Else
                Response.Cookies(CookieName)(SubKey) = Value
                Response.Cookies(CookieName)("lastVisit") = DateTime.Now.ToString()
                Response.Cookies(CookieName).Expires = DateTime.Now.AddDays(365)
            End If

        End Sub

        Public Sub SetCookieExp(ByVal CookieName As String, ByVal SubKey As String, ByVal Value As Object, DaysExpired As Integer)
            If Not Request.Cookies(CookieName) Is Nothing Then
                Dim CurrentCookie As String = Server.HtmlEncode(Request.Cookies(CookieName)(SubKey)).ToString()

                If Value <> CurrentCookie Then
                    Response.Cookies(CookieName)(SubKey) = Value
                    Response.Cookies(CookieName)("lastVisit") = DateTime.Now.ToString()
                    Response.Cookies(CookieName).Expires = DateTime.Now.AddDays(DaysExpired)
                End If

            Else
                Response.Cookies(CookieName)(SubKey) = Value
                Response.Cookies(CookieName)("lastVisit") = DateTime.Now.ToString()
                Response.Cookies(CookieName).Expires = DateTime.Now.AddDays(DaysExpired)
            End If

        End Sub

        Public Function GetAPRKeepMeIn() As String
            For Each item In HttpContext.Current.Request.Cookies
                Dim cookie As HttpCookie = HttpContext.Current.Request.Cookies(item)
                If Not cookie Is Nothing And item.ToString().ToUpper().Trim = "APRKEEPMEIN" Then
                    If cookie.Item("CID_Print") Is Nothing Then
                        Continue For
                    End If
                    If cookie.Item("CID_Print").ToString().Trim().Length < 3 Then
                        Continue For
                    End If
                    Return cookie.Item("CID_Print").ToString().Trim()
                End If
            Next
        End Function

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
        Public Function GetCookie2(ByVal CookieName As String, ByVal SubKey As String) As Dictionary(Of String, String)
            Dim dictionary As New Dictionary(Of String, String)

            If Not HttpContext.Current Is Nothing Then
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
