<%@ WebHandler Language="VB" Class="UserLogin" %>

Imports System
Imports System.Web
Imports App.Utilities.Web.Handlers
Imports System.Web.Script.Serialization
Imports System.DirectoryServices.AccountManagement
Imports System.Web.Configuration

Public Class UserLogin
    Inherits BaseHandler

    Public Function Authenicate(User As String, Password As String) As String
        Dim Authenticated As String = "FALSE"
        User = AdjustUserNameForm(User)
        Using pc As New PrincipalContext(ContextType.Domain, "standardtextile.com", Nothing, ContextOptions.SimpleBind)
            If pc.ValidateCredentials(User, Password, ContextOptions.SimpleBind) Then
                Authenticated = "TRUE"
                'SetCookie("APR_UserActivityLog", "PrimaryKey", UDAO.LogUserActivity(usernamefrm, "WebApp_STC_ActiveDirectory"))
                'SetCookie("APR_Username", "Username", usernamefrm)

                'AddAuthCookie(username)
            End If
        End Using
        Return Authenticated
    End Function

    Private Function AdjustUserNameForm(ByVal username As String) As String
        Dim usersplit = username.Split("\")
        If usersplit.Length < 2 Then
            username = "textile\" + username
        Else
            username = username
        End If
        Return username
    End Function


End Class