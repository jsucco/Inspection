<%@ WebHandler Language="VB" Class="SPC_InspectionInput_UserInputs" %>

Imports System
Imports System.Web
Imports App.Utilities.Web.Handlers
Imports System.Web.Script.Serialization

Public Class SPC_InspectionInput_UserInputs
    Inherits BaseHandler
    Private jser As New JavaScriptSerializer
    Public Sub UpdateLastUserInputs(InputArString As String, SessionId As String)
        If InputArString.Length > 0 Then
            Dim lastEntered = jser.Deserialize(Of core.SPCInspection.UserInputs)(InputArString)
            If Not lastEntered Is Nothing Then
                InsertIntoCache(lastEntered, SessionId)
            End If
        End If
    End Sub
    Private Sub InsertIntoCache(lastEntered As core.SPCInspection.UserInputs, SessionId As String)
        HttpRuntime.Cache.Insert("InspectionInput_LastEnteredInputs_" + SessionId, lastEntered)
    End Sub
    Public Function getLastUserInputs(SessionId As String) As String
        Dim serializedData As String = ""
        Dim lastInputs = HttpRuntime.Cache("InspectionInput_LastEnteredInputs_" + SessionId)
        If Not lastInputs Is Nothing Then
            serializedData = jser.Serialize(lastInputs)
        End If
        Return serializedData
    End Function
End Class

