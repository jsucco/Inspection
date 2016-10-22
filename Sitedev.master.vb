Imports System.Web.Security
Imports System.Security.Principal
Imports System.Threading
Imports core.Environment


Namespace core



    Partial Class Site
        Inherits System.Web.UI.MasterPage

        Public Shared companyname As String


        Private Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init




        End Sub

        Private Sub on_load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MainContent.Load

            Dim load As String

            load = Convert.ToString(Me.MainContent.Page.GetType())

            Dim mitem As Menu
            Dim value As New List(Of MenuItem)
            Dim adminrole As MenuItem
            Dim path As String
            Dim sitepaths As String()
            Dim stringpath As String
            Dim count As Integer
            Dim CID As New CID


            path = HttpContext.Current.Request.Url.AbsolutePath
            sitepaths = path.Split(New Char() {"/"c})
            mitem = NavigationMenu

            For i = 0 To NavigationMenu.Items.Count() - 1
                value.Add(NavigationMenu.Items.Item(i))

            Next


            For Each adminrole In value.ToList()

                count = 0
                For Each stringpath In sitepaths
                    count = count + 1

                    If adminrole.Text = stringpath Then

                        GoTo 101

                    ElseIf adminrole.Text = "MAIN MENU" Then

                        GoTo 101
                    ElseIf adminrole.Text = "QUALITY ASSURANCE" And stringpath = "QA" Then
                        GoTo 101
                    ElseIf adminrole.Text = "COMMERCIAL WASHING" And stringpath = "COMMERCIAL%20WASHING" Then
                        GoTo 101
                    ElseIf count = sitepaths.Count Then
                        NavigationMenu.Items.Remove(adminrole)
                        GoTo 101
                    End If


                Next

101:
            Next


        End Sub


    End Class

End Namespace