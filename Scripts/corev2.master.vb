
Partial Class Site
    Inherits System.Web.UI.MasterPage

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

        '        Dim pathcount As Integer
        '        pathcount = sitepaths.Count()

        '        If pathcount = 0 Then GoTo 102
        '        If sitepaths(pathcount - 1) = "default.aspx" Or sitepaths(pathcount - 1) = "Default.aspx" Then
        '            title_label.Text = ""
        '            GoTo 102
        '        ElseIf sitepaths(pathcount - 2) = "QA" Then
        '            title_label.Text = "QUALITY ASSURANCE"
        '        ElseIf sitepaths(pathcount - 2) = "COMMERCIAL%20WASHING" Then
        '            title_label.Text = "COMMERCIAL WASHING"
        '        Else
        '            title_label.Text = sitepaths(pathcount - 2)
        '        End If



        '102:
    End Sub

    
End Class

