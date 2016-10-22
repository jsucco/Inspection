<%@ WebHandler Language="VB" Class="core.NavigationControl" %>

Imports System
Imports System.Web
Imports App.Utilities.Web.Handlers
Imports System.Web.Script.Serialization
Imports System.Data.SqlClient
Imports System.Data
Imports System.Globalization

Namespace core

    Public Class NavigationControl
        Inherits BaseHandler
        Private DAOFactory As New DAOFactory
        
        Public Function GetPermission(ByVal Destination As String) As Boolean
            
            Dim dlayer As New dlayer
            Dim CID As Integer = dlayer.GetSessionCID()
            Dim PermissionsArray As Array
            
            Dim sqlstring As String = "SELECT APRPM_Enabled, APRUtility_Enabled, APRLoom_Enabled, APRInspection_Enabled, APRSPC_Enabled" & vbCrLf &
                                        "FROM corporate" & vbCrLf &
                                        "WHERE (CID = N'000" & CID.ToString() & "')"
            PermissionsArray = DAOFactory.getNavPermissions(sqlstring).ToArray()
            
            If Destination = "APRLoom" Then
                Return PermissionsArray(0).APRLoom_Enabled
            ElseIf Destination = "APRFlagboard" Then
                Return PermissionsArray(0).APRPM_Enabled
            ElseIf Destination = "APRInspect" Then
                Return PermissionsArray(0).APRInspection_Enabled
            ElseIf Destination = "APRUtilities" Then
                Return PermissionsArray(0).APRUtility_Enabled
            ElseIf Destination = "APRSPC" Then
                Return PermissionsArray(0).APRSPC_Enabled
            Else
                Return False
            End If
            
        End Function

    End Class
    
End Namespace