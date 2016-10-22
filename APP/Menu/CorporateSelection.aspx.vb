Imports core.Environment
Imports System.Web.Security
Imports System.Security.Principal
Imports System.Threading
Imports System.Reflection
Imports System.Web.Script.Serialization



Namespace core



    Partial Class login
        Inherits System.Web.UI.Page
        Public CorpList As String = "[0]"

        Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

            Dim corp As New corporate
            Dim jser As New JavaScriptSerializer

            CorpList = jser.Serialize(corp.GetFullCorporateList())

            Dim path As String
            Dim CID As New CID

            path = HttpContext.Current.Request.Url.AbsolutePath
            Dim fullurl As String = HttpContext.Current.Request.Url.AbsoluteUri

            If Not fullurl Is Nothing Then
                Dim fullurlarray = fullurl.Split("/")
                If fullurlarray.Length > 3 Then
                    If fullurlarray(3).ToString() = "APRFINAL" Then
                        Session("BaseUri") = fullurlarray(0).ToString() + "//" + fullurlarray(2).ToString() + "/" + fullurlarray(3).ToString()
                    Else
                        Session("BaseUri") = fullurlarray(0).ToString() + "//" + fullurlarray(2).ToString()
                    End If
                End If
            End If

        End Sub




    End Class


End Namespace