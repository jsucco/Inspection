Imports core.Environment

Namespace core



    Partial Class Mobile_Default
        Inherits System.Web.UI.Page


        Private Sub page_load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

            Dim CSCode As CSExample = New CSExample()
            Dim x As Integer = Request.Browser.ScreenPixelsWidth - 0.1 * Request.Browser.ScreenPixelsWidth
            Dim y As Integer = Request.Browser.ScreenPixelsHeight - 0.1 * Request.Browser.ScreenPixelsHeight




            mobilelabel.Text = CSCode.TeamString.ToString() + " Screen Pixel Width: " + Convert.ToString(x) + " Screen Pixel Height: " + Convert.ToString(y) + ""

            Dim Env As Environment = New Environment(Context)
            Dim corp As corporate = New corporate
            Dim strURLCustomer As String
            If Env.AppIsAvailable Then
                strURLCustomer = "000578"    'Me.Request.QueryString("UC")
                If Not strURLCustomer Is Nothing Then
                    Me.Session("CID") = strURLCustomer
                    If corp.GetDBuser(strURLCustomer) = True Then
                        Session("CID_Info") = corp.cidclass
                    End If
                    Response.Redirect("~/Mobile/Login.aspx")
                End If
                'If application is available, sent to application home page.  Note that any page in the application directory is secured by .net (see web.config in app directory), therefore .Net will provide login screen.
            End If
            'Application not available
            Response.Redirect("~/ErrorPage.aspx")




        End Sub


    End Class



End Namespace