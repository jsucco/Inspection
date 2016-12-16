﻿Imports System.Web.Security
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

            If IsNothing(ticket) = False Then
                If ticket.Name.Length > 0 And ticket.UserData = "APRSTCAUTHCOOKIE" Then
                    LoginLink.Text = "Welcome " + ticket.Name + " (Sign Out)"
                End If

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


    End Class

End Namespace