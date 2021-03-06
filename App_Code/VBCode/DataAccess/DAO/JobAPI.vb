﻿Imports System.Net.Http
Imports System.Net.Http.Headers
Imports System.Threading.Tasks
Imports System.Web.Script.Serialization
Imports Microsoft.VisualBasic
Imports System.Web.Ex
Imports Newtonsoft.Json

Public Class JobAPI
    Private Shared jser As JavaScriptSerializer

    Private Shared json As String

    Public Shared Async Function UpdateJobSuggestions(ByVal data As JobHeader) As Task(Of Boolean)

        Dim url = ConfigurationManager.AppSettings("InspectServicesBaseUrl")

        Dim json As String = JsonConvert.SerializeObject(data)

        Using client As New HttpClient()
            setAuthorizationHeader(client)
            client.DefaultRequestHeaders.Accept.Add(New MediaTypeWithQualityHeaderValue("application/json"))

            Dim result = Await client.PostAsJsonAsync(Of JobHeader)("http://" + url + "/api/SuggestJob", data).ConfigureAwait(False)
            Dim response = result.Content.ReadAsStringAsync().Result
        End Using
        Return True
    End Function

    Public Shared Sub UpdateJobSuggestionsAsync(ByVal data As JobHeader)
        Try
            UpdateJobSuggestions(data)


            'Dim t As System.Threading.Tasks.Task = System.Threading.Tasks.Task.Run(Sub()
            '                                                                           UpdateJobSuggestions(data)
            '                                                                       End Sub)
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
        End Try

    End Sub

    Public Shared Sub setAuthorizationHeader(client As HttpClient)
        client.DefaultRequestHeaders.Authorization =
            New AuthenticationHeaderValue("Basic",
                                          Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(String.Format("{0}:{1}", "sa", "nimda")))
                                          )
    End Sub
End Class

Public Class JobHeader
    Public Property id As Int64
    Public Property Name As String
    Public Property cid As String
End Class
