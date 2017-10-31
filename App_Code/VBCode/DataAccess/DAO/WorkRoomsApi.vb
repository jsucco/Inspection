Imports System.Net.Http
Imports System.Net.Http.Headers
Imports System.Web.Script.Serialization

Namespace core

    Public Class WorkRoomsApi
        Inherits System.Web.UI.Page

        Private Shared resultContent As String = ""
        Private Shared errorEx As Exception = Nothing

        Private Shared Async Function GetCached(CID As String) As Threading.Tasks.Task(Of MethodResponse)
            'Dim address As String = "http://coredemo2.standardtextile.com/api/WorkRooms/" + CID
            Dim address As String = "http://inspectservices.standardtextile.com/api/WorkRooms/" + CID
            Try
                Using client As New HttpClient()

                    setAuthorizationHeader(client)
                    client.DefaultRequestHeaders.Accept.Add(New MediaTypeWithQualityHeaderValue("application/json"))

                    Dim result = Await client.GetAsync(address, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(False)
                    Dim response = result.Content.ReadAsStringAsync().Result
                    resultContent = response
                    Return LoadResponse(response)
                End Using
            Catch ex As Exception
                errorEx = ex
            End Try
            Return LoadResponse("")
        End Function

        Public Shared Sub setAuthorizationHeader(client As HttpClient)
            client.DefaultRequestHeaders.Authorization =
                New AuthenticationHeaderValue("Basic",
                                              Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(String.Format("{0}:{1}", "sa", "nimda")))
                                              )
        End Sub

        Public Shared Function GetClientHandler(address As String) As HttpClientHandler
            Dim acookie = HttpContext.Current.Request.Cookies(FormsAuthentication.FormsCookieName)
            Dim uri As Uri = New Uri(address)
            Dim handler = New HttpClientHandler()

            'If Not acookie Is Nothing Then
            '    handler.CookieContainer = New Net.CookieContainer()
            '    handler.CookieContainer.Add(uri, New Net.Cookie(FormsAuthentication.FormsCookieName, acookie.Value))

            '    Return handler
            'End If

            Return handler
        End Function

        Public Shared Function LoadResponse(s As String) As MethodResponse
            Dim jser As New JavaScriptSerializer
            Dim r As MethodResponse
            Try
                r = jser.Deserialize(Of MethodResponse)(s)

                If r.Result = False Then
                    r.Content = New WorkroomsUserObject() {}
                End If
            Catch ex As Exception
                r = New MethodResponse
                r.ErrorMessage = ex.Message
                r.Result = False
                r.Content = New WorkroomsUserObject() {}
            End Try
            Return r
        End Function

        Public Shared Function GetAPIBaseUrl() As String
            Return "http://inspectservices.standardtextile.com"
        End Function

        Public Class MethodResponse
            Public Property Result As Boolean
            Public Property Content As WorkroomsUserObject()
            Public Property ErrorMessage As String
        End Class

        Public Class WorkroomsUserObject
            Public Property Id As Integer
            Public Property Name As String
            Public Property Abbreviation As String
            Public Property Status As Boolean
        End Class

        Public Shared Function GetResult(CID As String) As String
            Dim jser As New JavaScriptSerializer()
            Dim task = GetCached(CID)

            Dim content = task.Result

            If IsNothing(errorEx) = False Then
                Elmah.ErrorSignal.FromCurrentContext().Raise(errorEx)
            End If

            Return jser.Serialize(content.Content)
        End Function
    End Class

End Namespace

