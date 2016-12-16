Imports core.Environment
Imports System.DirectoryServices.AccountManagement

Namespace core

    Public Class _Default
        Inherits System.Web.UI.Page

        Public Property Util As New Utilities

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            Dim Env As Environment = New Environment(Context)
            Dim corp As corporate = New corporate
            Dim strURLCustomer As String = Nothing

            If Env.AppIsAvailable Then
                strURLCustomer = "000114" ' Me.Request.QueryString("UC")

                If Not strURLCustomer Is Nothing Then
                    Me.Session("CID") = strURLCustomer

                    If corp.IsAprContext(strURLCustomer) = True Then
                        Session("CID_Info") = corp.cidclass
                        Dim CIDArray = corp.cidclass.ToArray()
                        Try
                            Response.Cookies("APRKeepMeIn")("IPAddress") = CIDArray(0).IPAddress
                            Response.Cookies("APRKeepMeIn")("CID_Print") = CIDArray(0).CID_Print
                            Response.Cookies("APRKeepMeIn")("lastVisit") = DateTime.Now.ToString()
                            Response.Cookies("APRKeepMeIn").Expires = DateTime.Now.AddYears(10)
                        Catch ex As Exception

                        End Try

                        'Response.Redirect("~/Login.aspx")
                    End If

                End If
                    Response.Redirect("~/APR_SiteEntry.aspx?CID=" + strURLCustomer)
                'Response.Redirect("~/Login.aspx")
            End If
            'Application not available

            Response.Redirect("~/ErrorPage.aspx?UC=" + strURLCustomer)
        End Sub
    End Class

End Namespace
